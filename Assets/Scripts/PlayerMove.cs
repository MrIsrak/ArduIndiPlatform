using System.Collections; // Импортируем пространство имен для работы с коллекциями (например, списками и массивами).
using System.Collections.Generic; // Импортируем пространство имен для работы с обобщенными коллекциями.
using System.ComponentModel; // Импортируем пространство имен для работы с компонентами и свойствами.
using System.Security.Cryptography;
using System.Text;
using System.Threading; // Импортируем пространство имен для работы с потоками.
using UnityEngine; // Импортируем пространство имен Unity для доступа к функциональности движка.

public class PlayerMove : MonoBehaviour // Объявляем класс PlayerMove, который наследуется от MonoBehaviour (базового класса для скриптов в Unity).
{
    [SerializeField] private Transform footPos; // Поле footPos для ссылки на трансформ объекта, представляющего позицию ног персонажа.
    [SerializeField] private LayerMask groundMask; // Поле groundMask для задания маски слоя земли.

    private float horizontalInput; // Переменная для хранения ввода по горизонтали (например, клавиши A/D или стрелки).
    private float speed = 6f; // Публичное поле для настройки скорости перемещения персонажа.
    public float jumpforce = 15000f; // Публичное поле для настройки силы прыжка персонажа.

    bool isDashing = false;
    bool canDash = true;
    float dashRechargeTime = 0.5f;
    float dashDuration=0.7f;

    private bool isOnGround; // Переменная для отслеживания, находится ли персонаж на земле.
    private float CheckRadius = 0.05f; // Радиус проверки земли при использовании Physics2D.OverlapCircle.


    private SpriteRenderer spriteRenderer; // Переменная для доступа к компоненту SpriteRenderer объекта (для изменения отображения спрайта).
    private Animator anim; // Переменная для доступа к компоненту Animator объекта (для управления анимациями).
    private Vector2 moveVelocity; // Вектор для хранения скорости движения по горизонтали.
    private Vector2 moveVector; // Вектор для хранения ввода движения по горизонтали.
    private Rigidbody2D rb; // Переменная для доступа к компоненту Rigidbody2D объекта (для физики).

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Получаем компонент Rigidbody2D объекта.
        spriteRenderer = GetComponent<SpriteRenderer>(); // Получаем компонент SpriteRenderer объекта.
        anim = GetComponent<Animator>(); // Получаем компонент Animator объекта.
    }

    private void Update()
    {
        if (isDashing)
            return;
        rb.velocity = new Vector2(rb.velocity.x, Physics2D.gravity.y);
        Move(); // Вызываем метод для обработки движения.
        Jump(); // Вызываем метод для обработки прыжка.
        CheckDash();
        CheckGround(); // Вызываем метод для проверки, находится ли персонаж на земле.
        
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
        horizontalInput = Input.GetAxis("Horizontal"); // Получаем ввод по горизонтали.
        anim.SetFloat("Move X", Mathf.Abs(horizontalInput)); // Устанавливаем параметр анимации "Move X" на основе ввода по горизонтали.

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
            rb.AddForce(Vector2.up * jumpforce); // Рассчитываем целевую позицию для плавного прыжка.
            //rb.velocity += new Vector2(0, 10000);
            //rb.velocity = (Vector3.up * jumpforce);
        }
    }

    private void CheckGround()
    {
        isOnGround = Physics2D.OverlapCircle(footPos.position, CheckRadius, groundMask); // Проверяем, находится ли персонаж на земле.
        anim.SetBool("onGround", isOnGround); // Устанавливаем параметр анимации "onGround" в зависимости от состояния на земле.
    }


    [SerializeField] int dashForce;

    private void CheckDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) // Добавьте проверку !isDashing, чтобы не запускать Dash, если уже выполняется
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