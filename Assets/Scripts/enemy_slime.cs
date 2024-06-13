using UnityEditorInternal;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int heal = 5;
    private bool dead = false;

    public Transform player; // ������ �� ������ ������
    public float moveSpeed = 2f; // �������� �������� ����������
    public float jumpForce = 5f; // ���� ������ ����������
    public Transform groundCheck; // ����� ��� �������� ������� �����
    public float groundCheckRadius = 0.2f; // ������ �������� ������� �����
    public LayerMask groundLayer; // ���� �����
    public GameObject text_1;
    public GameObject text_2;

    private Animator anim;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;
    private Vector3 previousPosition;
    private Vector3 transformer;
    public float distanceThreshold = 2f; // ����������, �� ������� �������� ������ �� �������


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (dead == false)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            // �������� ���������� �� ������
            if (player != null && Vector2.Distance(transform.position, player.position) <= distanceThreshold)
            {
                // ����������� ���������� � ������
                MoveTowardsPlayer();
                anim.Play("Enemy Run");
            }
            else
            {
                anim.Play("Enemy Idle");
            }

            Jump();

            // ���������� ���������� �������
            previousPosition = transform.position;

            // �������������� ���������� � ����������� �� ����������� ��������
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
        RaycastHit2D hitInfo;

        // ���������� ������� ������ ���� � �������� ������ ����� �������
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.3f);

        // ���������� ����������� ���� � ����������� �� ����������� �������
        Vector2 direction;
        if (transform.localScale.x > 0)
        {
            direction = Vector2.right; // ���� ������ �� ����������, ��� ��������� ������
        }
        else
        {
            direction = Vector2.left; // ���� ������ ����������, ��� ��������� �����
        }

        float maxDistance = Random.Range(0.01f, 0.5f);
        LayerMask groundLayerMask = LayerMask.GetMask("ground");
        hitInfo = Physics2D.Raycast(origin, direction, maxDistance, groundLayerMask);

        if (hitInfo.collider != null && isGrounded)
        {
            Debug.Log("��� " + hitInfo.collider.gameObject.name);
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
        // ��������� ������� �������� ������� ����� � ������ �����
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }





    public void damage()
    {
        heal--;
        print(heal);
        anim.Play("Enemy Hit");
        if (heal <= 0)
        {
            dead = true;
            anim.Play("Enemy Death");
        }
        else
        {
            transformer = rb.transform.position;
            Instantiate(text_1, transformer, Quaternion.Euler(0, 0, 0));
            anim.Play("Enemy Hit");
        }
    }

    public void ult_damage()
    {
        heal -= 2;
        print(heal);
        anim.Play("Enemy Hit");
        if (heal <= 0)
        {
            dead = true;
            anim.Play("Enemy Death");
        }
        else
        {
            Instantiate(text_2, transformer, Quaternion.Euler(0, 0, 0));
            anim.Play("Enemy Hit");
        }
    }
}
