using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] int damage = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player Feet"))
        {
            var rb = collision.gameObject.GetComponentInParent<Rigidbody2D>();

            if (rb.velocity.y < 0)
            {
                Keep.I.NewDamageText(damage, transform, true);

                var script = collision.gameObject.GetComponentInParent<Player>();

                script.playerHealth -= damage;
                script.UpdateHealthText();
                script.CheckForDeath();
                script.PlayHurtAnimation();

                if(!script.Dead)
                {
                    collision.gameObject.GetComponentInParent<Rigidbody2D>().AddForce(new Vector2(0, 1000));
                }
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            var script = collision.gameObject.GetComponentInParent<Enemy>();

            if (script.dead) { return; }

            var rb = collision.gameObject.GetComponentInParent<Rigidbody2D>();

            if (rb.velocity.y < 0)
            {
                Keep.I.NewDamageText(damage, transform);
                script.health -= damage;
                script.CheckForDeath();
            }
        }
    }
}
