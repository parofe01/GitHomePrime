using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float myspeed;
    public GameObject hitobject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * myspeed, Space.Self);
    }


    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name != "Player")
        {
            col.gameObject.GetComponent<SkeletonScript>().Hurt();
            Instantiate(hitobject, transform.position, transform.rotation);
            Destroy(this.gameObject);

        }
        
        
    }


}
