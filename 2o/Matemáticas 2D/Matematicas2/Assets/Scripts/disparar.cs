using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using URandom = UnityEngine.Random;
using static UnityEditor.PlayerSettings;

public class disparar : MonoBehaviour
{
    public float functSelected;

    public float valorMax;
    public float valorMin;
    public int numDados;

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

            Vector3 direccion = new Vector3(SelectFunction(functSelected), 0f, 0f);
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
                P01FuerzaFija();
                break;
            case 2:
                P02RandomRange();
                break;
            case 3:
                P03RandomDosDados();
                break;
            case 4:
                P04RandomVariosDados();
                break;
            case 5:
                P05MaxDados();
                break;
            case 6:
                P06DescatarMinDados();
                break;
            case 7:
                P07DescatarMinYVolverATirar();
                break;
            case 8:
                P08DescatarMaxYVolverATirar();
                break;
            case 9:
                P09PosibleBonus();
                break;
        }
        return fuerzaFinal;
    }


    // Si bien en el enunciado se menciona que se desea que el maximo sea siempre 500, con el fin de parametrizarlo,
    // el valor máximo dependerá del valor que se introduzca en valorMax en el inspector, lo mismo para los minimos y
    // el numero de dados

    // Impulso con valor fijo (500)
    float P01FuerzaFija() 
    {
        return valorMax;
    }

    // Impulso aleatorio en un rango de valores definido[200, 500]
    float P02RandomRange() 
    {
        return URandom.Range(valorMin, valorMax);
    }

    // Sea la suma de dos valores aleatorios, de modo que su valor esté en el rango[0, 500]
    float P03RandomDosDados() 
    {
        float num1 = URandom.Range(0f, valorMin/2);
        float num2 = URandom.Range(0f, valorMax / 2);

        float sum = num1 + num2;

        return sum;
    }

    // Sea la suma de varios dados (éste será una variable que podamos modificar). De modo que el valor de la suma esté en el rango[0, 500]
    float P04RandomVariosDados()
    {
        float suma = 0f;

        for (int i = 0; i < numDados; i++)
        {
            suma += URandom.Range(valorMin, valorMax / numDados);
        }

        return suma;
    }

    // Se lanzan varios dados y se obtiene el mayor de ellos
    float P05MaxDados()
    {
        float mayor = 0f;
        float valor = 0f;

        for (int i = 0; i < numDados; i++)
        {
            valor = URandom.Range(valorMin, valorMax / numDados);
            if (valor > mayor)
            {
                mayor = valor;
            }
        }
        
        return mayor;
    }

    // El valor del impulso será la suma de n dados. Se lanzarán n+1 dados y se descarta el menor de ellos
    float P06DescatarMinDados()
    {
        float suma = 0f;
        float valor = 0f;
        float menor = 0f;

        for (int i = 0; i <= numDados; i++)
        {
            valor = URandom.Range(valorMin, valorMax / numDados);
            suma += valor;
            if (valor < menor)
            {
                menor = valor;
            }
        }
        suma -= menor;

        return suma;
    }
    // El valor del impulso será la suma de n dados. Se lanzarán n dados, se descartará el menor y se volverá a tirar éste
    float P07DescatarMinYVolverATirar()
    {
        float suma = 0f;
        float valor = 0f;
        float menor = valorMax;

        for (int i = 0; i <= numDados; i++)
        {
            valor = URandom.Range(valorMin, valorMax / numDados);
            suma += valor;
            if (valor < menor)
            {
                menor = valor;
            }
        }
        suma -= menor;

        valor = URandom.Range(valorMin, valorMax / numDados);
        suma += valor;

        return suma;
    }
    // El valor del impulso será la suma de n dados. Se lanzarán n+1 dados y se descarta el mayor de ellos
    float P08DescatarMaxYVolverATirar()
    {
        float suma = 0f;
        float valor = 0f;
        float mayor = valorMin;

        for (int i = 0; i <= numDados; i++)
        {
            valor = URandom.Range(valorMin, valorMax / numDados);
            suma += valor;
            if (valor > mayor)
            {
                mayor = valor;
            }
        }
        suma -= mayor;

        return suma;
    }
    // Se añadirá un bonus al impulso con una probabilidad de 20% de que esto ocurra
    float P09PosibleBonus() 
    {
        return 0;
    }





}
