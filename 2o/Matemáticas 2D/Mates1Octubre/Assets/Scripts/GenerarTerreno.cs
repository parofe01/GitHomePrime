using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class GenerarTerreno : MonoBehaviour
{
    public int depth = 20;

    public int width = 256;
    public int height = 256;

    public float offsetX = 200;
    public float offsetY = 200;

    public float scale = 20;
    public float speed = 2;


    // Start is called before the first frame update
    void Start()
    {
        offsetX = Random.Range(0, 999999);
        offsetY = Random.Range(0, 999999);
    }

    // Update is called once per frame
    void Update()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        offsetX += speed * Time.deltaTime;
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);   
            }
        }
        return heights;
    }
    
    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
