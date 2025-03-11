using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShell : MonoBehaviour {

    public GameObject bullet;
    public GameObject turret;
    public Transform turretBase;
    public GameObject enemy;

    public float rotSpeed = 2;
    private float s = 10;
    
    void CreateBullet() {

        Instantiate(bullet, turret.transform.position, turret.transform.rotation);
    }

    float? calculateAngle(bool low)
    {
        Vector3 targetDir = enemy.transform.position - this.transform.position;
        
        float y = targetDir.y;
        float x = targetDir.magnitude;
        float g = 9.8f;
        float sqrS = s * s;
        float underSqrRoot = Mathf.Pow(s, 4) - g * (g * x * x + 2 * y * sqrS);

        if (underSqrRoot >= 0)
        {
            float highAngle = sqrS + Mathf.Sqrt(underSqrRoot);
            float lowAngle = sqrS - Mathf.Sqrt(underSqrRoot);
            if (low)
                return (Mathf.Atan2(lowAngle, g * x) * Mathf.Rad2Deg);
            else
            {
                return (Mathf.Atan2(highAngle, g * x) * Mathf.Rad2Deg);
            }
        }
        else
        {
            return null;
        }
    }

    float? rotateTurret()
    {
        float? angle = calculateAngle(true);
        if (angle != null)
        {
            turretBase.localEulerAngles = new Vector3(360f - (float)angle, 0, 0);
        }
        return angle;
    }

    void Update() {

        Vector3 dir = (enemy.transform.position - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * rotSpeed);

        float? angle = rotateTurret();
        
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
                CreateBullet();
        }
    }
}
