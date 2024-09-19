using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class disparar : MonoBehaviour
{
    public int valorMax;
    public int valorMin;

    public GameObject bola;
    float timeAux;
    TextMeshPro texto;
    Rigidbody2D rb;

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

            Vector3 direccion = new Vector3(P02RandomRange(200f, 500f), 0f, 0f);
            rb.AddForce(direccion);

            timeAux = Time.time;
        }
    }

    // Funcion para seleccionar la funcion desde el inspector

    float SelectFunction(float f)
    {
        float fuerzaFinal = 0;
        switch (f)
        {
            case 1:
                P01FuerzaFija(500f);
                break;
            case 2:
                P02RandomRange();
                break;
            case 3:
                P03RandomDosDados();
                break;
            case 4:
                P04RandomVariosDados(5, 0f, 500f);
                break;



        }
        return fuerzaFinal;
    }




    // Impulso con valor fijo (500)
    float P01FuerzaFija(float num) 
    {
        return num;
    }

    // Impulso aleatorio en un rango de valores definido[200, 500]
    float P02RandomRange(float numMin, float numMax) 
    {
        return Random.Range(numMin, numMax);
    }

    // Sea la suma de dos valores aleatorios, de modo que su valor esté en el rango[0, 500]
    float P03RandomDosDados(float numMin, float numMax) 
    {
        float num1 = Random.Range(0f, numMin/2);
        float num2 = Random.Range(0f, numMax / 2);

        float sum = num1 + num2;

        return sum;
    }

    // Sea la suma de varios dados (éste será una variable que podamos modificar). De modo que el valor de la suma esté en el rango[0, 500]
    float P04RandomVariosDados(float dados, float numMin, float numMax)
    {
        float suma = 0f;

        for (int i = 0; i < dados; i++)
        {
            suma += Random.Range(numMin, numMax / dados);
        }

        return suma;
    }







}
