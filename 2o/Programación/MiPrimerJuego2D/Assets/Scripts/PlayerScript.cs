using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update

    int vida;
    string nombre;
    float estatura;
    bool vivo;
    float playerSpeed;


    void Start()
    {
        vida = 100;
        vivo = true;
        playerSpeed = 10;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        
    }

    void Inputs()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector2.up * playerSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            // Space.World hace que el personaje se mueva en base al eje de coordenasa del mundo, usado sobretodo en plataformas 2D
            // puesto que independientemente de la rotacion del personaje, las coordenadas que cambian son con respecto a la rotación del mundo
            // Space.Self hace que el personaje se mueva en base a su propio eje de coordenas, usado sobretodo en juegos 3D
            // puesto que al rotar el personaje te interesa que vaya a la dirección a la que mira
            transform.Translate(Vector2.left * playerSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector2.down * playerSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * playerSpeed * Time.deltaTime, Space.World);
        }
    }
}
