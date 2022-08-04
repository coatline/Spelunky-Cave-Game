using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage;

    private void Awake()
    {
        gameObject.tag = "Arrow";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow")) { return; }
        Destroy(gameObject);
    }
}
