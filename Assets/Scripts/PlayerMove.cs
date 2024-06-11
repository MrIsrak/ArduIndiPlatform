using System.Collections; // ����������� ������������ ���� ��� ������ � ����������� (��������, �������� � ���������).
using System.Collections.Generic; // ����������� ������������ ���� ��� ������ � ����������� �����������.
using System.ComponentModel; // ����������� ������������ ���� ��� ������ � ������������ � ����������.
using System.Security.Cryptography;
using System.Text;
using System.Threading; // ����������� ������������ ���� ��� ������ � ��������.
using UnityEngine; // ����������� ������������ ���� Unity ��� ������� � ���������������� ������.

public class PlayerMove : MonoBehaviour // ��������� ����� PlayerMove, ������� ����������� �� MonoBehaviour (�������� ������ ��� �������� � Unity).
{
    [SerializeField] private Transform footPos; // ���� footPos ��� ������ �� ��������� �������, ��������������� ������� ��� ���������.
    [SerializeField] private LayerMask groundMask; // ���� groundMask ��� ������� ����� ���� �����.

    private float horizontalInput; // ���������� ��� �������� ����� �� ����������� (��������, ������� A/D ��� �������).
    private float speed = 6f; // ��������� ���� ��� ��������� �������� ����������� ���������.
    public float jumpforce = 15000f; // ��������� ���� ��� ��������� ���� ������ ���������.

    bool isDashing = false;
    bool canDash = true;
    float dashRechargeTime = 0.5f;
    float dashDuration=0.7f;

    private bool isOnGround; // ���������� ��� ������������, ��������� �� �������� �� �����.
    private float CheckRadius = 0.05f; // ������ �������� ����� ��� ������������� Physics2D.OverlapCircle.


    private SpriteRenderer spriteRenderer; // ���������� ��� ������� � ���������� SpriteRenderer ������� (��� ��������� ����������� �������).
    private Animator anim; // ���������� ��� ������� � ���������� Animator ������� (��� ���������� ����������).
    private Vector2 moveVelocity; // ������ ��� �������� �������� �������� �� �����������.
    private Vector2 moveVector; // ������ ��� �������� ����� �������� �� �����������.
    private Rigidbody2D rb; // ���������� ��� ������� � ���������� Rigidbody2D ������� (��� ������).

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // �������� ��������� Rigidbody2D �������.
        spriteRenderer = GetComponent<SpriteRenderer>(); // �������� ��������� SpriteRenderer �������.
        anim = GetComponent<Animator>(); // �������� ��������� Animator �������.
    }

    private void Update()
    {
        if (isDashing)
            return;
        rb.velocity = new Vector2(rb.velocity.x, Physics2D.gravity.y);
        Move(); // �������� ����� ��� ��������� ��������.
        Jump(); // �������� ����� ��� ��������� ������.
        CheckDash();
        CheckGround(); // �������� ����� ��� ��������, ��������� �� �������� �� �����.
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;

        
    }


    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // �������� ���� �� �����������.

        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true; // Flip the sprite horizontally if moving left
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false; // Unflip the sprite horizontally if
        }
    }

    private void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround == true)
        {
            print("asfasd");
            rb.AddForce(transform.up * jumpforce, ForceMode2D.Impulse);
        }
    }

    private void CheckGround()
    {
        isOnGround = Physics2D.OverlapCircle(footPos.position, CheckRadius, groundMask); // ���������, ��������� �� �������� �� �����.
        if(isOnGround == false)
        {
            anim.Play("JumptoFall");
        }
        else
        {
            anim.Play("Idle");
        }
    }


    [SerializeField] int dashForce;

    private void CheckDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing) // Added check for !isDashing
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        anim.Play("Dash");

        int lookDirection = 1;
        if (spriteRenderer.flipX)
            lookDirection = -1;

        rb.velocity = new Vector2(lookDirection * dashForce, 0f);
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        yield return new WaitForSeconds(dashRechargeTime);
        canDash = true;
    }


}