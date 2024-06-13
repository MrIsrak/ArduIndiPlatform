using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private float beamLength = 0.7f;
    private Rigidbody2D rb;
    private bool facingRight = true;
    public float pushForce = 40f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        if (rb.velocity.x > 0.01f)
        {
            facingRight = true;
        }
        else if (rb.velocity.x < -0.01f)
        {
            facingRight = false;
        }
    }

    public void cast()
    {
        Vector2 startPosition = new Vector2(transform.position.x - 0.2f, transform.position.y - 0.1f);
        Vector2 endPosition = startPosition + (facingRight ? Vector2.right : Vector2.left) * beamLength;

        RaycastHit2D[] hits = Physics2D.LinecastAll(startPosition, endPosition);
        Debug.DrawLine(startPosition, endPosition, Color.red);

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

                    // Find the component with the damage method
                    var enemyScript = hit.collider.GetComponent<MonoBehaviour>();
                    if (enemyScript != null)
                    {
                        Rigidbody2D enemyRb = hit.collider.GetComponent<Rigidbody2D>();

                        // Get the player position
                        Vector2 playerPosition = rb.position;

                        // Get the enemy position
                        Vector2 enemyPosition = enemyRb.position;

                        // Determine the push direction from player to enemy
                        Vector2 pushDirection = (enemyPosition - playerPosition).normalized;

                        // Apply push force in the direction from player to enemy
                        enemyRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);

                        // Call the appropriate damage method based on the dashing state
                        var methodName = PlayerMove.isDashing ? "ult_damage" : "damage";
                        var method = enemyScript.GetType().GetMethod(methodName);
                        if (method != null)
                        {
                            method.Invoke(enemyScript, null);
                        }
                    }
                }
            }
        }
    }
}
