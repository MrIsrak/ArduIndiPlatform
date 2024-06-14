using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("UI")]
    public GameObject dasher; // Объект для отображения анимации рывка

    [Header("Settings")]
    [SerializeField] private Transform footPos; // Позиция для проверки земли
    [SerializeField] private LayerMask groundMask; // Слой земли
    [SerializeField] private int dashForce; // Сила рывка

    [Header("Movement Settings")]
    private float speed = 3f; // Скорость движения
    [SerializeField] private float jumpForce = 7f; // Сила прыжка
    private float checkRadius = 0.05f; // Радиус проверки земли
    private float dashRechargeTime = 0.5f; // Время восстановления рывка
    private float dashDuration = 0.7f; // Длительность рывка

    private float horizontalInput; // Ввод по горизонтали
    public static bool isDashing = false; // Флаг рывка
    private bool canDash = true; // Флаг возможности рывка
    private bool isOnGround; // Флаг на земле
    private bool hit = false; // Флаг атаки

    private SpriteRenderer spriteRenderer; // Спрайт рендерер
    private Animator anim; // Аниматор
    private Rigidbody2D rb; // Rigidbody2D

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Проверка на рывок
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing && !hit)
        {
            StartCoroutine(Dash()); // Начинаем рывок
            return;
        }

        run(); // Обработка движения
        CheckGroundStatus(); // Проверка, на земле ли персонаж
        Attack(); // Обработка атаки
        Jump();  // Прыжок

        // Обновление скорости, если не идет рывок и не атакуем
        if (!isDashing && !hit)
        {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }
    }

    // Обработка движения персонажа
    private void run()
    {
        if (hit)
        {
            horizontalInput = 0;
            anim.SetBool("isRun", false); // Остановка анимации бега
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");

        // Установка анимации бега
        anim.SetBool("isRun", horizontalInput != 0 && !isDashing);

        // Изменение направления спрайта
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    // Проверка, на земле ли персонаж
    private void CheckGroundStatus()
    {
        isOnGround = Physics2D.OverlapCircle(footPos.position, checkRadius, groundMask);

        if (!isOnGround)
        {
            anim.Play("JumptoFall"); // Анимация перехода в падение
        }
    }

    // Обработка рывка
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        anim.Play("Dash"); // Анимация рывка

        Animator dasherAnimator = dasher.GetComponent<Animator>();
        dasherAnimator.speed = dashRechargeTime + 0.2f;
        dasherAnimator.Play("dasher");

        int lookDirection = spriteRenderer.flipX ? -1 : 1;
        rb.velocity = new Vector2(lookDirection * dashForce, rb.velocity.y);

        float dashTime = 0f;

        // Длительность рывка
        while (dashTime < dashDuration)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                anim.Play("Dash-Attack"); // Анимация атаки во время рывка
                break;
            }
            dashTime += Time.deltaTime;
            yield return null;
        }

        // Завершение рывка
        yield return new WaitForSeconds(dashDuration - dashTime);

        isDashing = false;
        yield return new WaitForSeconds(dashRechargeTime);
        canDash = true; // Восстановление возможности рывка
    }

    // Прыжок
    private void Jump()
    {
        if (isOnGround && Input.GetKeyDown(KeyCode.Space) && !hit)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Применение силы прыжка
        }
    }

    // Обработка атаки
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !hit)
        {
            hit = true;
            anim.Play("Attack"); // Анимация атаки
            StartCoroutine(ResetHitAfterAttack());
        }
    }

    // Сброс флага атаки после завершения анимации
    private IEnumerator ResetHitAfterAttack()
    {
        yield return new WaitForSeconds(1); // Задержка перед сбросом
        hit = false; // Сброс флага атаки
    }
}
