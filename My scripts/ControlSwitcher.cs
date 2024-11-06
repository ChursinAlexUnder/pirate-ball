using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSwitcher : MonoBehaviour
{
    public GameObject ball;           // ���
    public GameObject ship;           // �������
    public Camera ballCamera;         // ������ ����
    public Camera shipCamera;         // ������ �������
    public Camera mastCamera;         // ������ � �������� ������
    public Transform helmPosition;    // ������� � �������� ��� �������� ����
    private bool isControllingShip = false;

    void Start()
    {
        // ���������, ��� � ������ ������� ������ ������ ����
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
        return distance < 2f; // ������� �������������� � ��������
    }

    void SwitchControl()
    {
        if (isControllingShip)
        {
            // ���������� ���������� ����
            ball.GetComponent<BallController>().enabled = true;
            ball.GetComponent<BallController>().SetFixed(false); // ��������� �������� ����
            ship.GetComponent<ShipController>().SetControl(false); // ��������� ���������� �������
            ballCamera.enabled = true;
            shipCamera.enabled = false;
            ball.transform.parent = null; // �������� ��� �� �������
            isControllingShip = false;  
        }
        else
        {
            // ������� ���������� �������
            ball.GetComponent<BallController>().enabled = false;
            ball.GetComponent<BallController>().SetFixed(true); // �������� �������� ����
            ship.GetComponent<ShipController>().SetControl(true); // �������� ���������� �������
            ball.transform.position = helmPosition.position; // �������� ��� �� ������� � ����
            ball.transform.parent = ship.transform; // ����������� ��� � �������
            ballCamera.enabled = false;
            shipCamera.enabled = true;
            isControllingShip = true;
        }
    }
}
