using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destruirBola : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
