using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using URandom = UnityEngine.Random;

public class disparar : MonoBehaviour
{
    public float functSelected;

    public float valorMax;
    public float valorMin;
    public int numDados;

    public float bonus;
    public float probabilidad;
    public bool subiendo;
    public float valorLanzamiento;

    public GameObject bola;
    float timeAux;
    public TextMeshProUGUI texto;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
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

        SubeBaja();
    }

    // Funcion para seleccionar la funcion desde el inspector

    float SelectFunction(float f)
    {
        float fuerzaFinal = 0;
        switch (f)
        {
            case 1:
                fuerzaFinal = P01FuerzaFija(valorMax);
                break;
            case 2:
                fuerzaFinal = P02RandomRange(valorMin, valorMax);
                break;
            case 3:
                fuerzaFinal = P03RandomDosDados(valorMin, valorMax);
                break;
            case 4:
                fuerzaFinal = P04RandomVariosDados(numDados, valorMin, valorMax);
                break;
            case 5:
                fuerzaFinal = P05MaxDados(numDados, valorMin, valorMax);
                break;
            case 6:
                fuerzaFinal = P06DescatarMinDados(numDados, valorMin, valorMax);
                break;
            case 7:
                fuerzaFinal = P07DescatarMinYVolverATirar(numDados, valorMin, valorMax);
                break;
            case 8:
                fuerzaFinal = P08DescatarMaxYVolverATirar(numDados, valorMin, valorMax);
                break;
            case 9:
                fuerzaFinal = P09PosibleBonus(valorMax, probabilidad, bonus);
                break;
        }
        texto.text = fuerzaFinal.ToString();
        return fuerzaFinal;
    }


    // Si bien en el enunciado se menciona que se desea que el maximo sea siempre 500, con el fin de parametrizarlo,
    // el valor máximo dependerá del valor que se introduzca en valorMax en el inspector, lo mismo para los minimos y
    // el numero de dados

    // Impulso con valor fijo (500)
    float P01FuerzaFija(float max) 
    {
        return max;
    }

    // Impulso aleatorio en un rango de valores definido[200, 500]
    float P02RandomRange(float min, float max) 
    {
        return URandom.Range(min, max);
    }

    // Sea la suma de dos valores aleatorios, de modo que su valor esté en el rango[0, 500]
    float P03RandomDosDados(float min, float max) 
    {
        float num1 = URandom.Range(min, max / 2);
        float num2 = URandom.Range(min, max / 2);

        float sum = num1 + num2;
        return sum;
    }

    // Sea la suma de varios dados (éste será una variable que podamos modificar). De modo que el valor de la suma esté en el rango[0, 500]
    float P04RandomVariosDados(int dados, float min, float max)
    {
        float suma = 0f;

        for (int i = 0; i < dados; i++)
        {
            suma += URandom.Range(min, max / dados);
        }

        return suma;
    }

    // Se lanzan varios dados y se obtiene el mayor de ellos
    float P05MaxDados(int dados, float min, float max)
    {
        float mayor = 0f;
        float valor = 0f;

        for (int i = 0; i < dados; i++)
        {
            valor = URandom.Range(min, max);

            if (valor > mayor) 
            { 
                mayor = valor;
            }

        }
        
        return mayor;
    }

    // El valor del impulso será la suma de n dados. Se lanzarán n+1 dados y se descarta el menor de ellos
    float P06DescatarMinDados(int dados, float min, float max)
    {
        float suma = 0f;
        float valor = 0f;
        float menor = 0f;

        for (int i = 0; i <= dados; i++)
        {
            valor = URandom.Range(min, max / dados);
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
    float P07DescatarMinYVolverATirar(int dados, float min, float max)
    {
        float suma = 0f;
        float valor = 0f;
        float menor = max;

        for (int i = 0; i <= dados; i++)
        {
            valor = URandom.Range(min, max / dados);
            suma += valor;
            if (valor < menor)
            {
                menor = valor;
            }
        }
        suma -= menor;

        valor = URandom.Range(min, max / dados);
        suma += valor;

        return suma;
    }
    // El valor del impulso será la suma de n dados. Se lanzarán n+1 dados y se descarta el mayor de ellos
    float P08DescatarMaxYVolverATirar(int dados, float min, float max)
    {
        float suma = 0f;
        float valor = 0f;
        float mayor = min;

        for (int i = 0; i <= dados; i++)
        {
            valor = URandom.Range(min, max / dados);
            suma += valor;
            if (valor > mayor)
            {
                mayor = valor;
            }
        }
        suma -= mayor;
        suma += URandom.Range(min, max / dados);
        return suma;
    }
    // Se añadirá un bonus al impulso con una probabilidad de 20% de que esto ocurra
    float P09PosibleBonus(float max, float prob, float b) 
    {
        float valor = max;
        if (URandom.Range(0, 100) < prob)
        {
            valor += b;
            return valor;
        }
        return valor;
    }


    // Movimiento Cañon

    void SubeBaja()
    {
        if (subiendo == true)
        {
            transform.Translate(Vector3.up * 2 * Time.deltaTime);
            if (transform.position.y > 4)
            {
                subiendo = false;
            }
        }
        else
        {
            transform.Translate(Vector3.down * 2 * Time.deltaTime);
            if (transform.position.y < -1)
            {
                subiendo = true;
            }
        }
    }

    

}
