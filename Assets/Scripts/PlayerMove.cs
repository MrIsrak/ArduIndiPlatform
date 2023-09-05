using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEngine;
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Transform footPos;
    [SerializeField] private LayerMask groundMask;

    private float horizontalInput;
    public float speed;
    public float jumpforce = 3500000;
    private bool isOnGround;
    private float CheckRadius = 0.05f;
    private bool isFlipping;
    private float flipCooldown = 0.1f; 
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Vector2 moveVelocity;
    private Vector2 moveVector;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }
    private void Update()
    {
        Move();
        Jump();
        CheckGround();
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

    }
    

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        moveVector.x = Input.GetAxis("Horizontal");
        moveVelocity = Vector2.right * speed * horizontalInput;

        anim.SetFloat("Move X", Mathf.Abs(moveVector.x));


        if (horizontalInput < 0 && !isFlipping)
        {
            spriteRenderer.flipX = true; // Отразить, если идем влево
            isFlipping = true;
            StartCoroutine(ResetFlipCooldown());
        }
        else if (horizontalInput > 0 && !isFlipping)
        {
            spriteRenderer.flipX = false; // Не отражать, если идем вправо
            isFlipping = true;
            StartCoroutine(ResetFlipCooldown());
        }

        
    }

    private IEnumerator ResetFlipCooldown()
    {
        yield return new WaitForSeconds(flipCooldown);
        isFlipping = false;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(Vector2.up * jumpforce);
        }
    }
    private void CheckGround()
    {
        isOnGround = Physics2D.OverlapCircle(footPos.position, CheckRadius, groundMask);
    }
}