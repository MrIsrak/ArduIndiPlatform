using System.Collections; // ����������� ������������ ���� ��� ������ � ����������� (��������, �������� � ���������).
using System.Collections.Generic; // ����������� ������������ ���� ��� ������ � ����������� �����������.
using System.ComponentModel; // ����������� ������������ ���� ��� ������ � ������������ � ����������.
using System.Security.Cryptography;
using System.Threading; // ����������� ������������ ���� ��� ������ � ��������.
using UnityEngine; // ����������� ������������ ���� Unity ��� ������� � ���������������� ������.

public class PlayerMove : MonoBehaviour // ��������� ����� PlayerMove, ������� ����������� �� MonoBehaviour (�������� ������ ��� �������� � Unity).
{
    [SerializeField] private Transform footPos; // ���� footPos ��� ������ �� ��������� �������, ��������������� ������� ��� ���������.
    [SerializeField] private LayerMask groundMask; // ���� groundMask ��� ������� ����� ���� �����.

    private float horizontalInput; // ���������� ��� �������� ����� �� ����������� (��������, ������� A/D ��� �������).
    public float speed; // ��������� ���� ��� ��������� �������� ����������� ���������.
    private float jumpforce = 200000; // ��������� ���� ��� ��������� ���� ������ ���������.
    private bool isOnGround; // ���������� ��� ������������, ��������� �� �������� �� �����.
    private float CheckRadius = 0.05f; // ������ �������� ����� ��� ������������� Physics2D.OverlapCircle.
    public float smoothness = 0.5f; // ��������� ���� ��� ��������� ��������� ��������.


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
        Move(); // �������� ����� ��� ��������� ��������.
        Jump(); // �������� ����� ��� ��������� ������.
        Dash();
        CheckGround(); // �������� ����� ��� ��������, ��������� �� �������� �� �����.
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime); // ���������� ��������� � ������ �������� � �������������� �������.
        rb.MovePosition(Vector2.Lerp(rb.position, rb.position + moveVelocity * Time.fixedDeltaTime, smoothness)); // ������� ����������� ���������.
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
    }

    //private bool faceRight = false;

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // �������� ���� �� �����������.
        moveVector.x = Input.GetAxis("Horizontal"); // ��������� ������ �������� �� �����������.
        moveVelocity = Vector2.right * speed * horizontalInput; // ������������ �������� ��������.

        anim.SetFloat("Move X", Mathf.Abs(moveVector.x)); // ������������� �������� �������� "Move X" �� ������ ����� �� �����������.

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
            //rb.velocity = (Vector3.up * jumpforce);
        }
    }

    private void CheckGround()
    {
        isOnGround = Physics2D.OverlapCircle(footPos.position, CheckRadius, groundMask); // ���������, ��������� �� �������� �� �����.
        //Debug.Log(isOnGround);
        anim.SetBool("onGround", isOnGround); // ������������� �������� �������� "onGround" � ����������� �� ��������� �� �����.
    }


    public int dashForce = 9999;

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) // �������� �������� !isDashing, ����� �� ��������� Dash, ���� ��� �����������
        {
            //rb.velocity = new Vector2(0, 0);
            anim.SetBool("isDashing", true);
            //Vector2 dashDirection = faceRight ? Vector2.right : Vector2.left;

            // ��������� ���� Impuls � Rigidbody2D
            Vector2 dashDirection;
            if (spriteRenderer.flipX == true)
            {
                dashDirection = Vector2.left;
            }
            else
            {
                dashDirection = Vector2.right;
            }

            rb.AddForce(dashDirection * dashForce);


            // ��������� ���������� � ������� ��������
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);

            // ��������� ������������ ������� ��������
            float duration = currentState.length;


            // ��������� Dash ����� ��������
            StartCoroutine(DisableDash(0.7f));

            // ����� ��������, ����� �������� �����������, ������������� isDashing � false
            anim.SetBool("isDashing", false);
        }
    }

    private IEnumerator DisableDash(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

}
