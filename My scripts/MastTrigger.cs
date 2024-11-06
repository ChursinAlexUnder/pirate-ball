using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MastTrigger : MonoBehaviour
{
    public Camera crowNestCamera; // ������ � �������� ������
    public Camera ballCamera;     // ������ ����
    public GameObject ball;     // �����
    public Transform mastPosition;    // ������� � �������� ��� �������� ����
    private bool isInCrowNest = false; // ����, ��������� �� ����� � �������� ������

    void Update()
    {
        // �������� ������� ������� "e" ��� ������������ ������
        if (isInCrowNest && Input.GetKeyDown(KeyCode.E))
        {
            SwitchCamera();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // ��������, ���� ����� ����� � ���������� ������� � �������� ������
        if (other.gameObject == ball)
        {
            isInCrowNest = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // ����� ����� ������� �� ���������� �������
        if (other.gameObject == ball)
        {
            isInCrowNest = false;
        }
    }

    void SwitchCamera()
    {
        // ������������ ����� ������� ���� � �������� �������
        if (crowNestCamera.enabled)
        {
            crowNestCamera.enabled = false;
            ballCamera.enabled = true;
            ball.GetComponent<BallController>().enabled = true;
            ball.GetComponent<BallController>().SetFixed(false); // ��������� �������� ����
            ball.GetComponent<Rigidbody>().isKinematic = false;
            // �������� ���������� �������, ���� �����
        }
        else
        {
            ball.GetComponent<Rigidbody>().isKinematic = true;
            crowNestCamera.enabled = true;
            ballCamera.enabled = false;
            ball.GetComponent<BallController>().enabled = false;
            ball.GetComponent<BallController>().SetFixed(true); // �������� �������� ����
            ball.transform.position = mastPosition.position; // �������� ��� �� ������� � ����
            // ��������� ���������� �������, ���� �����
        }
    }
}
