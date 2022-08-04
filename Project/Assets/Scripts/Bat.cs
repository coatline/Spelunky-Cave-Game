using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField] float eyesight = 10f;
    [SerializeField] float speed = .1f;

    bool targetAquired;
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        if (!targetAquired)
        {
            var direction = player.transform.position - transform.position;
            var distance = direction.magnitude;

            var directionNormalized = direction / distance;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionNormalized, eyesight);

            Debug.DrawRay(transform.position, hit.point, Color.blue);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    targetAquired = true;
                }
            }
        }
        else
        {
            Fly();
            Debug.DrawRay(transform.position, player.transform.position, Color.blue);
        }
    }

    void Fly()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
    }

}
