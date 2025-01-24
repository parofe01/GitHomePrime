using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    public bool holdClick;
    public float bulletDispersion;
    public int inUse;
    public int maxBullets;
    public int bullets;
    public float fireRate;
    public float fireRateCooldown;
    public float reloadMaxTime;
    public float reloadTime;
    public bool reloading;
    public float maxDispersion = 2f; // Máxima dispersión permitida
    public float dispersionIncrement = 0.1f; // Cuánto aumenta la dispersión por disparo
    public float dispersionDecayRate = 1f; // Velocidad a la que la dispersión vuelve a reducirse
    public float currentDispersion = 0f; // Dispersión actual acumulativa

    public RaycastHit hit;

    public enum Type { Pistol, AR1, Shootgun };
    public Type type;

    public GameObject bullet;
    public GameObject shootPoint;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        bullets = maxBullets;
    }

    // Update is called once per frame
    void Update()
    {
        Timers();
        DecayDispersion();
    }

    //Animations
    void AniShooting()
    {
        switch (type)
        {
            case Type.Pistol:
                animator.Play("pistol1_hands_Fire_pistol1");
                break;
            case Type.AR1:
                animator.Play("ar1(automatic rifle)_hands_Fire_ar1(automatic rifle)");
                break;
            case Type.Shootgun:
                animator.Play("shotgun1_hands_Fire_shotgun1");
                break;
        }
    }
    

    void AniReloading()
    {
        switch (type)
        {
            case Type.Pistol:
                animator.Play("pistol1_hands_Reload_pistol1");
                break;
            case Type.AR1:
                animator.Play("ar1(automatic rifle)_hands_Reload_ar1(automatic rifle)");
                break;
            case Type.Shootgun:
                animator.Play("shotgun1_hands_ReloadNoAmmo_shotgun1");
                break;
        }
    }

    void DecayDispersion()
    {
        if (currentDispersion > 0f)
        {
            currentDispersion -= dispersionDecayRate * Time.deltaTime;
            currentDispersion = Mathf.Max(currentDispersion, 0f); // Asegura que no sea negativa
        }
    }


    // Player Weapons
    public void PistolShoot()
    {
        if (fireRateCooldown <= 0 && bullets > 0 && !reloading)
        {
            AniShooting();
            fireRateCooldown = fireRate;
            bullets--;

            Instantiate(bullet, shootPoint.transform.position, transform.rotation);
            Physics.Raycast(shootPoint.transform.position, shootPoint.transform.forward, out hit);
            Debug.DrawRay(shootPoint.transform.position, shootPoint.transform.forward * 50, Color.red, 2f);

            if (Physics.Raycast(shootPoint.transform.position, shootPoint.transform.forward, out hit))
            {
                Debug.Log("Impacto con: " + hit.collider.name);
                if (hit.collider.name == "Enemy")
                {
                    GameObject enemy = hit.collider.gameObject;
                    IA_Soldado ene = enemy.GetComponent<IA_Soldado>();
                    ene.health -= 15;
                }
            }
        
        }
    }

    public void AR1Shoot()
    {
        if (fireRateCooldown <= 0 && bullets > 0 && !reloading)
        {
            AniShooting();
            fireRateCooldown = fireRate;
            bullets--;

            // Aumenta la dispersión acumulativa hasta el límite
            currentDispersion = Mathf.Min(currentDispersion + dispersionIncrement, maxDispersion);

            // Aplica dispersión al disparo
            Quaternion dispersion = Quaternion.Euler(
                Random.Range(-currentDispersion, currentDispersion),
                Random.Range(-currentDispersion, currentDispersion),
                0f
            );

            // Instancia la bala con dispersión
            Instantiate(bullet, shootPoint.transform.position, shootPoint.transform.rotation * dispersion);

            // Realiza un Raycast con dispersión
            Vector3 rayDirection = (shootPoint.transform.rotation * dispersion) * Vector3.forward;
            if (Physics.Raycast(shootPoint.transform.position, rayDirection, out hit))
            {
                Debug.Log("Impacto con: " + hit.collider.name); 
                if (hit.collider.name == "Enemy")
                {
                    GameObject enemy = hit.collider.gameObject;
                    IA_Soldado ene = enemy.GetComponent<IA_Soldado>();
                    ene.health -= 15;
                }
            }
        

            // Dibuja el rayo para depuración
            Debug.DrawRay(shootPoint.transform.position, rayDirection * 50, Color.red, 2f);
        }
    }


    public void ShotgunShoot()
    {
        AniShooting();
        if (fireRateCooldown <= 0 && bullets > 0 && !reloading)
        {
            fireRateCooldown = fireRate;
            bullets--;


            for (int i = 0; i < 15; i++)
            {
                Quaternion dispersion = Quaternion.Euler(Random.Range(-bulletDispersion, bulletDispersion),
                                                Random.Range(-bulletDispersion, bulletDispersion),
                                                Random.Range(-bulletDispersion, bulletDispersion));
                Vector3 rayDirection = (shootPoint.transform.rotation * dispersion) * Vector3.forward;
                if (Physics.Raycast(shootPoint.transform.position, rayDirection, out hit))
                {
                    Debug.Log("Impacto con: " + hit.collider.name);
                    if (hit.collider.name == "Enemy")
                    {
                        GameObject enemy = hit.collider.gameObject;
                        IA_Soldado ene = enemy.GetComponent<IA_Soldado>();
                        ene.health -= 10;
                    }
                }
                Instantiate(bullet, shootPoint.transform.position, transform.rotation * dispersion);
                Debug.DrawRay(shootPoint.transform.position, rayDirection * 50, Color.red, 2f);
            }


        }
    }

    public void Reload()
    {
        if (!reloading && maxBullets != bullets)
        {
            AniReloading();
            reloading = true;
            reloadTime = reloadMaxTime;
        }
    }

    public void Timers()
    {

        if (reloading)
        {
            reloadTime -= Time.deltaTime;
            if (reloadTime <= 0f)
            {
                reloading = false;
            }
            if (reloading == false)
            {
                bullets = maxBullets;
            }
        }

        if (fireRateCooldown > 0)
        {
            fireRateCooldown -= Time.deltaTime;
        }

    }

    public void EnemyShoot()
    {
        currentDispersion = maxDispersion;
        if (fireRateCooldown <= 0)
        {


            fireRateCooldown = fireRate;

            // Aplica dispersión al disparo
            Quaternion dispersion = Quaternion.Euler(
                Random.Range(-currentDispersion, currentDispersion),
                Random.Range(-currentDispersion, currentDispersion),
                0f
            );

            // Instancia la bala con dispersión
            Instantiate(bullet, shootPoint.transform.position, shootPoint.transform.rotation * dispersion);

            // Realiza un Raycast con dispersión
            Vector3 rayDirection = (shootPoint.transform.rotation * dispersion) * Vector3.forward;
            if (Physics.Raycast(shootPoint.transform.position, rayDirection, out hit))
            {
                Debug.Log("Impacto con: " + hit.collider.name);
                if (hit.collider.name == "Player")
                {
                    GameObject player = hit.collider.gameObject;
                    PlayerController pla = player.GetComponent<PlayerController>();
                    pla.health -= 15;
                }

                // Dibuja el rayo para depuración
                Debug.DrawRay(shootPoint.transform.position, rayDirection * 50, Color.red, 2f);
            }
        }
    }

}
