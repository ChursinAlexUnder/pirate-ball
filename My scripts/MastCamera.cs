using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MastCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // ���������������� ����
    private float xRotation = 0f;          // ������� ������� �� ��� X
    private float yRotation = -90f;          // ������� ������� �� ��� Y
    private Camera mastCamera;
    private Quaternion defaultRotation;    // ����������� �������� ������

    void Start()
    {
        mastCamera = GetComponent<Camera>();  // �������� ��������� Camera

        // ��������� ����������� �������� ������
        defaultRotation = transform.localRotation;
    }

    void Update()
    {
        // ���������, ������� �� ������
        if (!mastCamera.enabled)
        {
            // ���������� ������ � ������������ ����������� ��� � ����������
            transform.localRotation = defaultRotation;
            xRotation = 0f; // ����� �������� �� X
            yRotation = -90f; // ����� �������� �� Y
            return;
        }

        HandleCameraRotation();
    }

    void HandleCameraRotation()
    {
        // �������� �������� ���� �� ����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ������������ �������� �� ��� X (�����/����) � ������������
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        // ������������ �������� �� ��� Y (�����/������)
        yRotation += mouseX;

        // ������������ ������ �� ����
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

}
