using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [Header("UI")]
    public GameObject dasher;



    [Header("Settings")]
    [SerializeField] private Transform footPos;
    [SerializeField] private LayerMask groundMask;

    private float horizontalInput;
    private float speed = 3f;
    public float jumpforce = 15f;

    private bool isDashing = false;
    private bool canDash = true;
    private float dashRechargeTime = 0.5f;
    private float dashDuration = 0.7f;

    private bool isOnGround;
    private float CheckRadius = 0.05f;

    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] private int dashForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Проверка на ускорение
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            
            StartCoroutine(Dash());
            return;
        }

        Move();
        CheckGround();

        if (isOnGround && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (!isDashing) // Обновление скорости только если не идет ускорение
        {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }
    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void CheckGround()
    {
        isOnGround = Physics2D.OverlapCircle(footPos.position, CheckRadius, groundMask);

        if (!isOnGround)
        {
            anim.Play("JumptoFall");
        }
        else if(isOnGround == true && canDash == true)
        {
            anim.Play("Idle");
        }
        else if( isOnGround ==  true && canDash == false)
        {
            return;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        anim.Play("Dash");

        Animator animator = dasher.GetComponent<Animator>();
        animator.speed = dashRechargeTime + 0.2f;
        animator.Play("dasher");

        int lookDirection = spriteRenderer.flipX ? -1 : 1;
        rb.velocity = new Vector2(lookDirection * dashForce, rb.velocity.y);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        yield return new WaitForSeconds(dashRechargeTime);
        canDash = true;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);
    }
}
