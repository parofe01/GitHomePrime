using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class disparar : MonoBehaviour
{
    public GameObject bola;
    float timeAux;
    TextMeshPro texto;
    Rigidbody2D rb;
    probabilidades pr;

    // Start is called before the first frame update
    void Start()
    {
        texto = GetComponent<TextMeshPro>();
        timeAux = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float timeDif = Time.time - timeAux;

        //if (Input.GetKeyDown(KeyCode.Space) && (timeDif > 0.5f))
        if (timeDif > 2f)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z+0.5f);
            GameObject clon = Instantiate(bola, pos, Quaternion.identity) as GameObject;
            clon.SetActive(true);

            rb = clon.GetComponent<Rigidbody2D>();

            Vector3 direccion = new Vector3(pr.P01FuerzaFija(500f), 0f, 0f);
            rb.AddForce(direccion);

            timeAux = Time.time;
        }
    }
}
