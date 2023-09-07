using UnityEngine; // ����������� ������������ ���� Unity ��� ������� � ���������������� ������.

public class PlayerMove : MonoBehaviour // ��������� ����� PlayerMove, ������� ����������� �� MonoBehaviour (�������� ������ ��� �������� � Unity).
{
    [SerializeField] private Transform footPos; // ���� footPos ��� ������ �� ��������� �������, ��������������� ������� ��� ���������.
    [SerializeField] private LayerMask groundMask; // ���� groundMask ��� ������� ����� ���� �����.

    private float horizontalInput; // ���������� ��� �������� ����� �� ����������� (��������, ������� A/D ��� �������).
    public float speed; // ��������� ���� ��� ��������� �������� ����������� ���������.
    private float jumpforce = 150000; // ��������� ���� ��� ��������� ���� ������ ���������.
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
        CheckGround(); // �������� ����� ��� ��������, ��������� �� �������� �� �����.
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime); // ���������� ��������� � ������ �������� � �������������� �������.
        rb.MovePosition(Vector2.Lerp(rb.position, rb.position + moveVelocity * Time.fixedDeltaTime, smoothness)); // ������� ����������� ���������.
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // �������� ���� �� �����������.
        moveVector.x = Input.GetAxis("Horizontal"); // ��������� ������ �������� �� �����������.
        moveVelocity = Vector2.right * speed * horizontalInput; // ������������ �������� ��������.

        anim.SetFloat("Move X", Mathf.Abs(moveVector.x)); // ������������� �������� �������� "Move X" �� ������ ����� �� �����������.

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
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(Vector2.up * jumpforce); // ������������ ������� ������� ��� �������� ������.
            //rb.velocity = (Vector3.up * jumpforce);
        }
    }

    private void CheckGround()
    {
        isOnGround = Physics2D.OverlapCircle(footPos.position, CheckRadius, groundMask); // ���������, ��������� �� �������� �� �����.
        anim.SetBool("onGround", !isOnGround); // ������������� �������� �������� "onGround" � ����������� �� ��������� �� �����.
    }
}
