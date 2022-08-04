using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] float jumpHeight = 0;
    [SerializeField] float xVelocityLimit = 0;
    [SerializeField] float timeBetweenJumps = 0;

    public ParticleSystem ps;
    public bool isBigSlime = false;
    public bool jumping;

    Rigidbody2D rb;
    Player player;
    Animator a;

    float jumpTimer = 0;

    float randFactor = 0;


    void Start()
    {

        ps = GetComponentInChildren<ParticleSystem>();
        a = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        randFactor = Random.Range(0, timeBetweenJumps / 2);

        a.speed = Random.Range(.8f, 1.4f);
    }


    void Update()
    {
        transform.rotation = Quaternion.identity;

        if (jumpTimer >= timeBetweenJumps - randFactor)
        {
            randFactor = Random.Range(0, timeBetweenJumps / 3);
            jumpTimer = 0;
            Jump();
        }
        else
        {
            jumpTimer += Time.deltaTime;
        }

        if (ps.particleCount >= 15)
        {
            ps.Stop();
        }
    }

    void Jump()
    {
        var randomFactor = Random.Range(jumpHeight / 2, jumpHeight + jumpHeight / 2);
        float xVel = 0;

        //player is to the left of slime
        if (player.transform.position.x < transform.position.x)
        {
            xVel = Random.Range(-1, -xVelocityLimit);
            jumping = true;

        }

        //player is to the right of slime
        else
        {
            xVel = Random.Range(1, xVelocityLimit);
            jumping = true;

        }

        rb.velocity = new Vector2(xVel, randomFactor);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        jumping = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (jumping)
        {
            jumping = false;
        }
    }
}
