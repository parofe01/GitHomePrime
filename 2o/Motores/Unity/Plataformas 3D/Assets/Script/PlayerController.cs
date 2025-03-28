using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    public enum State { Idle ,Walk, Jump, Attack, Hurt, Die, Win }
    public State state;
    
    Vector2 lastMovement;
    Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
    }

    void StateMachine()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Walk:
                break;
            case State.Jump:
                break;
            case State.Attack:
                break;
            case State.Hurt:
                break;
            case State.Die:
                break;
            case State.Win:
                break;
        }
    }
    
    // Inputs

    public void Move(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().magnitude > 0.1f)
        {
            lastMovement = context.ReadValue<Vector2>();
        }
        
        float angle = Mathf.Atan2(lastMovement.x, lastMovement.y) * Mathf.Rad2Deg;
        
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        rb.MoveRotation(rotation);
        rb.AddTorque(lastMovement.normalized * context.ReadValue<float>());
    }
}
