using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGeneration : MonoBehaviour
{
    public float multiplicadorDesplazamiento;
    public float velocidadCubo;
    float timer = 0f;

    public GameObject cube;
    public GameObject powerUp;
    public GameObject tanque;

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
        {
            timer = 5f;
            GenerateObjects();
        }
        GeneratorTimer();
    }

    void GeneratorTimer()
    {
        timer -= Time.deltaTime;
    }

    void GenerateObjects()
    {
        float perlinHeight;
        for (float x = -4.5f; x < 5.5; x++)
        {
            perlinHeight = CalcularAltura(x, 10);
            if (perlinHeight < 0.3)
            {
                Vector3 pos = new Vector3(x, 0.5f, 15);
                Instantiate<GameObject>(cube, pos, Quaternion.identity);
            }
            else
            {
                GenerarExtras(x);
            }
        }
    }

    void GenerarExtras(float x)
    {
        float value = Random.Range(0f, 100f);
        if (value <= 5)
        {
            Vector3 pos = new Vector3(x, 0f, 15);
            Instantiate<GameObject>(tanque, pos, Quaternion.identity);
        }
        if (value >= 95)
        {
            Vector3 pos = new Vector3(x, 0.5f, 15);
            Instantiate<GameObject>(powerUp, pos, Quaternion.identity);
        }
    }

    float CalcularAltura(float x, int y)
    {
        float xCoord = (float)x / 1000 * Random.Range(0, 999999f);
        float yCoord = (float)y / 1000 * Random.Range(0, 999999f);

        float value = Mathf.PerlinNoise(xCoord, yCoord);

        return value;
    }

    
}
