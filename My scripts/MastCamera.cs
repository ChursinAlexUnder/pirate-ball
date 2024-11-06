using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MastCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // Чувствительность мыши
    private float xRotation = 0f;          // Текущая ротация по оси X
    private float yRotation = -90f;          // Текущая ротация по оси Y
    private Camera mastCamera;
    private Quaternion defaultRotation;    // Изначальное вращение камеры

    void Start()
    {
        mastCamera = GetComponent<Camera>();  // Получаем компонент Camera

        // Сохраняем изначальное вращение камеры
        defaultRotation = transform.localRotation;
    }

    void Update()
    {
        // Проверяем, активна ли камера
        if (!mastCamera.enabled)
        {
            // Возвращаем камеру к изначальному направлению при её отключении
            transform.localRotation = defaultRotation;
            xRotation = 0f; // Сброс вращения по X
            yRotation = -90f; // Сброс вращения по Y
            return;
        }

        HandleCameraRotation();
    }

    void HandleCameraRotation()
    {
        // Получаем смещение мыши по осям
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Рассчитываем вращение по оси X (вверх/вниз) с ограничением
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        // Рассчитываем вращение по оси Y (влево/вправо)
        yRotation += mouseX;

        // Поворачиваем камеру по осям
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

}
