using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuiLevel : MonoBehaviour
{

    public Player player;
    //GUI
    public TMP_Text guiHealthAmount;
    public TMP_Text guiAmmoAmount;
    public TMP_Text guiReloadTime;

    // Start is called before the first frame update
    void Start()
    {
        // Con esto busco el primer objeto que tenga el script del jugador como componente
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (player.weaponInUse)
        {
            case 0:
                if (player.scriptPistol.reloadTime <= 0f)
                {
                    guiReloadTime.text = "";
                }
                else
                {
                    guiReloadTime.text = "Reloading: " + player.scriptPistol.reloadTime.ToString("F" + 2);
                }
                guiAmmoAmount.text = player.scriptPistol.bullets.ToString();
                break;
            case 1:
                if (player.scriptShotgun.reloadTime <= 0f)
                {
                    guiReloadTime.text = "";
                }
                else
                {
                    guiReloadTime.text = "Reloading: " + player.scriptShotgun.reloadTime.ToString("F" + 2);
                }
                    guiAmmoAmount.text = player.scriptShotgun.bullets.ToString();
                break;
            default:
                break;
        }
        
        guiHealthAmount.text = player.health.ToString();

    }
}
