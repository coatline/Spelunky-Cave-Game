using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField] float flapForce = 30f;
    [SerializeField] float eyesight = 10f;
    [SerializeField] float speed = 0.1f;
    [SerializeField] float flapInterval = 0.3f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Sprite flapSprite;
    [SerializeField] Sprite normalSprite;
    [SerializeField] SpriteRenderer sr;

    bool targetAquired;
    Player player;
    float flapTimer;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        Vector3 direction = (player.transform.position - transform.position).normalized;

        if (!targetAquired)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, eyesight, LayerMask.GetMask("Player"));

            Debug.DrawLine(transform.position, transform.position + direction * eyesight, Color.red);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
                rb.simulated = true;
                targetAquired = true;
            }
        }
        else
        {
            if (flapTimer >= flapInterval * 0.2f)
                sr.sprite = normalSprite;

            if (flapTimer < flapInterval)
            {
                flapTimer += Time.fixedDeltaTime;
                return;
            }

            flapTimer = 0f;
            rb.linearVelocityY = 0f;
            sr.sprite = flapSprite;

            if (direction.y > 0)
                direction.y = 1;

            rb.AddForce(flapForce * direction * Time.fixedDeltaTime * Mathf.Max(1, (player.transform.position - transform.position).magnitude * 0.5f));
            Debug.DrawLine(transform.position, player.transform.position, Color.green);
        }
    }
}