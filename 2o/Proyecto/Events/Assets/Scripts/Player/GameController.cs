using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);                       
    }

    private void OnEnable()
    {
        PlayerHealth.onGameOver += RestartGame;
    }

    private void OnDisable()
    {
        PlayerHealth.onGameOver -= RestartGame;
    }
}
