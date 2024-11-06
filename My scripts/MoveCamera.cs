using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform ball; // Ссылка на шар
    public Vector3 offset; // Смещение камеры
    public float mouseSensitivity = 2.0f; // Чувствительность мыши
    public Camera ballCamera; // Ссылка на саму камеру

    private float rotationY = 0.0f; // Вертикальная ротация
    private float rotationX = 0.0f; // Горизонтальная ротация
    private Quaternion initialRotation; // Изначальная ротация камеры

    void Start()
    {
        // Изначальная ротация камеры
        initialRotation = transform.rotation;

        // Устанавливаем начальное смещение
        offset = transform.position - ball.position;

        // Скрываем курсор и фиксируем его в центре экрана
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Проверяем, активна ли камера
        if (!ballCamera.enabled) return;

        // Получаем ввод мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Обновляем вертикальную и горизонтальную ротацию
        rotationY += mouseY;
        rotationX += mouseX;

        // Ограничиваем вертикальный угол
        rotationY = Mathf.Clamp(rotationY, -30f, 19f);

        // Поворачиваем камеру вокруг шара
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
        transform.position = ball.position + rotation * offset;

        // Смотрим на шар
        transform.rotation = initialRotation * rotation; // Применяем начальную ротацию
        transform.LookAt(ball.position);
    }

}
