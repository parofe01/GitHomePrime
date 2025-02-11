using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProyectile : MonoBehaviour
{

    private float speed = 20f;
    private float timeAlive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * 10 * Time.deltaTime);
        AutoDestroySelf();

    }

    private void AutoDestroySelf()
    {
        if (timeAlive > 2)
        {
            Destroy(gameObject);
        }
        else
        {
            timeAlive += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Pellet"))
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}
