using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool vertical = false;
    public float speed, posMax, posMin;
    float mov = 1;
    public Vector2 posInicial;

    // Start is called before the first frame update
    void Start()
    {
        posInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!vertical)
        {
            transform.Translate(new Vector2(mov, 0) * Time.deltaTime * speed, Space.World);
            if (transform.position.x > posInicial.x + posMax)
            {
                mov = -1;
            }

            if (transform.position.x < posInicial.x - posMin)
            {
                mov = 1;
            }
        }
        else
        {
            transform.Translate(new Vector2(0, mov) * Time.deltaTime * speed, Space.World);
            if (transform.position.y > posInicial.y + posMax)
            {
                mov = -1;
            }

            if (transform.position.y < posInicial.y - posMin)
            {
                mov = 1;
            }
        }
    }
}
