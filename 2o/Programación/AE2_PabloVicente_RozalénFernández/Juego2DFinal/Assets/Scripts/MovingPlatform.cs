using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatform : MonoBehaviour
{

    public float speed;
    public float desplazamiento;
    public float posicion;
    public bool horizontal = true;
    // Start is called before the first frame update
    void Start()
    {
        if (horizontal)
        {
            posicion = transform.position.x;
        }
        else
        {
            posicion = transform.position.y;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (horizontal)
        {
            
            if ((posicion + desplazamiento) <= transform.position.x)
            {
                speed *= -1;
            }
            if ((posicion - desplazamiento) >= transform.position.x)
            {
                speed *= -1;
            }
            transform.Translate(speed * Time.deltaTime * Vector2.right);
        }
        else
        {
            if ((posicion + desplazamiento) <= transform.position.y)
            {
                speed *= -1;
            }
            if ((posicion - desplazamiento) >= transform.position.y)
            {
                speed *= -1;
            }
            transform.Translate(speed * Time.deltaTime * Vector2.up);
        }
    }
}
