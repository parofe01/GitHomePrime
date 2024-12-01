using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGeneration : MonoBehaviour
{

    public GameObject bullet;

    // Update is called once per frame
    void Start()
    {
        InvokeRepeating("GenerateBullets", 2f, 4f);
    }

    void GenerateBullets()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        Instantiate<GameObject>(bullet, pos, Quaternion.identity);

    }
}
