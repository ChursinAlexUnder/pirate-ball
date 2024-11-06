using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public Transform shipWheel;       // Объект штурвала корабля
    public GameObject ball;           // Шар
    public Transform helmPosition;    // Позиция у штурвала для фиксации шара
    public float shipSpeed = 20f;      // Максимальная скорость движения корабля
    public float turnSpeed = 5f;      // Максимальная скорость поворота корабля
    public float acceleration = 2f;   // Ускорение при движении вперёд/назад
    public float turnAcceleration = 2f; // Ускорение поворота
    public float deceleration = 1.5f; // Замедление движения и поворота, когда нет ввода
    private bool isControllingShip = false;
    private float wheelRotation = 0f; // Текущий угол поворота штурвала
    private float maxWheelRotation = 2.0f * 360f; // Максимальный угол поворота (2 оборота)
    private float returnSpeed = 260f; // Скорость возврата штурвала в исходное положение
    private float currentSpeed = 0f;  // Текущая скорость движения корабля
    private float currentTurnSpeed = 0f; // Текущая скорость поворота корабля
    public float exitDecelerationMultiplier = 60f; // Коэффициент ускоренного торможения при выходе из управления
    public float tiltAmount = 8f; // Угол наклона при полном повороте
    public float tiltSmooth = 0.2f; // Скорость интерполяции для наклона

    private float currentTilt = 0f; // Текущий угол наклона


    void Update()
    {
        // Логика контроля корабля только если игрок управляет им
        if (isControllingShip)
        { 
            HandleShipControls();
        }
        else
        {
            ApplyShipInertia();
        }
        
    }

    // Метод для включения/выключения управления кораблём
    public void SetControl(bool isActive)
    {
        isControllingShip = isActive;

        if (!isActive && ball != null)
        {
            ball.GetComponent<Rigidbody>().isKinematic = false;
            ball.transform.position = helmPosition.position; // Возвращаем шар на позицию у руля
            ball.transform.parent = null; // Убираем родительскую связь
        }
        else if (isActive)
        {
            ball.transform.position = helmPosition.position; // Помещаем шар на позицию у руля
            ball.transform.parent = transform; // Привязываем шар к кораблю
            ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void HandleShipControls()
    {
        // Получаем ввод для поворота и движения корабля
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D
        float verticalInput = Input.GetAxis("Vertical"); // W/S

        // Двигаем корабль вперед/назад
        MoveShip(verticalInput);

        // Поворачиваем корабль влево/вправо
        TurnShip(horizontalInput);

        // Вращаем штурвал в зависимости от ввода
        RotateWheel(horizontalInput);

        SlantShip(horizontalInput);

        if (Input.GetKey(KeyCode.Space))
        {
            EmergencyBraking(6f);
        }
    }

    void EmergencyBraking(float SpeedBreak)
    {
        // Применение инерции движения
        if (currentSpeed != 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, SpeedBreak * Time.deltaTime);
        }
        // Применение инерции поворота
        if (currentTurnSpeed != 0)
        {
            currentTurnSpeed = Mathf.MoveTowards(currentTurnSpeed, 0, SpeedBreak * Time.deltaTime);
        }
    }

    void ApplyShipInertia()
    {
        // Применение инерции движения
        if (currentSpeed != 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, exitDecelerationMultiplier * Time.deltaTime);
            Vector3 rightDirection = transform.right;
            rightDirection.y = 0;
            rightDirection.Normalize();
            Vector3 movement = -rightDirection * currentSpeed * Time.deltaTime;
            transform.position += movement;
        }

        // Применение инерции поворота
        if (currentTurnSpeed != 0)
        {
            currentTurnSpeed = Mathf.MoveTowards(currentTurnSpeed, 0, exitDecelerationMultiplier * Time.deltaTime);
            transform.Rotate(Vector3.up, currentTurnSpeed * Time.deltaTime);
        }
        // Возвращение штурвала к нейтральному положению
        wheelRotation = Mathf.MoveTowards(wheelRotation, 0f, returnSpeed * Time.deltaTime);
        shipWheel.localRotation = Quaternion.Euler(wheelRotation, 0, 0);

        if (currentTilt != 0)
        {
            currentTilt = Mathf.Lerp(currentTilt, 0, tiltSmooth * 3 * Time.deltaTime);
            // Применяем наклон к кораблю
            transform.rotation = Quaternion.Euler(currentTilt, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }



    void MoveShip(float input)
    {
        // Добавляем инерцию для движения
        if (input > 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, shipSpeed * input, acceleration * Time.deltaTime);
        }
        else
        {
            // Если нет ввода, замедляем корабль до остановки
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        // Двигаем корабль вперед или назад с учётом инерции
        Vector3 rightDirection = transform.right;
        rightDirection.y = 0;
        rightDirection.Normalize();

        Vector3 movement = -rightDirection * currentSpeed * Time.deltaTime;
        transform.position += movement;
    }

    void TurnShip(float input)
    {
        // Добавляем инерцию для поворота
        if (input != 0)
        {
            currentTurnSpeed = Mathf.MoveTowards(currentTurnSpeed, turnSpeed * input, turnAcceleration * Time.deltaTime);
        }
        else
        {
            // Если нет ввода для поворота, замедляем вращение корабля
            currentTurnSpeed = Mathf.MoveTowards(currentTurnSpeed, 0, deceleration * Time.deltaTime);
        }

        // Поворачиваем корабль с учётом инерции
        transform.Rotate(Vector3.up, currentTurnSpeed * Time.deltaTime);
    }

    void SlantShip(float input)
    {
        // Поворачиваем корабль вокруг оси X
        transform.Rotate(Vector3.right, input * currentTurnSpeed * Time.deltaTime);

        // Вычисляем целевой угол наклона
        float targetTilt = input * tiltAmount;
        // Плавно изменяем текущий угол наклона
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, tiltSmooth * Time.deltaTime);
        // Применяем наклон к кораблю
        transform.rotation = Quaternion.Euler(currentTilt, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    void RotateWheel(float input)
    {
        if (input != 0)
        {
            // Вычисляем новый угол поворота штурвала
            wheelRotation += input * Time.deltaTime * 220f;
            wheelRotation = Mathf.Clamp(wheelRotation, -maxWheelRotation, maxWheelRotation);
        }
        else
        {
            // Если нет ввода для поворота, постепенно возвращаем штурвал в исходное положение
            wheelRotation = Mathf.MoveTowards(wheelRotation, 0f, returnSpeed * Time.deltaTime);
        }

        // Вращаем штурвал вокруг оси X
        shipWheel.localRotation = Quaternion.Euler(wheelRotation, 0, 0);
    }
}
