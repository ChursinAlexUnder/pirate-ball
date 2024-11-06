using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public float amplitude = 2f; // Высота волны
    public float speed = 1f; // Скорость колебания
    private float baseHeight; // Базовая высота воды, к которой добавляются колебания

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            baseHeight = transform.position.y; // Сохраняем начальную высоту воды
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Update()
    {
        // Меняем положение воды по оси Y для всей поверхности
        float newY = baseHeight + amplitude * Mathf.Sin(Time.time * speed);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // Метод больше не нужен для подъема волн на основе позиции
    public float GetWaveHeight(float _x)
    {
        // Высота воды теперь единая для всей поверхности
        return transform.position.y;
    }
}
