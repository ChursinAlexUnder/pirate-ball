using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform ball; // ������ �� ���
    public Vector3 offset; // �������� ������
    public float mouseSensitivity = 2.0f; // ���������������� ����
    public Camera ballCamera; // ������ �� ���� ������

    private float rotationY = 0.0f; // ������������ �������
    private float rotationX = 0.0f; // �������������� �������
    private Quaternion initialRotation; // ����������� ������� ������

    void Start()
    {
        // ����������� ������� ������
        initialRotation = transform.rotation;

        // ������������� ��������� ��������
        offset = transform.position - ball.position;

        // �������� ������ � ��������� ��� � ������ ������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // ���������, ������� �� ������
        if (!ballCamera.enabled) return;

        // �������� ���� ����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // ��������� ������������ � �������������� �������
        rotationY += mouseY;
        rotationX += mouseX;

        // ������������ ������������ ����
        rotationY = Mathf.Clamp(rotationY, -30f, 19f);

        // ������������ ������ ������ ����
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
        transform.position = ball.position + rotation * offset;

        // ������� �� ���
        transform.rotation = initialRotation * rotation; // ��������� ��������� �������
        transform.LookAt(ball.position);
    }

}
