using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float player_health = 100f;

    public delegate void OnGameOver();
    public static OnGameOver onGameOver;

    public static event Action<float> OnPlayerHurt; 

    public void TakeDamage(float damage)
    {
        player_health -= damage;
        
        OnPlayerHurt?.Invoke(damage);
        
        if (player_health <= 0f)
        {
            player_health = 0f;
            onGameOver?.Invoke();
        }
    }

    [ContextMenu("TakeDamage100")]
    public void TakeDamage100()
    {
        TakeDamage(100);
    }

}

