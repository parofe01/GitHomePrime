using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScriptEnemy : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.name == "Player")
        {
            
            col.gameObject.GetComponent<PlayerScript>().TakeDamage();
           

        }
        

    }
}
