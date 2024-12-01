using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detroyer : MonoBehaviour
{  
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Indestructible"))
        {
            Destroy(other.gameObject);
        }
    }
}
