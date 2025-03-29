using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public List<GameObject> enemies1 = new List<GameObject>();
    public List<GameObject> enemies2 = new List<GameObject>();
    public List<EnemyScript> enemyScripts1 = new List<EnemyScript>();
    public List<EnemyScript> enemyScripts2 = new List<EnemyScript>();
    

    public GameObject platform;
    public GameObject heal;
    public GameObject star;

    public GameObject battlezone1;
    public GameObject battlezone2;
    public BattlezoneScript bzScript1;
    public BattlezoneScript bzScript2;
    
    
    bool fase1 = true, fase2 = true;
    
    public GameObject player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var i in enemies1)
        {
            enemyScripts1.Add(i.gameObject.GetComponent<EnemyScript>());
        }
        
        platform.gameObject.SetActive(false);
        star.gameObject.SetActive(false);
        heal.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        EnemiesFase1();
        EnemiesFase2();
        EnemyTracking();
    }

    void EnemiesFase1()
    {
        if (fase1)
        {
            bool breakLoop = true;
            
            for (int i = 0; i < enemyScripts1.Count; i++)
            {
                if (enemyScripts1[i].health > 0)
                {
                    breakLoop = false;
                }

                if (breakLoop)
                {
                    platform.gameObject.SetActive(true);
                    heal.gameObject.SetActive(true);
                    fase1 = false;
                }
            }
        }
    }

    void EnemiesFase2()
    {
        if (fase2)
        {
            bool breakLoop = true;
            
            for (int i = 0; i < enemyScripts2.Count; i++)
            {
                if (enemyScripts2[i].health > 0)
                {
                    breakLoop = false;
                }

                if (breakLoop)
                {
                    platform.gameObject.SetActive(true);
                    heal.gameObject.SetActive(true);
                    fase1 = false;
                }
            }
        }
    }

    void EnemyTracking()
    {
        if(bzScript1.playerInside && fase1)
        {
            foreach (var i in enemyScripts1)
            {
                if (i.health <= 0)
                {
                    i.TrackPlayer(player.transform.position);
                }
            }
        }
        else
        {
            foreach (var i in enemyScripts1)
            {
                if (i.health <= 0)
                {
                    i.TrackPlayer(player.transform.position);
                }
            }
        }
        
        if(bzScript2.playerInside && fase2)
        {
            foreach (var i in enemyScripts2)
            {
                if (i.health <= 0)
                {
                    i.TrackPlayer(player.transform.position);
                }
            }
        }
        else
        {
            foreach (var i in enemyScripts2)
            {
                if (i.health <= 0)
                {
                    i.TrackPlayer(player.transform.position);
                }
            }
        }
    }
    
}
