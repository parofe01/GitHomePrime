using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGeneration : MonoBehaviour
{
    public int width = 10;
    public int height = 10;

    public float multiplicadorDesplazamiento;
    public float velocidadCubo;
    float timer = 0f;

    public GameObject cube;
    public GameObject powerUp;
    public GameObject torreta;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        cube.SetActive(false);
        if (timer <= 0f)
        {
            GenerateObjects();
        }
        GeneratorTimer();


    }

    void GeneratorTimer()
    {
        timer -= Time.deltaTime;
        Debug.Log(timer);
    }

    void GenerateObjects()
    {
        
        float perlinHeight;
        for (float x = -4.5f; x < 5.5; x++)
        {
            for (int y = 0; y < height; y++)
            {
                perlinHeight = CalcularAltura(x, y);
                if (perlinHeight < 0.04)
                {
                    Vector3 pos = new Vector3(x, 0.5f, 10);
                    Instantiate<GameObject>(cube, pos, Quaternion.identity);
                }
                else
                {
                    GenerarTorreta(x);
                }
            }
        }
        timer = 5;
    }

    void GenerarTorreta(float x)
    {
        int value = Random.Range(0, 101);
        if (value < 5)
        {
            Vector3 pos = new Vector3(x, 0.5f, 10);
            Instantiate<GameObject>(torreta, pos, Quaternion.identity);
        }
        if (value > 95)
        {
            Vector3 pos = new Vector3(x, 0.5f, 10);
            Instantiate<GameObject>(powerUp, pos, Quaternion.identity);
        }
    }

    float CalcularAltura(float x, int y)
    {

        float xCoord = (float)x / 1 + Random.Range(0, 999999f);
        float yCoord = (float)y / 1 + Random.Range(0, 999999f);

        float value = Mathf.PerlinNoise(xCoord, yCoord);

        return value;
    }

    
}
