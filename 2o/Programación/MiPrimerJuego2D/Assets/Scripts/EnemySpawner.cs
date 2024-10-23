using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject ememy;

    private float timeCooldown = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnemyGeneration()
    {
        Instantiate(ememy, transform.position + new Vector3( 0, 0.5f ,0), transform.rotation);
    }


    private void OnBecameVisible()
    {
        InvokeRepeating("EnemyGeneration", 2f, 4f);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
