using System.Collections; // ����������� ������������ ���� ��� ������ � ����������� (��������, �������� � ���������).
using System.Collections.Generic; // ����������� ������������ ���� ��� ������ � ����������� �����������.
using System.ComponentModel; // ����������� ������������ ���� ��� ������ � ������������ � ����������.
using System.Threading; // ����������� ������������ ���� ��� ������ � ��������.
using UnityEngine; // ����������� ������������ ���� Unity ��� ������� � ���������������� ������.

public class PlayerMove : MonoBehaviour // ��������� ����� PlayerMove, ������� ����������� �� MonoBehaviour (�������� ������ ��� �������� � Unity).
{
    [SerializeField] private Transform footPos; // ���� footPos ��� ������ �� ��������� �������, ��������������� ������� ��� ���������.
    [SerializeField] private LayerMask groundMask; // ���� groundMask ��� ������� ����� ���� �����.

    private float horizontalInput; // ���������� ��� �������� ����� �� ����������� (��������, ������� A/D ��� �������).
    public float speed; // ��������� ���� ��� ��������� �������� ����������� ���������.
    private float jumpforce = 350000; // ��������� ���� ��� ��������� ���� ������ ���������.
    private bool isOnGround; // ���������� ��� ������������, ��������� �� �������� �� �����.
    private float CheckRadius = 0.05f; // ������ �������� ����� ��� ������������� Physics2D.OverlapCircle.
    private bool isFlipping; // ���������� ��� �������������� ������������� ��������� ��������� �� �����������.
    private float flipCooldown = 0.1f; // ����� �������� ����� ����������� (�������������� �������� ��������� �����������).
   

    public float smoothness = 0.5f; // ��������� ���� ��� ��������� ��������� ��������.
    private Vector2 targetPosition; // ���������� ��� �������� ������� ������� ��� ���������� �������� ������.
    private bool jumpControl;
    private int jumpIteration;
    public int jumpValueIteration = 60;

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
        CheckGround(); // �������� ����� ��� ��������, ��������� �� �������� �� �����.
        Dash();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime); // ���������� ��������� � ������ �������� � �������������� �������.
        rb.MovePosition(Vector2.Lerp(rb.position, rb.position + moveVelocity * Time.fixedDeltaTime, smoothness)); // ������� ����������� ���������.
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
    }


    private bool RightFlip;
    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // �������� ���� �� �����������.
        moveVector.x = Input.GetAxis("Horizontal"); // ��������� ������ �������� �� �����������.

        if (horizontalInput < 0 && !isFlipping)
        {
            spriteRenderer.flipX = true; // �������� ������, ���� ��������� �����.
            isFlipping = true;
            RightFlip = false;
            StartCoroutine(ResetFlipCooldown()); // ��������� �������� ��� �������������� ������� ���������.
        }
        else if (horizontalInput > 0 && !isFlipping)
        {
            spriteRenderer.flipX = false; // �� �������� ������, ���� ��������� ������.
            isFlipping = true;
            RightFlip = true;
            StartCoroutine(ResetFlipCooldown()); // ��������� �������� ��� �������������� ������� ���������.
        }

        moveVelocity = Vector2.right * speed * horizontalInput; // ������������ �������� ��������.

        anim.SetFloat("Move X", Mathf.Abs(moveVector.x)); // ������������� �������� �������� "Move X" �� ������ ����� �� �����������.
    }




    private IEnumerator ResetFlipCooldown()
    {
        yield return new WaitForSeconds(flipCooldown); // ���� ��������� ����� ����� ������� isFlipping.
        isFlipping = false; // ���������� ���� ���������.
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(Vector2.up * jumpforce); // ������������ ������� ������� ��� �������� ������.
        }
        //if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        //{
        //    jumpControl = true; 
        //}
        //else { jumpControl = false; }
        //if (jumpControl && jumpIteration++ < jumpValueIteration)
        //{
        //    rb.AddForce(Vector2.up * jumpforce / jumpIteration);
        //}
        //else { jumpIteration = 0; }
    }

    private void CheckGround()
    {
        isOnGround = Physics2D.OverlapCircle(footPos.position, CheckRadius, groundMask); // ���������, ��������� �� �������� �� �����.
        anim.SetBool("onGround", isOnGround); // ������������� �������� �������� "onGround" � ����������� �� ��������� �� �����.
    }


    private int lungueImpulse = 5000;
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.StopPlayback();
            anim.Play("Dash");

            rb.velocity = new Vector2(0, 0);

            if (!RightFlip) { rb.AddForce(Vector2.left * lungueImpulse); }
            else { rb.AddForce(Vector2.right * lungueImpulse); }
        }
    }
}