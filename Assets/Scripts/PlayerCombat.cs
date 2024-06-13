using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private float beamLength = -4f;
    private Rigidbody2D rb;
    public float pushForce = 40f;

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
        // Начальная точка внутри игрока
        Vector2 startPosition = (Vector2)transform.position;

        // Определяем направление луча на основе скорости движения игрока
        Vector2 direction = rb.velocity.normalized; // Направление движения игрока

        Vector2 endPosition = startPosition + direction * beamLength;

        Debug.Log($"Start: {startPosition}, End: {endPosition}");

        // Отладочная визуализация луча
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

                    var enemyScript = hit.collider.GetComponent<EnemyAI>(); // Получаем скрипт EnemyAI
                    if (enemyScript != null)
                    {
                        Rigidbody2D enemyRb = hit.collider.GetComponent<Rigidbody2D>();

                        if (enemyRb != null)
                        {
                            Vector2 playerPosition = rb.position;
                            Vector2 enemyPosition = enemyRb.position;

                            Vector2 pushDirection = (enemyPosition - playerPosition).normalized;
                            enemyRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);

                            // Выбираем метод в зависимости от isDashing
                            if (PlayerMove.isDashing)
                            {
                                enemyScript.ult_damage();
                            }
                            else
                            {
                                enemyScript.damage();
                            }
                        }
                    }
                }
            }
        }
    }

}
