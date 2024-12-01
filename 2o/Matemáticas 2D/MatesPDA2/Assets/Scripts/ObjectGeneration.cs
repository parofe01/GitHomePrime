using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectGeneration : MonoBehaviour
{
    public GameObject cube;
    public GameObject powerUp;
    public GameObject tanque;

    private float offSetX, offSetY = 0f;

    public float speedOffSetY;

    void Start()
    {
        InvokeRepeating("GenerateObjects", 0, 4);
        offSetX = Random.Range(0f, 999999f);
        offSetY = Random.Range(0f, 999999f);
    }

    void Update()
    {
        offSetY += speedOffSetY * Time.deltaTime;
    }


    // Genera los objetos primero calculando si generar o no el muro y de no hacerlo llamar al metodo que genera los extras
    void GenerateObjects()
    {
        float perlinHeight;
        for (float x = -4.5f; x < 5.5f; x++)
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

    // Calcula la generación de los tanques y las vidas
    void GenerarExtras(float x)
    {
        float value = Random.Range(0f, 100f);
        if (value < 5)
        {
            Vector3 pos = new Vector3(x, 0f, 15);
            Instantiate<GameObject>(tanque, pos, Quaternion.identity);
        }
        if (value > 95)
        {
            Vector3 pos = new Vector3(x, 0.5f, 15);
            Instantiate<GameObject>(powerUp, pos, Quaternion.identity);
        }
    }


    // Se encarga de calcular el Perlin Noise
    float CalcularAltura(float x, int y)
    {
        float xCoord = (float)x / 1000 * offSetX;
        float yCoord = (float)y / 1000 * offSetY;

        float value = Mathf.PerlinNoise(xCoord, yCoord);

        value = Random.Range(0f,1f);

        return value;
    }
    
    
}
