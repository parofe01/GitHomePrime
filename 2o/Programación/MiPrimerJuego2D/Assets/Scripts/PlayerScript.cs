using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerScript : MonoBehaviour
{
    public enum States { idle, run, attack, bow, hurt }
    public States mystate;
    public float myspeed;
    public GameObject arrowObject;

    private Animator myanimator;
    private int vidas;

    public static float PosX, PosY;

    // Start is called before the first frame update
    private void Awake()
    {
        PosX = transform.position.x;
        PosY = transform.position.y;
    }

    void Start()
    {

        vidas = 3;
        myanimator = GetComponent<Animator>();
        mystate = States.idle;
    }

    // Update is called once per frame
    void Update()
    {
        PosX = transform.position.x;
        PosY = transform.position.y;

        switch (mystate)
        {
            case States.idle:
                Idle();
                break;
            case States.run:
                Run();
                break;
            case States.attack:
                Attack();
                break;
            case States.bow:
                Bow();
                break;
            case States.hurt:
                Hurt();
                break;
            default:
                print("Incorrect state");
                break;
        }
    }

    private void Idle()
    {
        myanimator.Play("Player_Idle");


        ///////////////////////////////

        if (Input.GetAxisRaw("Horizontal") != 0) SetState(States.run);
        if (Input.GetButtonDown("Fire1")) SetState(States.attack);
        if (Input.GetButtonDown("Fire2")) SetState(States.bow);
    }

    private void Run()
    {
        myanimator.Play("Player_Run");

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.Translate(Vector3.right * Time.deltaTime * myspeed, Space.World);
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.Translate(Vector3.left * Time.deltaTime * myspeed, Space.World);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }


        ///////////////////////////////

        if (Input.GetAxisRaw("Horizontal") == 0) SetState(States.idle);
        if (Input.GetButtonDown("Fire1")) SetState(States.attack);
        if (Input.GetButtonDown("Fire2")) SetState(States.bow);
    }

    private void Attack()
    {
        myanimator.Play("Player_Attack");


        ///////////////////////////////



    }

    private void Bow()
    {
        myanimator.Play("Player_Bow");


        ///////////////////////////////


    }

    private void SetState(States newstate)
    {
        mystate = newstate;
    }


    private void ShootArrow()
    {
        if (transform.eulerAngles.y == 0)
        {
            Instantiate(arrowObject, transform.position + new Vector3(1, 0, 0), transform.rotation);
        }
        else
        {
            Instantiate(arrowObject, transform.position - new Vector3(1, 0, 0), transform.rotation);
        }


    }



    public void Hurt()
    {
        myanimator.Play("Player_Hurt");
        
        
    }


    public void TakeDamage()
    {
        vidas--;
        if (vidas > 0)
        {
            SetState(States.hurt);

        }
        else
        {
            //SetState(States.die);
        }
        
    }


    /*
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
    }



    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("OnTrigerEnter2D");
    }
    */


}
