using System;
using UnityEngine;

public class BattlezoneScript : MonoBehaviour
{
    public bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " entered Battlezone");
        playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name + " exited Battlezone");
        playerInside = false;
    }
}
