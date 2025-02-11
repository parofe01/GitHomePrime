using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Player Data
    public int maxHealth = 100;
    public int health = 100;
    public float playerSpeed = 5;
    public float jumpSpeed;
    public bool jumping;
    public int weaponInUse;
    public GameObject shotgun;
    public GameObject pistol;
    public GameObject ar1;
    public GameObject TextVida;
    public Weapon scriptAR1;
    public Weapon scriptShotgun;
    public Weapon scriptPistol;
    public Text LabelVida;
    private Rigidbody rigid;

    private void Awake()
    {
        health = 100;
    }
    void Start()
    {
        health = 100;

        rigid = GetComponent<Rigidbody>();
        TextVida = GameObject.Find("TextVida");
        LabelVida = TextVida.GetComponent<Text>();

        if (weaponInUse == 0)
        {
            pistol.SetActive(true);
            shotgun.SetActive(false);
            ar1.SetActive(false);
        }
        else if (weaponInUse == 1)
        {
            pistol.SetActive(false);
            shotgun.SetActive(true);
            ar1.SetActive(false);
        }
        else
        {
            pistol.SetActive(false);
            shotgun.SetActive(false);
            ar1.SetActive(true);
        }
    }

    void Update()
    {
        Inputs();
        Status();
    }

   
    public void Inputs()
    {
        //Movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * playerSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * playerSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * playerSpeed * Time.deltaTime);
        }
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !jumping)
        {
            jumping = true;
            rigid.velocity = new Vector2(0, jumpSpeed);
        }
        if(jumping == true && rigid.velocity.y == 0)
        {
            jumping = false;
        }

        //Weapon Management
        switch (weaponInUse)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    scriptPistol.PistolShoot();
                }
                break;

            case 1:
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    scriptShotgun.ShotgunShoot();
                }
                break;
            case 2:
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    scriptAR1.AR1Shoot();
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            switch (weaponInUse)
            {
                case 0:
                    scriptPistol.Reload();
                    break;

                case 1:
                    scriptShotgun.Reload();
                    break;
                case 2:
                    scriptAR1.Reload();
                    break;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.F3) && !scriptPistol.reloading && !scriptShotgun.reloading && !scriptAR1.reloading)
        {
             weaponInUse = 0;
             pistol.SetActive(true);
             shotgun.SetActive(false);
             ar1.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.F2) && !scriptPistol.reloading && !scriptShotgun.reloading && !scriptAR1.reloading)
        {
            weaponInUse = 1;
            pistol.SetActive(false);
            shotgun.SetActive(true);
            ar1.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.F1) && !scriptPistol.reloading && !scriptShotgun.reloading && !scriptAR1.reloading)
        {
            weaponInUse = 2;
            pistol.SetActive(false);
            shotgun.SetActive(false);
            ar1.SetActive(true);
        }
    }

    void Status()
    {
        LabelVida.text = "VIDA: " + health;
        if (health <= 0)
        {
            health = 0;
            Debug.Log("Game Over");
            Time.timeScale = 0;
        }
    }
}
