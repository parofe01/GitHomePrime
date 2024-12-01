using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScriptEnemy : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.layer != 1 << 6)
        {
            col.gameObject.GetComponent<PlayerScript>().TakeDamage();
           

        }
        

    }
}
