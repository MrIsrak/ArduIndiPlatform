using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour
{
    public float jumpForce = 10f;  // Сила прыжка
    public Transform groundCheck;  // Точка проверки земли
    public float groundCheckRadius = 0.2f;  // Радиус проверки земли
    public LayerMask whatIsGround;  // Слой земли

    private bool isGrounded;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Проверка, находится ли персонаж на земле
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // Если игрок нажимает пробел и персонаж на земле, то выполняется прыжок
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
