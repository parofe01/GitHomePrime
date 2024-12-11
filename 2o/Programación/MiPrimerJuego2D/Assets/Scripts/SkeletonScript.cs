using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : MonoBehaviour
{

    public enum States { rise, walk, bury, die}
    public States mystate;
    public float myspeed,burytime;

   

    private Animator myanimator;




    // Start is called before the first frame update
    void Start()
    {

        InitialLookAt();
        
        Invoke("GoToBury", burytime);

        myanimator = GetComponent<Animator>();
        mystate = States.rise;

    }

    // Update is called once per frame
    void Update()
    {
        switch (mystate)
        {
            case States.rise:
                //Rise();
                break;
            case States.walk:
                Walk();
                break;
            case States.bury:
                Bury();
                break;
            case States.die:
                Die();
                break;
            default:
                print("Incorrect state");
                break;
        }
    }


    /*
    private void Rise()
    {

        //////

    }
    */

    private void Walk()
    {
        myanimator.Play("Skeleton_Walk");
        transform.Translate(Vector3.right * Time.deltaTime * myspeed, Space.Self);
       
        ///////

       

    }
    private void Bury()
    {
        myanimator.Play("Skeleton_Bury");
        //////


    }

    private void DestroyMe()
    {
        Destroy(this.gameObject);
    }


    private void InitialLookAt()
    {
        
        if (PlayerScript.PosX > transform.position.x )
        {
            transform.eulerAngles = new Vector3(0,0,0);
        }
        else 
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
       
    }

    private void Die()
    {
        myanimator.Play("Skeleton_Die");
       

        ///////



    }


    private void SetState(States newstate)
    {
        mystate = newstate;
    }

    public void Hurt()
    {
        SetState(States.die);
    }


    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }


    private void GoToBury()
    {
        SetState(States.bury);
    }
}
