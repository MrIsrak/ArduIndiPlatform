using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float beamLength = 2.0f; 
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
        Vector2 startPosition = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.1f);
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;

        RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition, direction, beamLength);
        Debug.DrawLine(startPosition, startPosition + direction * beamLength, Color.red);

        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    continue;
                }
                if (hit.collider.CompareTag("enemy") && PlayerMove.isDashing == false)
                {
                    Debug.Log("Hit Enemy: " + hit.collider.name);

                    // Find the component with the damage method
                    var enemyScript = hit.collider.GetComponent<MonoBehaviour>();
                    if (enemyScript != null)
                    {
                        var method = enemyScript.GetType().GetMethod("damage");



                        Rigidbody2D enemyRb = hit.collider.GetComponent<Rigidbody2D>();
                        Vector2 pushDirection = (enemyRb.position - rb.position).normalized;
                        enemyRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);



                        if (method != null)
                        {
                            method.Invoke(enemyScript, null);
                        }
                    }
                }
                else if (hit.collider.CompareTag("enemy") && PlayerMove.isDashing == true)
                    {
                        Debug.Log("Hit Enemy: " + hit.collider.name);

                        // Find the component with the damage method
                        var enemyScript = hit.collider.GetComponent<MonoBehaviour>();
                        if (enemyScript != null)
                        {

                        Rigidbody2D enemyRb = hit.collider.GetComponent<Rigidbody2D>();
                        Vector2 pushDirection = (enemyRb.position - rb.position).normalized;
                        enemyRb.AddForce(pushDirection * pushForce * 3, ForceMode2D.Impulse);

                        var method = enemyScript.GetType().GetMethod("ult_damage");
                            if (method != null)
                            {
                                method.Invoke(enemyScript, null);
                            }
                        }
                    }
                break;
            }
        }
    }
}
