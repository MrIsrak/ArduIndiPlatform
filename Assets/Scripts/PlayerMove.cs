using System.Collections; // Импортируем пространство имен для работы с коллекциями (например, списками и массивами).
using System.Collections.Generic; // Импортируем пространство имен для работы с обобщенными коллекциями.
using System.ComponentModel; // Импортируем пространство имен для работы с компонентами и свойствами.
using System.Threading; // Импортируем пространство имен для работы с потоками.
using UnityEngine; // Импортируем пространство имен Unity для доступа к функциональности движка.

public class PlayerMove : MonoBehaviour // Объявляем класс PlayerMove, который наследуется от MonoBehaviour (базового класса для скриптов в Unity).
{
    [SerializeField] private Transform footPos; // Поле footPos для ссылки на трансформ объекта, представляющего позицию ног персонажа.
    [SerializeField] private LayerMask groundMask; // Поле groundMask для задания маски слоя земли.

    private float horizontalInput; // Переменная для хранения ввода по горизонтали (например, клавиши A/D или стрелки).
    public float speed; // Публичное поле для настройки скорости перемещения персонажа.
    private float jumpforce = 150000; // Публичное поле для настройки силы прыжка персонажа.
    private bool isOnGround; // Переменная для отслеживания, находится ли персонаж на земле.
    private float CheckRadius = 0.05f; // Радиус проверки земли при использовании Physics2D.OverlapCircle.
    public float smoothness = 0.5f; // Публичное поле для настройки плавности движения.


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
        Move(); // Вызываем метод для обработки движения.
        Jump(); // Вызываем метод для обработки прыжка.
        Dash();
        CheckGround(); // Вызываем метод для проверки, находится ли персонаж на земле.
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime); // Перемещаем персонажа с учетом скорости и фиксированного времени.
        rb.MovePosition(Vector2.Lerp(rb.position, rb.position + moveVelocity * Time.fixedDeltaTime, smoothness)); // Плавное перемещение персонажа.
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
    }

    private bool faceRight = false;

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // Получаем ввод по горизонтали.
        moveVector.x = Input.GetAxis("Horizontal"); // Сохраняем вектор движения по горизонтали.
        moveVelocity = Vector2.right * speed * horizontalInput; // Рассчитываем скорость движения.

        anim.SetFloat("Move X", Mathf.Abs(moveVector.x)); // Устанавливаем параметр анимации "Move X" на основе ввода по горизонтали.

        if (horizontalInput < 0)
        {
            faceRight = false;
            spriteRenderer.flipX = true; // Flip the sprite horizontally if moving left
        }
        else if (horizontalInput > 0)
        {
            faceRight = true;
            spriteRenderer.flipX = false; // Unflip the sprite horizontally if
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(Vector2.up * jumpforce); // Рассчитываем целевую позицию для плавного прыжка.
            //rb.velocity = (Vector3.up * jumpforce);
        }
    }

    private void CheckGround()
    {
        isOnGround = Physics2D.OverlapCircle(footPos.position, CheckRadius, groundMask); // Проверяем, находится ли персонаж на земле.
        anim.SetBool("onGround", !isOnGround); // Устанавливаем параметр анимации "onGround" в зависимости от состояния на земле.
    }

    public int Impuls = 500000;
    private bool isDashing = true;

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isDashing = true;
            anim.SetBool("isDashing", isDashing);
            
            //anim.StopPlayback();
            //anim.Play("Dash");
            //Debug.Log("Dash");
            
            //rb.velocity = new Vector2(0, 0);

            Vector2 dashDirection = faceRight ? Vector2.right : Vector2.left;

            rb.AddForce(dashDirection * Impuls);
            Debug.Log(message: "Imp");
            isDashing = false;
            anim.SetBool("isDashing", isDashing);
            StartCoroutine(DisableDash(1f));

            
        }
    }

    private IEnumerator DisableDash(float delay)
    {
        yield return new WaitForSeconds(delay);
        //isDashing = true;
    }
}
