using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    private Rigidbody ball;

    private float speed = 18.0f;
    private float acceleration = 200.0f;
    private float deceleration = 200.0f;
    private float jumpForce = 40.0f; // ���� ������
    private float additionalGravityMultiplier = 0.7f; // ��������� ��� ���������� ���� ����������
    private bool isInWater = false;

    private Vector3 currentVelocity;

    public Transform cameraTransform;

    public float groundCheckRadius = 0.5f; // ������ ����� ��� �������� �����
    public LayerMask groundLayer; // ����, ������� ����� ��������� �����

    public Transform helmPosition;    // ������� � �������� ��� �������� ����
    private bool isFixed = false;

    void Start()
    {
        ball = GetComponent<Rigidbody>();
        ball.angularDrag = 0.7f;
        ball.drag = 2.0f; // ��������� ���������� ������������� ��� �������� ����������
    }

    void Update()
    {
        // �������� ���� �� ������������
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ������� ������ ����������� ������������ ������
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = (right * horizontal + forward * vertical).normalized;

        // ��������� ���������
        if (movement.magnitude > 0)
        {
            currentVelocity += movement * acceleration * Time.deltaTime;
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, speed);
        }
        else
        {
            // ��������� ����������
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        // ��������� ������� ������� ��� ������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isInWater)
            {
                JumpInWater(); // ��������� ������ ������, ���� � ����
            }
            else if (IsGrounded())
            {
                Jump(); // ������� ������ �� �����
            }
        }

        if (isFixed)
        {
            FixBallToHelm();
        }
    }

    void FixedUpdate()
    {
        // ��������� ���� ���������� ������������� � ����������� ��������
        ball.AddForce(Physics.gravity * (ball.mass * additionalGravityMultiplier), ForceMode.Acceleration);
        // ��������� ���� � Rigidbody ������ � FixedUpdate
        ball.AddForce(currentVelocity, ForceMode.Acceleration);

        if (!isInWater)
        {
            ball.velocity = Vector3.ClampMagnitude(ball.velocity, speed);
        }
    }

    private void Jump()
    {
        // ��������� ���� ����� ��� �������� ������
        ball.AddForce(Vector3.up * jumpForce / 10000, ForceMode.Impulse);
    }

    private void JumpInWater()
    {
        // ��������� ��������� ���� ����� ��� ������ � ���� (� 6 ���� ������ ��������)
        ball.AddForce(Vector3.up * jumpForce * 6 / 10000, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        // ���������, ��������� �� ��� �� ����� � �������������� SphereCast
        RaycastHit hit;
        return Physics.SphereCast(transform.position, groundCheckRadius, Vector3.down, out hit, 1.1f, groundLayer);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            ball.drag = 0.01f; // ������������� ������������� ���� ��� �����
            ball.angularDrag = 0.05f;
            ball.useGravity = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            ball.drag = 2.0f; // ���������� ������������� ��� ������ �� ����
            ball.angularDrag = 0.7f;
            ball.useGravity = true;
        }
    }

    public void FixBallToHelm()
    {
        transform.position = helmPosition.position;
        transform.rotation = helmPosition.rotation;
    }

    public void SetFixed(bool fixedStatus)
    {
        isFixed = fixedStatus;
    }
}
