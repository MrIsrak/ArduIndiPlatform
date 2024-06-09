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
        
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;

        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
    }

    //private bool faceRight = false;

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // �������� ���� �� �����������.
        anim.SetFloat("Move X", Mathf.Abs(horizontalInput)); // ������������� �������� �������� "Move X" �� ������ ����� �� �����������.

        if (horizontalInput < 0)
        {
            //faceRight = false;
            spriteRenderer.flipX = true; // Flip the sprite horizontally if moving left
        }
        else if (horizontalInput > 0)
        {
            //faceRight = true;
            spriteRenderer.flipX = false; // Unflip the sprite horizontally if
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(Vector2.up * jumpforce); // ������������ ������� ������� ��� �������� ������.
            //rb.velocity += new Vector2(0, 10000);
            //rb.velocity = (Vector3.up * jumpforce);
        }
    }

    private void CheckGround()
    {
        isOnGround = Physics2D.OverlapCircle(footPos.position, CheckRadius, groundMask); // ���������, ��������� �� �������� �� �����.
        anim.SetBool("onGround", isOnGround); // ������������� �������� �������� "onGround" � ����������� �� ��������� �� �����.
    }


    [SerializeField] int dashForce;

    private void CheckDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) // �������� �������� !isDashing, ����� �� ��������� Dash, ���� ��� �����������
        {
            StartCoroutine("Dash");
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        anim.SetBool("isDashing", true);

        int lookDirection=1;
        if (spriteRenderer.flipX)
            lookDirection = -1;

        rb.velocity = new Vector2(lookDirection * dashForce, 0f);
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        anim.SetBool("isDashing", false);

        yield return new WaitForSeconds(dashRechargeTime);    
        canDash = true;
    }

}