using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public float offsetX = 100f; 
    public float offsetY = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalcularColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    Color CalcularColor(int x, int y)
    {
        
        float xCoord = (float)x/width * scale + Random.Range(0, 999999f);
        float yCoord = (float)y/height * scale + Random.Range(0, 999999f);

        float value = Mathf.PerlinNoise(xCoord, yCoord);

        return new Color(value, value, value);
    }
}
