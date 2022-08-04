using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] TMP_Text dmgTxtPrfb = null;
    [SerializeField] int chanceToDropItem = 0;
    [SerializeField] int maxHealth = 25;
    public int health = 0;
    public bool dead;

    public string enemyType = "Slime";

    public int minDamage = 0;
    public int maxDamage = 0;


    void Start()
    {
        transform.tag = "Enemy";

        health = maxHealth;
    }

    public int Damage()
    {
        return Random.Range(minDamage, maxDamage + 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow") && !dead)
        {
            if (enemyType == "Slime")
            {
                GetComponent<Slime>().ps.Play();
            }

            var arrow = collision.gameObject.GetComponent<Arrow>();

            health -= arrow.damage;

            Destroy(collision.gameObject);

            Keep.I.NewDamageText(arrow.damage, transform);

            CheckForDeath();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon") && !dead)
        {

            var playerScript = collision.gameObject.GetComponentInParent<Player>();
            health -= playerScript.WeaponDamage;

            Keep.I.NewDamageText(playerScript.WeaponDamage, transform);

            if (enemyType == "Slime")
            {
                GetComponent<Slime>().ps.Play();
            }

            CheckForDeath();
        }

    }

    public void CheckForDeath()
    {
        if (health <= 0 && !dead)
        {
            var randomChance = Random.Range(0, 100);

            if (chanceToDropItem >= randomChance)
            {
                Keep.I.CreateItem(transform);
            }

            Destroy(gameObject);

            dead = true;
        }
    }

}
