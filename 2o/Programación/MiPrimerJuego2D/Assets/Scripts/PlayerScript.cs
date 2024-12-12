using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public enum States { idle, run, jump, finishJump, falling, attack, bow, hurt, die }
    public States mystate;
    public float myspeed, jumpForce;
    public GameObject arrowObject;
    public GameObject puntoFinal;

    private Animator myanimator;
    private Rigidbody2D myRigid;
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
        myRigid = GetComponent<Rigidbody2D>();
        myanimator = GetComponent<Animator>();
        mystate = States.idle;
    }

    // Update is called once per frame
    void Update()
    {
        PosX = transform.position.x;
        PosY = transform.position.y;

        StateMachine();
    }
    void StateMachine()
    {
        switch (mystate)
        {
            case States.idle:
                Idle();
                break;
            case States.run:
                Run();
                break;
            case States.jump:
                Jump();
                break;
            case States.finishJump:
                FinishJump();
                break;
            case States.falling:
                Falling();
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
            case States.die:
                Die();
                break;
            default:
                print("Incorrect state");
                break;
        }
    }
    private void Idle()
    {
        myanimator.Play("Player_Idle");
        if (GroundCheckScript.tocoSuelo) myRigid.velocity = Vector2.zero;

        ///////////////////////////////

        if (Input.GetAxisRaw("Horizontal") != 0) SetState(States.run);
        if (Input.GetButtonDown("Fire1")) SetState(States.attack);
        if (Input.GetButtonDown("Fire2")) SetState(States.bow);
        if (Input.GetButtonDown("Jump")) SetState(States.jump);
        if (myRigid.velocity.y < 0) SetState(States.falling);
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
        if (myRigid.velocity.y < 0) SetState(States.falling);


        ///////////////////////////////

        if (Input.GetAxisRaw("Horizontal") == 0) SetState(States.idle);
        if (Input.GetButtonDown("Jump")) SetState(States.jump);
        if (Input.GetButtonDown("Fire1")) SetState(States.attack);
        if (Input.GetButtonDown("Fire2")) SetState(States.bow);
    }

    private void Jump()
    {
        myanimator.Play("Player_Jump");
        myRigid.velocity = new Vector2 (0, 8);

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            myRigid.velocity = new Vector2(myspeed, jumpForce);
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            myRigid.velocity = new Vector2(-myspeed, jumpForce);
        }
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            myRigid.velocity = new Vector2(0, jumpForce);
        }
        SetState(States.finishJump);
    }

    private void FinishJump()
    {
        if (myRigid.velocity.y < 0) SetState(States.falling);
    }

    private void Falling()
    {
        myanimator.Play("Player_Falling");
        if (myRigid.velocity.y == 0)
        {
            SetState(States.idle);
        }
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

    public void Die()
    {
        myanimator.Play("Player_Die");
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            SetState(States.die);
        }
        
    }

}
