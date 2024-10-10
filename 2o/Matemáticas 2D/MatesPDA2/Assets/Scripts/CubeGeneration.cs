using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGeneration : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public float offsetX = 100f;
    public float offsetY = 100f;

    public float multiplicadorDesplazamiento;
    public float velocidadCubo;

    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GenerateTexture();
    }

    void GenerateTexture()
    {
        float perlinHeight;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                perlinHeight = CalcularColor(x, y);
                if (perlinHeight < 0.4)
                {
                    
                }
            }
        }
        
    }

    float CalcularColor(int x, int y)
    {

        float xCoord = (float)x / width * scale + Random.Range(0, 999999f);
        float yCoord = (float)y / height * scale + Random.Range(0, 999999f);

        float value = Mathf.PerlinNoise(xCoord, yCoord);

        return value;
    }
}
