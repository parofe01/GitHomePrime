using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class funciones : MonoBehaviour
{
    void funcion01()
    {
        Debug.Log("HOLA");
    }

    void funcion02(int numero1)
    {
        Debug.Log(numero1);
    }

    void funcion03(int numero1, int numero2)
    {
        int suma;
        suma = numero1 + numero2;
        Debug.Log(suma);
    }

    int funcion04(int numero1, int numero2)
    {
        int suma;
        suma = numero1 + numero2;
        return suma;
    }

    // Start is called before the first frame update
    void Start()
    {
        int a = 5;
        int b = 6;
        int resultado;

        funcion01();
        funcion02(a);
        funcion03(a, b);
        resultado = funcion04(a, b);
        Debug.Log("RECIBIDO. RESULTADO: " + resultado);

    }

    // Update is called once per frame
    void Update()
    {
        float numero = Random.Range(200, 500);
        Debug.Log(numero);
    }
}
