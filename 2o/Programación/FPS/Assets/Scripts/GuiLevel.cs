using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuiLevel : MonoBehaviour
{

    public GameObject player;
    public PlayerController playerScript;
    //GUI
    public TMP_Text guiHealthAmount;
    public TMP_Text guiAmmoAmount;
    public TMP_Text guiReloadTime;

    // Start is called before the first frame update
    void Start()
    {
        // Con esto busco el primer objeto que tenga el script del jugador como componente
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerScript.weaponInUse)
        {
            case 0:
                if (playerScript.scriptPistol.reloadTime <= 0f)
                {
                    guiReloadTime.text = "";
                }
                else
                {
                    guiReloadTime.text = "Reloading: " + playerScript.scriptPistol.reloadTime.ToString("F" + 2);
                }
                guiAmmoAmount.text = playerScript.scriptPistol.bullets.ToString();
                break;
            case 1:
                if (playerScript.scriptShotgun.reloadTime <= 0f)
                {
                    guiReloadTime.text = "";
                }
                else
                {
                    guiReloadTime.text = "Reloading: " + playerScript.scriptShotgun.reloadTime.ToString("F" + 2);
                }
                    guiAmmoAmount.text = playerScript.scriptShotgun.bullets.ToString();
                break;
            default:
                break;
        }
        
        guiHealthAmount.text = playerScript.health.ToString();

    }
}
