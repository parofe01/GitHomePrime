using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShell : MonoBehaviour {

    public float fireForce;
    
    public Drive drive;
    public GameObject bullet;
    public GameObject turret;
    public GameObject enemy;

    void Start () {
        drive = GetComponent<Drive>();
    }

    void CreateBullet() {

        GameObject cheese = Instantiate(bullet, turret.transform.position, turret.transform.rotation);
        cheese.gameObject.GetComponent<Rigidbody>().velocity = fireForce * turret.transform.forward;
        drive.quesos--;
    }

    void Update() {


        if (Input.GetKeyDown(KeyCode.Space) && drive.quesos > 0) 
        {
                CreateBullet();
        }
    }
}
