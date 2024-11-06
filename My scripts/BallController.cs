using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    private Rigidbody ball;

    private float speed = 18.0f;
    private float acceleration = 200.0f;
    private float deceleration = 200.0f;
    private float jumpForce = 40.0f; // Сила прыжка
    private float additionalGravityMultiplier = 0.7f; // Множитель для увеличения силы гравитации
    private bool isInWater = false;

    private Vector3 currentVelocity;

    public Transform cameraTransform;

    public float groundCheckRadius = 0.5f; // Радиус сферы для проверки земли
    public LayerMask groundLayer; // Слой, который будет считаться землёй

    public Transform helmPosition;    // Позиция у штурвала для фиксации шара
    private bool isFixed = false;

    void Start()
    {
        ball = GetComponent<Rigidbody>();
        ball.angularDrag = 0.7f;
        ball.drag = 2.0f; // Добавляем физическое сопротивление для плавного замедления
    }

    void Update()
    {
        // Получаем ввод от пользователя
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Создаем вектор направления относительно камеры
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = (right * horizontal + forward * vertical).normalized;

        // Управляем скоростью
        if (movement.magnitude > 0)
        {
            currentVelocity += movement * acceleration * Time.deltaTime;
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, speed);
        }
        else
        {
            // Добавляем замедление
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        // Проверяем нажатие пробела для прыжка
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isInWater)
            {
                JumpInWater(); // Совершить мощный прыжок, если в воде
            }
            else if (IsGrounded())
            {
                Jump(); // Обычный прыжок на земле
            }
        }

        if (isFixed)
        {
            FixBallToHelm();
        }
    }

    void FixedUpdate()
    {
        // Применяем силу гравитации дополнительно к физическому движению
        ball.AddForce(Physics.gravity * (ball.mass * additionalGravityMultiplier), ForceMode.Acceleration);
        // Применяем силу к Rigidbody только в FixedUpdate
        ball.AddForce(currentVelocity, ForceMode.Acceleration);

        if (!isInWater)
        {
            ball.velocity = Vector3.ClampMagnitude(ball.velocity, speed);
        }
    }

    private void Jump()
    {
        // Применяем силу вверх для обычного прыжка
        ball.AddForce(Vector3.up * jumpForce / 10000, ForceMode.Impulse);
    }

    private void JumpInWater()
    {
        // Применяем усиленную силу вверх для прыжка в воде (в 6 раза мощнее обычного)
        ball.AddForce(Vector3.up * jumpForce * 6 / 10000, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        // Проверяем, находится ли шар на земле с использованием SphereCast
        RaycastHit hit;
        return Physics.SphereCast(transform.position, groundCheckRadius, Vector3.down, out hit, 1.1f, groundLayer);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            ball.drag = 0.01f; // Устанавливаем сопротивление воды при входе
            ball.angularDrag = 0.05f;
            ball.useGravity = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            ball.drag = 2.0f; // Сбрасываем сопротивление при выходе из воды
            ball.angularDrag = 0.7f;
            ball.useGravity = true;
        }
    }

    public void FixBallToHelm()
    {
        transform.position = helmPosition.position;
        transform.rotation = helmPosition.rotation;
    }

    public void SetFixed(bool fixedStatus)
    {
        isFixed = fixedStatus;
    }
}
