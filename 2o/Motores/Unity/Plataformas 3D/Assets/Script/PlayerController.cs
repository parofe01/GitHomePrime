using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public enum State { Idle, Walk, Jump, Falling, EndJump ,Attack, Hurt, Die, Win }
    public State state;

    Vector2 lastMovement;
    public float movementSpeed;
    public float rotationSpeed;
    public float jumpForce;
    public float health;
    public float healthMax;
    public float damage;
    

    // Inputs
    Vector2 joystick;
    public bool grounded = false;

    // Components
    Animator animator;
    Rigidbody rb;
    
    // GameObjects
    GameObject playerCameraController;
    public GameObject damageCheck;
    public Text healthTextLbl;
    
    

    // Start is called before the first frame update
    void Start()
    {
        damageCheck.SetActive(false);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        health = healthMax;
    }

    void Update()
    {
        UpdateCanvas();
    }

    void FixedUpdate()
    {
        Movement();
    }

    public void UpdateCanvas()
    {
        healthTextLbl.text = "HP: " + health.ToString("000") + "/" + healthMax.ToString("000");
    }

    public void SetState(State s)
    {
        state = s;
    }

    void Movement()
    {

        if (state == State.Walk)
        {
            float angle = Mathf.Atan2(lastMovement.x, lastMovement.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * (movementSpeed * Time.fixedDeltaTime));
        }
        if (joystick.magnitude <= 0.2f && state != State.Jump && state != State.Falling && state != State.EndJump)
        {
            state = State.Idle;
        }
        

        if (state == State.Falling && grounded)
        {
            Debug.Log("Grounded");
            animator.SetTrigger("EndFall");
            state = State.EndJump;
        }

        if (transform.position.y < -20)
        {
            TakeDamage(healthMax+1);
        }
    }
    
    // Inputs
    public void InputMove(InputAction.CallbackContext context)
    {
        joystick = context.ReadValue<Vector2>();
        lastMovement = joystick;
        animator.SetFloat("Speed", joystick.magnitude);
        if ((state == State.Idle || state == State.Walk ) && joystick.magnitude > 0.1f)
        {
            state = State.Walk;
        }
    }

    public void InputJump(InputAction.CallbackContext context)
    {
        if (context.started && (state == State.Idle || state == State.Walk))
        {
            rb.AddForce(Vector3.up * jumpForce + transform.forward * joystick.magnitude * movementSpeed, ForceMode.Impulse);
            animator.SetTrigger("Jump");
            state = State.Jump;
        }
    }


    public void InputAttack(InputAction.CallbackContext context)
    {
        if (context.started && (state == State.Idle || state == State.Walk))
        {
            state = State.Attack;
            animator.SetTrigger("Attack");
        }
    }

    public void EndAction()
    {
        if (joystick.magnitude > 0.1f)
        {
            state = State.Walk;
        }
        else
        {
            state = State.Idle;
        }
    }

    void Heal(float h)
    {
        health += h;
        if (health > healthMax)
        {
            health = healthMax;
        }
    }
    
    [ContextMenu("TakeDamage")]
    public void TakeDamage(float damage)
    {
        Debug.Log("Taking Damage");
        if (health > 0)
        {
            Debug.Log("damaged");
            health -= damage;
            animator.SetTrigger("Hurt");
            state = State.Hurt;
            if (health <= 0)
            {
                Debug.Log("Deadisimo");
                health = 0;
                state = State.Die;
                animator.SetTrigger("Die");
                Invoke("RestartLevel", 3);
            }
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DamageColliderOn()
    {
        damageCheck.SetActive(true);
    }

    public void DamageColliderOff()
    {
        damageCheck.SetActive(false);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("entro");
            TakeDamage(other.gameObject.GetComponentInParent<EnemyScript>().damage);
        }

        if (other.gameObject.CompareTag("Heal"))
        {
            health = healthMax;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Star"))
        {
            state = State.Win;
            animator.SetTrigger("Win");
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}