using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public GameObject hitobject;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name != "Player")
        {
            col.gameObject.GetComponent<SkeletonScript>().Hurt();
            Instantiate(hitobject, transform.position, transform.rotation);

        }


    }
}
