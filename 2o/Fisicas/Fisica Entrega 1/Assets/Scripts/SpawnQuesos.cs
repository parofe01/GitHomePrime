using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnQuesos : MonoBehaviour
{
    public float xMax, xMin, yMax, yMin;

    public Transform pozoPosition;
    public GameObject queso;
    
    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating("SpawnQueso", 0f, 5);
    }
    public void SpawnQueso()
    {
        Vector3 position = new Vector3(Random.Range(xMin, xMax), 2f, Random.Range(yMax, yMin));
        Instantiate(queso, position, Quaternion.identity);
    }
}