using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCamera : MonoBehaviour
{
    public float rotationSpeed = 40f; // Скорость поворота камеры
    private float currentYRotation = -90f; // Текущий угол поворота камеры по умолчанию
    private Camera shipCamera;
    private Quaternion defaultRotation; // Изначальное вращение камеры

    void Start()
    {
        // Получаем компонент Camera
        shipCamera = GetComponent<Camera>();

        // Сохраняем изначальное вращение камеры
        defaultRotation = transform.localRotation;
    }

    void Update()
    {
        // Проверяем, активна ли камера
        if (!shipCamera.enabled)
        {
            // Возвращаем камеру к изначальному направлению при её отключении
            transform.localRotation = defaultRotation;
            currentYRotation = -90f; // Сброс текущего угла к изначальному значению
            return;
        }

        HandleCameraRotation();
    }

    void HandleCameraRotation()
    {
        // Получаем смещение мыши по оси X
        float mouseX = Input.GetAxis("Mouse X");

        // Рассчитываем новое значение угла поворота вокруг локальной оси Y
        currentYRotation += mouseX * rotationSpeed * Time.deltaTime;

        // Ограничиваем поворот камеры
        currentYRotation = Mathf.Clamp(currentYRotation, -120f, -60f);

        // Применяем поворот к камере
        transform.localRotation = Quaternion.Euler(0, currentYRotation, 0);
    }


}
