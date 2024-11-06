using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSwitcher : MonoBehaviour
{
    public GameObject ball;           // Шар
    public GameObject ship;           // Корабль
    public Camera ballCamera;         // Камера шара
    public Camera shipCamera;         // Камера корабля
    public Camera mastCamera;         // Камера в вороньем гнезде
    public Transform helmPosition;    // Позиция у штурвала для фиксации шара
    private bool isControllingShip = false;

    void Start()
    {
        // Убедитесь, что в начале активна только камера шара
        ballCamera.enabled = true;
        shipCamera.enabled = false;
        mastCamera.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (IsBallNearHelm() || isControllingShip))
        {
            SwitchControl();
        }
    }

    bool IsBallNearHelm()
    {
        float distance = Vector3.Distance(ball.transform.position, helmPosition.position);
        return distance < 2f; // Условие приближенности к штурвалу
    }

    void SwitchControl()
    {
        if (isControllingShip)
        {
            // Возвращаем управление шару
            ball.GetComponent<BallController>().enabled = true;
            ball.GetComponent<BallController>().SetFixed(false); // Отключаем фиксацию шара
            ship.GetComponent<ShipController>().SetControl(false); // Отключаем управление кораблём
            ballCamera.enabled = true;
            shipCamera.enabled = false;
            ball.transform.parent = null; // Отвязать шар от корабля
            isControllingShip = false;  
        }
        else
        {
            // Передаём управление кораблю
            ball.GetComponent<BallController>().enabled = false;
            ball.GetComponent<BallController>().SetFixed(true); // Включаем фиксацию шара
            ship.GetComponent<ShipController>().SetControl(true); // Включаем управление кораблём
            ball.transform.position = helmPosition.position; // Помещаем шар на позицию у руля
            ball.transform.parent = ship.transform; // Привязываем шар к кораблю
            ballCamera.enabled = false;
            shipCamera.enabled = true;
            isControllingShip = true;
        }
    }
}
