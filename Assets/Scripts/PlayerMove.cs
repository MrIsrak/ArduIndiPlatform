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
    private float jumpforce = 350000; // Публичное поле для настройки силы прыжка персонажа.
    private bool isOnGround; // Переменная для отслеживания, находится ли персонаж на земле.
    private float CheckRadius = 0.05f; // Радиус проверки земли при использовании Physics2D.OverlapCircle.
    private bool isFlipping; // Переменная для предотвращения многократного отражения персонажа по горизонтали.
    private float flipCooldown = 0.1f; // Время задержки между отражениями (предотвращение быстрого изменения направления).
   

    public float smoothness = 0.5f; // Публичное поле для настройки плавности движения.
    private Vector2 targetPosition; // Переменная для хранения целевой позиции при выполнении плавного прыжка.
    private bool jumpControl;
    private int jumpIteration;
    public int jumpValueIteration = 60;

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
        CheckGround(); // Вызываем метод для проверки, находится ли персонаж на земле.
        Dash();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime); // Перемещаем персонажа с учетом скорости и фиксированного времени.
        rb.MovePosition(Vector2.Lerp(rb.position, rb.position + moveVelocity * Time.fixedDeltaTime, smoothness)); // Плавное перемещение персонажа.
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
    }


    private bool RightFlip;
    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // Получаем ввод по горизонтали.
        moveVector.x = Input.GetAxis("Horizontal"); // Сохраняем вектор движения по горизонтали.

        if (horizontalInput < 0 && !isFlipping)
        {
            spriteRenderer.flipX = true; // Отражаем спрайт, если двигаемся влево.
            isFlipping = true;
            RightFlip = false;
            StartCoroutine(ResetFlipCooldown()); // Запускаем корутину для предотвращения частого отражения.
        }
        else if (horizontalInput > 0 && !isFlipping)
        {
            spriteRenderer.flipX = false; // Не отражаем спрайт, если двигаемся вправо.
            isFlipping = true;
            RightFlip = true;
            StartCoroutine(ResetFlipCooldown()); // Запускаем корутину для предотвращения частого отражения.
        }

        moveVelocity = Vector2.right * speed * horizontalInput; // Рассчитываем скорость движения.

        anim.SetFloat("Move X", Mathf.Abs(moveVector.x)); // Устанавливаем параметр анимации "Move X" на основе ввода по горизонтали.
    }




    private IEnumerator ResetFlipCooldown()
    {
        yield return new WaitForSeconds(flipCooldown); // Ждем указанное время перед сбросом isFlipping.
        isFlipping = false; // Сбрасываем флаг отражения.
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(Vector2.up * jumpforce); // Рассчитываем целевую позицию для плавного прыжка.
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
        isOnGround = Physics2D.OverlapCircle(footPos.position, CheckRadius, groundMask); // Проверяем, находится ли персонаж на земле.
        anim.SetBool("onGround", isOnGround); // Устанавливаем параметр анимации "onGround" в зависимости от состояния на земле.
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