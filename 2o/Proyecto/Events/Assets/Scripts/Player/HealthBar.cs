using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public void UpdateHealthBar(float damage)
    {
        Debug.Log("Health removed"+damage);
    }
    public void OnEnable()
    {
        PlayerHealth.OnPlayerHurt += UpdateHealthBar;
    }
    
    public void OnDisable()
    {
        PlayerHealth.OnPlayerHurt -= UpdateHealthBar;
    }
}
