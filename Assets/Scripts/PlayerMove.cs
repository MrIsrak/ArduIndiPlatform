using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Transform footPos;
    [SerializeField] private LayerMask groundMask;

    private float horizontalInput;
    private float speed = 6f;
    public float jumpforce = 15f;

    private bool isDashing = false;
    private bool canDash = true;
    private float dashRechargeTime = 0.5f;
    private float dashDuration = 0.7f;

    private bool isOnGround;
    private float CheckRadius = 0.05f;

    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Vector2 moveVelocity;
    private Vector2 moveVector;
    private Rigidbody2D rb;

    [SerializeField] int dashForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDashing)
            return;

        Move();
        CheckDash();
        CheckGround();

        if (isOnGround && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;
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

        if (isOnGround == false)
        {
            anim.Play("JumptoFall");
        }
        else
        {
            anim.Play("Idle");
        }
    }

    private void CheckDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        anim.Play("Dash");

        int lookDirection = spriteRenderer.flipX ? -1 : 1;
        rb.velocity = new Vector2(lookDirection * dashForce, 0f);

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
