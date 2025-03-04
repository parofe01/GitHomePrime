using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public enum States { idle, run, jump, finishJump, falling, attack, hurt, die }
    public States mystate;
    public float myspeed, jumpForce;
    public GameObject arrowObject;
    public GameObject puntoFinal;
    public GameObject groundCheck;
    public GroundCheckScript GroundCheckScript;

    private Animator myanimator;
    private SpriteRenderer sprite;
    private Rigidbody2D myRigid;
    public int health;

    public Text textLabel;

    public static float PosX, PosY;

    private bool canShoot = true;
    private float shootCooldown = 0.5f;
    private bool hasShotDuringAttack = false;

    private void Awake()
    {
        PosX = transform.position.x;
        PosY = transform.position.y;
    }

    void Start()
    {
        health = 2;
        myRigid = GetComponent<Rigidbody2D>();
        myanimator = GetComponent<Animator>();
        mystate = States.idle;
    }

    void Update()
    {
        PosX = transform.position.x;
        PosY = transform.position.y;

        textLabel.text = "VIDA: " + health;
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

    private void SetState(States s)
    {
        mystate = s;
    }

    private void Idle()
    {
        myanimator.Play("Idle-Animation");
        if (GroundCheckScript.tocoSuelo) myRigid.linearVelocity = Vector2.zero;

        if (Input.GetAxisRaw("Horizontal") != 0) SetState(States.run);
        if (Input.GetButtonDown("Fire1") && canShoot) SetState(States.attack);
        if (Input.GetButtonDown("Jump")) SetState(States.jump);
        if (myRigid.linearVelocity.y < 0) SetState(States.falling);
    }

    private void Run()
    {
        myanimator.Play("Run-Animation");

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
        if (myRigid.linearVelocity.y < 0) SetState(States.falling);

        if (Input.GetAxisRaw("Horizontal") == 0) SetState(States.idle);
        if (Input.GetButtonDown("Jump")) SetState(States.jump);
        if (Input.GetButtonDown("Fire1") && canShoot) SetState(States.attack);
    }

    private void Jump()
    {
        myanimator.Play("Jump-Animation");
        myRigid.linearVelocity = new Vector2(0, 8);

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            myRigid.linearVelocity = new Vector2(myspeed, jumpForce);
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            myRigid.linearVelocity = new Vector2(-myspeed, jumpForce);
        }
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            myRigid.linearVelocity = new Vector2(0, jumpForce);
        }
        SetState(States.finishJump);
    }

    private void FinishJump()
    {
        if (myRigid.linearVelocity.y < 0) SetState(States.falling);
    }

    private void Falling()
    {
        myanimator.Play("Fall-Animation");
        if (GroundCheckScript.tocoSuelo)
        {
            SetState(States.idle);
        }
    }

    private void Attack()
    {
        myanimator.Play("Shoot-Animation");

        if (!hasShotDuringAttack && canShoot)
        {
            hasShotDuringAttack = true;
            ShootArrow();
            StartCoroutine(ShootCooldown());
        }

        if (myanimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            hasShotDuringAttack = false;
            SetState(States.idle);
        }
    }

    private void ShootArrow()
    {
        if (transform.eulerAngles.y == 0)
        {
            Instantiate(arrowObject, puntoFinal.transform.position + new Vector3(0, 0, 0), transform.rotation);
        }
        else
        {
            Instantiate(arrowObject, puntoFinal.transform.position - new Vector3(0, 0, 0), transform.rotation);
        }
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    public void Hurt()
    {
        myanimator.Play("Hurt-Animation");
    }

    public void Die()
    {
        myanimator.Play("Die-Animation");
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TakeDamage()
    {
        health--;
        if (health > 0)
        {
            SetState(States.hurt);
        }
        else
        {
            SetState(States.die);
        }
    }
}
