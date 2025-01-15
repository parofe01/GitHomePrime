using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{

    public bool roundStart = false;
    private int oleada = -1;

    public GameObject puertaEntrada;
    public GameObject puertaSalida;
    public GameObject player;
    public GameObject mosca;
    public GameObject sansS2;
    public GameObject sansS3;
    public GameObject dinger;
    public GameObject bossponja;
    public GameObject checkPoints;

    public List<GameObject> checkPointsList1;
    public List<GameObject> checkPointsList2;
    public List<GameObject> checkPointsList3;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (oleada == 0)
        {

            if (roundStart)
            {
                roundStart = false;
                for (int i = 0; i < checkPointsList1.Count; i++)
                {
                    int child = Random.Range(0, checkPoints.transform.childCount);
                    Instantiate(checkPointsList1[i], new Vector3( checkPoints.transform.GetChild(child).position.x, checkPoints.transform.GetChild(child).position.y+1, checkPoints.transform.GetChild(child).position.z), checkPoints.transform.GetChild(child).rotation);
                }
            }

            GameObject[] moscas = GameObject.FindGameObjectsWithTag("Mosca");
            GameObject[] dinger = GameObject.FindGameObjectsWithTag("Dinger");
            GameObject[] sans = GameObject.FindGameObjectsWithTag("Sans");
            GameObject[] Bossponja = GameObject.FindGameObjectsWithTag("Bossponja");

            if (moscas.Length == 0 && dinger.Length == 0 && sans.Length == 0 && Bossponja.Length == 0)
            {
                oleada = 1;
                roundStart = true;
            }
        }

        if (oleada == 1)
        {

            if (roundStart)
            {
                roundStart = false;
                for (int i = 0; i < checkPointsList2.Count; i++)
                {
                    int child = Random.Range(0, checkPoints.transform.childCount);
                    Instantiate(checkPointsList2[i], new Vector3(checkPoints.transform.GetChild(child).position.x, checkPoints.transform.GetChild(child).position.y + 1, checkPoints.transform.GetChild(child).position.z), checkPoints.transform.GetChild(child).rotation);
                }
            }

            GameObject[] moscas = GameObject.FindGameObjectsWithTag("Mosca");
            GameObject[] dinger = GameObject.FindGameObjectsWithTag("Dinger");
            GameObject[] sans = GameObject.FindGameObjectsWithTag("Sans");
            GameObject[] Bossponja = GameObject.FindGameObjectsWithTag("Bossponja");

            if (moscas.Length == 0 && dinger.Length == 0 && sans.Length == 0 && Bossponja.Length == 0)
            {
                oleada = 2;
                roundStart = true;
            }
        }

        if (oleada == 2)
        {

            if (roundStart)
            {
                roundStart = false;
                for (int i = 0; i < checkPointsList3.Count; i++)
                {
                    int child = Random.Range(0, checkPoints.transform.childCount);
                    Instantiate(checkPointsList3[i], new Vector3(checkPoints.transform.GetChild(child).position.x, checkPoints.transform.GetChild(child).position.y + 1, checkPoints.transform.GetChild(child).position.z), checkPoints.transform.GetChild(child).rotation);
                }
            }

            GameObject[] moscas = GameObject.FindGameObjectsWithTag("Mosca");
            GameObject[] dinger = GameObject.FindGameObjectsWithTag("Dinger");
            GameObject[] sans = GameObject.FindGameObjectsWithTag("Sans");
            GameObject[] Bossponja = GameObject.FindGameObjectsWithTag("Bossponja");

            if (moscas.Length == 0 && dinger.Length == 0 && sans.Length == 0 && Bossponja.Length == 0)
            {
                oleada = -1;
                roundStart = true;

                puertaSalida.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            puertaEntrada.SetActive(true);
            roundStart = true;
            oleada = 0;

            Destroy(gameObject.GetComponent<BoxCollider>());
        }
    }





}
