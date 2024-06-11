using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour
{
    public float jumpForce = 10f;  // ���� ������
    public Transform groundCheck;  // ����� �������� �����
    public float groundCheckRadius = 0.2f;  // ������ �������� �����
    public LayerMask whatIsGround;  // ���� �����

    private bool isGrounded;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ��������, ��������� �� �������� �� �����
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // ���� ����� �������� ������ � �������� �� �����, �� ����������� ������
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
