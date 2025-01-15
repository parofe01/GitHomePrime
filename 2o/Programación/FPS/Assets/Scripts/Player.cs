using UnityEngine;

public class Player : MonoBehaviour
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
    public Weapon scriptShotgun;
    public Weapon scriptPistol;
    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        if (weaponInUse == 0)
        {
            pistol.SetActive(true);
            shotgun.SetActive(false);
        }
        else if (weaponInUse == 1)
        {
            pistol.SetActive(false);
            shotgun.SetActive(true);
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
                    scriptShotgun.AR1Shoot();
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
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Q) && !scriptPistol.reloading && !scriptShotgun.reloading)
        {
            if (weaponInUse == 0)
            {
                weaponInUse = 1;
                pistol.SetActive(false);
                shotgun.SetActive(true);
            }
            else if (weaponInUse == 1)
            {
                weaponInUse = 0;
                pistol.SetActive(true);
                shotgun.SetActive(false);
            }
        }
    }

    void Status()
    {
        if (health <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
    }
}
