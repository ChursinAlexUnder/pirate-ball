using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public float amplitude = 2f; // ������ �����
    public float speed = 1f; // �������� ���������
    private float baseHeight; // ������� ������ ����, � ������� ����������� ���������

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            baseHeight = transform.position.y; // ��������� ��������� ������ ����
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Update()
    {
        // ������ ��������� ���� �� ��� Y ��� ���� �����������
        float newY = baseHeight + amplitude * Mathf.Sin(Time.time * speed);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // ����� ������ �� ����� ��� ������� ���� �� ������ �������
    public float GetWaveHeight(float _x)
    {
        // ������ ���� ������ ������ ��� ���� �����������
        return transform.position.y;
    }
}
