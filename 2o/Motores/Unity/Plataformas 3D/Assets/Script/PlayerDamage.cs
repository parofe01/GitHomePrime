using System;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public GameObject player;
    public PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponentInParent<EnemyScript>().TakeDamage(playerController.damage);
        }
    }
}
