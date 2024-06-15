using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private float beamLength = -8f; // Увеличена длина луча для атаки
    private Rigidbody2D rb;
    public float pushForce = 40f;
    public int health = 3; // Здоровье игрока
    public bool isAttacking = false; // Флаг атаки

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    public void Cast()
    {
        Vector2 startPosition = (Vector2)transform.position;
        Vector2 direction = rb.velocity.normalized;
        Vector2 endPosition = startPosition + direction * beamLength;

        Debug.DrawRay(startPosition, direction * beamLength, Color.red, 0.5f);

        RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition, direction, beamLength);

        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    continue;
                }

                if (hit.collider.CompareTag("enemy"))
                {
                    Debug.Log("Hit Enemy: " + hit.collider.name);
                    var enemyScript = hit.collider.GetComponent<IEnemy>();

                    if (enemyScript != null)
                    {
                        Rigidbody2D enemyRb = hit.collider.GetComponent<Rigidbody2D>();
                        if (enemyRb != null)
                        {
                            Vector2 playerPosition = rb.position;
                            Vector2 enemyPosition = enemyRb.position;
                            Vector2 pushDirection = (enemyPosition - playerPosition).normalized;

                            if (!PlayerMove.isDashing)
                            {
                                enemyRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                            }
                            else
                            {
                                enemyRb.AddForce(pushDirection * pushForce * 2, ForceMode2D.Impulse);
                            }

                            if (enemyScript.Health <= 0)
                            {
                                return;
                            }

                            if (PlayerMove.isDashing)
                            {
                                enemyScript.TakeUltDamage();
                            }
                            else
                            {
                                enemyScript.TakeDamage();
                            }
                        }
                    }
                }
            }
        }
    }

    // Метод для нанесения урона игроку
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player Health: " + health);
        if (health <= 0)
        {
            Debug.Log("Player is dead");
        }
    }

    // Метод для обработки начала атаки
    public void StartAttack()
    {
        isAttacking = true;
        Cast(); // Выпуск луча при начале атаки
    }

    // Метод для обработки завершения атаки
    public void EndAttack()
    {
        isAttacking = false;
    }
}
