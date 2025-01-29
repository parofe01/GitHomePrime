using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayer : MonoBehaviour
{
    public float myspeed;
    public GameObject hitobject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * myspeed, Space.Self);
    }


    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<EnemyIA>().TakeDamage();
            Instantiate(hitobject, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
        if (col.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
}

