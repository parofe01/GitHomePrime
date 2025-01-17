using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dificulty : MonoBehaviour
{
    public static Dificulty Instance { get; private set; } // Singleton para acceso global
    public bool dificulty; // Variable para almacenar la dificultad

    private void Awake()
    {
        // Si ya existe una instancia, destruye el nuevo objeto
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Asigna esta instancia y persiste entre escenas
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
