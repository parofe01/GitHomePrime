using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadScene(string nombre)
    {
        Debug.Log("Entro" + nombre);
        SceneManager.LoadScene(nombre);
    }

    public void ClickStart()
    {
        LoadScene("Demo_SKS");
    }
    public void ClickExit()
    {
        Application.Quit();
    }

}
