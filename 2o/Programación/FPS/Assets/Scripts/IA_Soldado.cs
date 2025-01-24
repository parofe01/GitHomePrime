using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class IA_Soldado : MonoBehaviour
{
    public enum State { Idle, Run, Shoot }
    public State state;

    public float health = 100;

    public float visionRange;
    public float shootingRange;

    public float playerDistance;

    Animator animator;
    Weapon weapon;

    public NavMeshAgent nmAgent;
    public GameObject shootHere;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
        shootHere = GameObject.Find("ShootHere");
    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 0) Destroy(gameObject);
        StateMachine();
        playerDistance = (shootHere.transform.position - transform.position).magnitude;
    }

    //private void FixedUpdate()
    //{
    //    Vector3 boxPosition = transform.position; // Box position
    //    Vector3 boxHalfSize = Vector3.one * 0.5f; // Half size of the box
    //    Quaternion boxRotation = transform.rotation; // Box rotation
    //    Vector3 castDirection = transform.forward; // Cast direction
    //    RaycastHit hitResult; // Stores the result
    //    // Call BoxCast
    //    if (Physics.BoxCast(boxPosition, boxHalfSize, castDirection, out hitResult, boxRotation))
    //    {
    //        Debug.Log($"BoxCast hitted: {hitResult.collider.name}");
    //    }
    //}

    void StateMachine()
    {
        
        switch(state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Run:
                Run();
                break;
            case State.Shoot:
                Shoot();
                break;
        }
    }

    void SetState(State s)
    {
        state = s;
    }

    void Idle()
    {
        animator.Play("demo_combat_idle");

        if (playerDistance < visionRange)
        {
            SetState(State.Run);
        }
    }

    void Run()
    {
        animator.Play("demo_combat_run");

        
        nmAgent.SetDestination(shootHere.transform.position);
        if (playerDistance > visionRange)
        {
            SetState(State.Idle);
        }
        if (playerDistance < 10)
        {
            nmAgent.SetDestination(transform.position);
            SetState(State.Shoot);
        }
    }

    void Shoot()
    {
        animator.Play("demo_combat_shoot");

        transform.LookAt(shootHere.transform.position);
        weapon.EnemyShoot();

        if (playerDistance > shootingRange)
        {
            SetState(State.Run);
        }
        
    }
}
