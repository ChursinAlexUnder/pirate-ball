using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MastTrigger : MonoBehaviour
{
    public Camera crowNestCamera; // Камера в вороньем гнезде
    public Camera ballCamera;     // Камера шара
    public GameObject ball;     // шарик
    public Transform mastPosition;    // Позиция у лестницы для фиксации шара
    private bool isInCrowNest = false; // Флаг, находится ли игрок в вороньем гнезде

    void Update()
    {
        // Проверка нажатия клавиши "e" для переключения камеры
        if (isInCrowNest && Input.GetKeyDown(KeyCode.E))
        {
            SwitchCamera();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Проверка, если игрок вошел в триггерную область в вороньем гнезде
        if (other.gameObject == ball)
        {
            isInCrowNest = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Когда игрок выходит из триггерной области
        if (other.gameObject == ball)
        {
            isInCrowNest = false;
        }
    }

    void SwitchCamera()
    {
        // Переключение между камерой шара и вороньим гнездом
        if (crowNestCamera.enabled)
        {
            crowNestCamera.enabled = false;
            ballCamera.enabled = true;
            ball.GetComponent<BallController>().enabled = true;
            ball.GetComponent<BallController>().SetFixed(false); // Отключаем фиксацию шара
            ball.GetComponent<Rigidbody>().isKinematic = false;
            // Включаем управление шариком, если нужно
        }
        else
        {
            ball.GetComponent<Rigidbody>().isKinematic = true;
            crowNestCamera.enabled = true;
            ballCamera.enabled = false;
            ball.GetComponent<BallController>().enabled = false;
            ball.GetComponent<BallController>().SetFixed(true); // Включаем фиксацию шара
            ball.transform.position = mastPosition.position; // Помещаем шар на позицию у руля
            // Отключаем управление шариком, если нужно
        }
    }
}
