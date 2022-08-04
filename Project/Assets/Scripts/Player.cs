using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] Bomb bombPrefab = null;
    [SerializeField] Arrow arrowPrefab = null;
    [SerializeField] Vector2 bombSpeed = new Vector2(4, 3);
    [SerializeField] Vector2 arrowForce = new Vector2(10, 5);
    [SerializeField] Sprite deadSprite = null;
    [SerializeField] float bombTime = 3;
    [SerializeField] float speed = 7;
    [SerializeField] float jumpHeight = 12;
    [SerializeField] float shotRate = .5f;

    Animator a;
    BoxCollider2D col;
    Rigidbody2D rb;
    SpriteRenderer sr;

    bool canJump = true;
    bool canPickupItem;
    bool canMove = true;

    public int maxHealth;

    public int playerHealth;

    public Weapon currentWeapon;
    public Bow currentBow;

    public int WeaponDamage
    {
        get
        {
            if (currentWeapon == null)
            {
                Debug.LogWarning("You don't have a weapon!");
                return Random.Range(1, 3);
            }
            else
            {
                var damageRange = Random.Range(currentWeapon.minDamage, currentWeapon.maxDamage + 1);
                return damageRange;
            }

        }
    }

    public bool Dead
    {
        get
        {
            if(playerHealth > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public int BowDamage
    {
        get
        {
            var damageRange = Random.Range(currentBow.minDamage, currentBow.maxDamage + 1);
            return damageRange;
        }
    }


    void Start()
    {
        transform.parent = null;

        if (Keep.I.level == 1)
        {
            currentWeapon = WeaponGenerator.I.startingWeapon;
            currentBow = BowGenerator.I.startingBow;

            print(currentBow.myName);

            Keep.I.SetText("Weapon", currentWeapon.myName);
            Keep.I.SetText("Bow", null, currentBow.myName);

            playerHealth = maxHealth;

            Keep.I.currentWeapon = currentWeapon;
            Keep.I.currentBow = currentBow;
        }
        else if (Keep.I.level > 1)
        {
            playerHealth = Keep.I.playerHealth;
            currentWeapon = Keep.I.currentWeapon;
            currentBow = Keep.I.currentBow;

            if (currentWeapon == null)
            {
                Keep.I.SetText("Weapon", "Fists");
            }
            else
            {
                Keep.I.SetText("Weapon", currentWeapon.myName);
            }

            if (currentBow == null)
            {
                Keep.I.SetText("Bow", null, "empty");
            }
            else
            {
                Keep.I.SetText("Bow", null, currentBow.myName);
            }
        }

        UpdateHealthText();

        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        a = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    Item overItem;

    void Update()
    {
        Movement();
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E)&&overItem)
        {

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy") && collider.IsTouching(col))
        {
            var enemy = collider.gameObject.GetComponentInParent<Enemy>();

            if (enemy.enemyType == "Slime")
            {
                if (collider.gameObject.GetComponentInParent<Slime>().jumping == false)
                {
                    return;
                }
            }

            playerHealth -= enemy.Damage();

            Keep.I.playerHealth = playerHealth;

            if (Keep.I.healthText != null)
            {
                UpdateHealthText();
            }
            else
            {
                Keep.I.FindTexts();
                UpdateHealthText();
            }

            PlayHurtAnimation();

            CheckForDeath();

            Keep.I.NewDamageText(enemy.Damage(), transform, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pickup") && overItem)
        {
            overItem = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pickup"))
        {
            if (!canPickupItem)
            {
                return;
            }


            var script = collision.gameObject.GetComponent<Item>();
            overItem = script;

            if (script.type == "Weapon")
            {
                if (currentWeapon != null)
                {
                    DropWeapon();
                }

                currentWeapon = script.weapon;

                Keep.I.SetText("Weapon", currentWeapon.myName);
            }
            else if (script.type == "Bow")
            {
                if (currentBow != null)
                {
                    DropBow();
                }

                currentBow = script.bow;

                Keep.I.SetText("Bow", null, currentBow.myName);
            }

            Destroy(collision.gameObject);

            canPickupItem = false;
        }
    }

    void DropWeapon()
    {
        var gob = new GameObject();

        gob.transform.position = transform.position + new Vector3(0, .1f, 0);

        gob.AddComponent<Item>();

        var itemInfo = gob.GetComponent<Item>();

        itemInfo.name = currentWeapon.myName;

        gob.name = itemInfo.name;

        gob.tag = "Pickup";

        var col = gob.AddComponent<BoxCollider2D>();

        col.isTrigger = true;

        itemInfo.type = "Weapon";

        gob.AddComponent<SpriteRenderer>();

        gob.GetComponent<SpriteRenderer>().sprite = currentWeapon.mySprite;

        itemInfo.callStart = false;

        itemInfo.weapon = currentWeapon;

        currentWeapon = null;
    }

    void DropBow()
    {
        var gob = new GameObject();

        gob.transform.position = transform.position + new Vector3(0, .1f, 0);

        gob.AddComponent<Item>();

        var itemInfo = gob.GetComponent<Item>();

        itemInfo.name = currentBow.myName;

        gob.name = itemInfo.name;

        gob.tag = "Pickup";

        var col = gob.AddComponent<BoxCollider2D>();

        col.isTrigger = true;

        itemInfo.type = "Bow";

        gob.AddComponent<SpriteRenderer>();

        gob.GetComponent<SpriteRenderer>().sprite = currentBow.mySprite;

        itemInfo.callStart = false;

        itemInfo.bow = currentBow;

        currentBow = null;
    }

    public void PlayHurtAnimation()
    {
        a.Play("Player_Hurt");
    }

    public void Die()
    {
        canMove = false;

        sr.sprite = deadSprite;

        Fader fader = FindObjectOfType<Fader>();
        fader.ChangeState(Fader.State.fadeOut);

    }

    void ThrowBomb()
    {
        var newBomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);

        newBomb.GetComponent<Rigidbody2D>().velocity = bombSpeed * transform.forward;

        newBomb.GetComponent<Bomb>().time = bombTime;
    }

    public void CheckForDeath()
    {
        if (playerHealth <= 0)
        {
            if (playerHealth < 0)
            {
                playerHealth = 0;
                UpdateHealthText();
            }

            Die();
        }
    }

    public void UpdateHealthText()
    {
        Keep.I.SetText("Health", null, null, playerHealth, maxHealth);
    }

    bool right = false;
    float timer;

    void Movement()
    {
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        float dx = Input.GetAxisRaw("Horizontal");
        bool jump = Input.GetButton("Jump");

        rb.velocity = new Vector2(speed * dx, rb.velocity.y);

        var mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);


        // if mouse is on the left of the screen
        if (mousePos.x < .5f)
        {
            transform.rotation = Quaternion.identity;
            right = false;
        }

        // if mouse is on the right of the screen
        else if (mousePos.x >= .5f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            right = true;
        }

        if (jump && canJump)
        {
            canJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }

        if (Input.GetMouseButtonDown(0))
        {
            a.Play("Player_Hit");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowBomb();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            canPickupItem = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentWeapon != null)
            {
                DropWeapon();
                Keep.I.SetText("Weapon", "Fists");
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (timer < shotRate)
            {
                return;
            }

            for (int i = 0; i < currentBow.arrowNumber; i++)
            {
                var newArrow = Instantiate(arrowPrefab, transform.position + new Vector3(0, i / 4), Quaternion.identity);

                if (right)
                {
                    newArrow.GetComponent<Rigidbody2D>().velocity = new Vector2(arrowForce.x + Random.Range(-arrowForce.x / 10, arrowForce.x / 5), arrowForce.y + Random.Range(-arrowForce.y / 4f, arrowForce.y / 2f));
                    newArrow.GetComponent<Arrow>().damage = BowDamage;
                }

                else if (!right)
                {
                    newArrow.transform.rotation = Quaternion.Euler(0, 0, 180);
                    newArrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-arrowForce.x + Random.Range(-arrowForce.x / 10, arrowForce.x / 5), arrowForce.y + Random.Range(-arrowForce.y / 4f, arrowForce.y / 2f));
                    newArrow.GetComponent<Arrow>().damage = BowDamage;
                }
            }

            timer = 0;
        }
    }
}
