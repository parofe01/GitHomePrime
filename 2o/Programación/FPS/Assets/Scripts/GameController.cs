using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityStandardAssets.Utility.TimedObjectActivator;

public class GameController : MonoBehaviour
{
    public GameObject Win;
    public GameObject Lose;

    

    public GameObject[] enemies;
    bool enemyAlive;

    public GameObject player;
    public PlayerController playerController;
    // Start is called before the first frame update
    private void Awake()
    {
        Time.timeScale = 1.0f;
    }
    void Start()
    {
        Time.timeScale = 1.0f;

        Win = GameObject.Find("Win");
        Lose = GameObject.Find("Lose");

        Win.SetActive(false);
        Lose.SetActive(false);

        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        GameStateCheck();
        GameEndCheck();
    }

    void GameStateCheck()
    {
        // Verifica si al menos un enemigo no es null
        enemyAlive = false;
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemyAlive = true;
                break;
            }
        }
    }

    void GameEndCheck()
    {
        Debug.Log(playerController.health);
        if (enemyAlive)
        {
            Lose.SetActive(true);
            Time.timeScale = 0.0f;
        }
        if (playerController.health <= 0)
        {
            playerController.health = 0;
            Win.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
