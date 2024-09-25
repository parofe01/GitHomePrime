using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ejercicios : MonoBehaviour
{
    // Variables

    public int selectActivity, n, n1, n2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Activities();
        }
    }
    void Activities()
    {
        switch (selectActivity)
        {
            // Ejercicio 1
            case 1:
                EJ1();
                break;
            // Ejercicio 2
            case 2:
                EJ2();
                break;
            // Ejercicio 3
            case 3:
                EJ3();
                break;
            // Ejercicio 4
            case 4:
                EJ4();
                break;
            // Ejercicio 5
            case 5:
                EJ5();
                break;
            // Ejercicio 6
            case 6:
                EJ6();
                break;
            // Ejercicio 7
            case 7:
                EJ7();
                break;
            // Ejercicio 8
            case 8:
                EJ8();
                break;
            // Ejercicio 9
            case 9:
                EJ9();
                break;
            // Ejercicio 10
            case 10:
                EJ10();
                break;
        }

        
    }

    void EJ1()
    {
        for (int i = 1; i <= 100; i++)
        {
            Debug.Log(i);
        }
    }

    void EJ2()
    {
        for (int i = 100; i >= 1; i--)
        {
            Debug.Log(i);
        }
    }
    void EJ3()
    {
        for (int i = 0; i <= 100; i+=2)
        {
            Debug.Log(i);
        }
    }
    void EJ4()
    {
        for (int i = 100; i >= 1; i-=5)
        {
            Debug.Log(i);
        }
    }
    void EJ5()
    {
        int suma = 0;
        for (int i = 0;i <= 100; i++)
        {
            suma = suma + i;
            Debug.Log(suma);
        }
    }
    void EJ6()
    {
        int suma = 0;
        for (int i = 100; i >= 1; i -= 5)
        {
            if(i%2 == 0)
            {
                suma += i;
                Debug.Log(suma);
            }  
        }
    }
    void EJ7()
    {
        for (int i = n1; i <= n2  ; i++)
        { 
            Debug.Log(i);
        }
    }
    void EJ8()
    {
        string asterisco = "";
        for (int i = 0; i<= n ; i++)
        {
            asterisco += "*";
            Debug.Log("*");
        }
        Debug.Log(asterisco);
    }
    void EJ9()
    {
        for (int i = 0; i<= 10 ; i++)
        {
            Debug.Log("9 x " + i + " = " + (9 * i));
        }
    }
    void EJ10()
    {
        for (int i = 1; i <= 10; i++)
        {
            Debug.Log("Tabla del " + i);
            for (int j = 0; j <= 10; j++)
            {
                Debug.Log(i + " x " + j + " = " + (i * j));
            }
        }
    }
}
