using UnityEngine;
using System.Collections;

public class enemy_slime : Enemy
{
    public GameObject[] drop;
    public Transform player;
    public float moveSpeed = 2f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;
    private bool facingRight = true;
    private Vector3 previousPosition;
    public float distanceThreshold = 2f;
    public float pushForce = 10f;  // Сила отталкивания игрока
    private Coroutine damageCoroutine;  // Корутина для нанесения урона

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (!dead)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (player != null && Vector2.Distance(transform.position, player.position) <= distanceThreshold)
            {
                MoveTowardsPlayer();
                anim.Play("Enemy Run");
            }
            else
            {
                anim.Play("Enemy Idle");
            }

            Jump();
            previousPosition = transform.position;
            FlipTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) <= distanceThreshold)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }
    }

    void Jump()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.3f);
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        float maxDistance = Random.Range(0.01f, 0.5f);
        LayerMask groundLayerMask = LayerMask.GetMask("ground");
        RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, maxDistance, groundLayerMask);

        if (hitInfo.collider != null && isGrounded)
        {
            Debug.Log("это " + hitInfo.collider.gameObject.name);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        Debug.DrawRay(origin, direction * maxDistance, Color.red);
    }

    void FlipTowardsPlayer()
    {
        if (player != null)
        {
            if ((player.position.x > transform.position.x && !facingRight) ||
                (player.position.x < transform.position.x && facingRight))
            {
                facingRight = !facingRight;
                Vector3 scaler = transform.localScale;
                scaler.x *= -1;
                transform.localScale = scaler;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        if (health <= 0)
        {
            SpawnLoot();
        }
    }

    public override void TakeUltDamage()
    {
        base.TakeUltDamage();
        if (health <= 0)
        {
            SpawnLoot();
        }
    }

    private void SpawnLoot()
    {
        foreach (var item in drop)
        {
            GameObject obj = Instantiate(item, transform.position, Quaternion.identity);
            Rigidbody2D rbObj = obj.GetComponent<Rigidbody2D>();
            rbObj.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 pushDirection = (playerRb.position - rb.position).normalized;
                playerRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);

                PlayerCombat playerCombat = collision.collider.GetComponent<PlayerCombat>();
                if (playerCombat != null)
                {
                    if (damageCoroutine == null)
                    {
                        damageCoroutine = StartCoroutine(DealDamageOverTime(playerCombat));
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DealDamageOverTime(PlayerCombat playerCombat)
    {
        yield return new WaitForSeconds(1f); // Ожидание 2 секунды перед нанесением урона
        while (true)
        {
            if (!playerCombat.isAttacking || PlayerMove.isDashing)  // Проверяем, атакует ли игрок или использует деш
            {
                playerCombat.TakeDamage(1);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
