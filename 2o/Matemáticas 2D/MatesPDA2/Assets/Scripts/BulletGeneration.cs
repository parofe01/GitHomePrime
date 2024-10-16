using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGeneration : MonoBehaviour
{
    float timer = 0f;

    public GameObject bullet;

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
        {
            timer = 2f;
            GenerateObjects();
        }
        GeneratorTimer();
    }

    void GeneratorTimer()
    {
        timer -= Time.deltaTime;
    }

    void GenerateBullets()
    {
        Vector 3 = new Vector 3(transform.position.x, transform.position.y, transform.position.z);
        Instantiate<GameObject>(bullet, pos, Quaternion.identity);
    }
}
