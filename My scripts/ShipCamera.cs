using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCamera : MonoBehaviour
{
    public float rotationSpeed = 40f; // �������� �������� ������
    private float currentYRotation = -90f; // ������� ���� �������� ������ �� ���������
    private Camera shipCamera;
    private Quaternion defaultRotation; // ����������� �������� ������

    void Start()
    {
        // �������� ��������� Camera
        shipCamera = GetComponent<Camera>();

        // ��������� ����������� �������� ������
        defaultRotation = transform.localRotation;
    }

    void Update()
    {
        // ���������, ������� �� ������
        if (!shipCamera.enabled)
        {
            // ���������� ������ � ������������ ����������� ��� � ����������
            transform.localRotation = defaultRotation;
            currentYRotation = -90f; // ����� �������� ���� � ������������ ��������
            return;
        }

        HandleCameraRotation();
    }

    void HandleCameraRotation()
    {
        // �������� �������� ���� �� ��� X
        float mouseX = Input.GetAxis("Mouse X");

        // ������������ ����� �������� ���� �������� ������ ��������� ��� Y
        currentYRotation += mouseX * rotationSpeed * Time.deltaTime;

        // ������������ ������� ������
        currentYRotation = Mathf.Clamp(currentYRotation, -120f, -60f);

        // ��������� ������� � ������
        transform.localRotation = Quaternion.Euler(0, currentYRotation, 0);
    }


}
