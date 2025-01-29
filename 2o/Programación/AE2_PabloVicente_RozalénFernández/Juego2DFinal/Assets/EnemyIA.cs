using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    public enum State { Walk, Shoot, Hurt, Die }
    public State state;

    public float alturaDisparo;
    public bool isShooting;
    public float health = 10;
    public float speed;
    public float desplazamiento;
    public float posicion;
    public float playerDistance;

    public GameObject arrowObject;
    public GameObject trofeo;
    GameObject player;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        animator = GetComponent<Animator>();
        
        InvokeRepeating(nameof(SetShootState), 3, 3);

        posicion = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
        playerDistance = (player.transform.position - transform.position).magnitude;
    }
    void StateMachine()
    {
        switch(state)
        {
            case State.Walk:
                Walk();
                break;
            case State.Shoot:
                Shoot();
                break;
            case State.Hurt:
                Hurt();
                break;
            case State.Die:
                Die();
                break;
        }
    }

    void SetState(State s)
    {
        state = s;
    }
    
    void Walk()
    {
        isShooting = false;
        animator.Play("Lizzard_moves_Animation");

        if ((posicion + desplazamiento) <= transform.position.x)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        if ((posicion - desplazamiento) >= transform.position.x)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        transform.Translate(speed * Time.deltaTime * Vector2.right);

    }
    void Shoot()
    {
        
        if (player.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (!isShooting)
        {
            animator.Play("lizzard_shoots_Animation"); 
        }
        isShooting = true;
    }
    public void ShootArrow()
    {
        Instantiate(arrowObject, new Vector2(transform.position.x,transform.position.y - alturaDisparo), transform.rotation);
    }

    public void Hurt()
    {
        animator.Play("lizzard_hurt_Animation");
    }

    public void Die()
    {
        animator.Play("lizzard_die_Animation");
    }

    public void TakeDamage()
    {
        health--;
        if (health > 0)
        {
            SetState(State.Hurt);

        }
        else
        {
            SetState(State.Die);
        }

    }

    void SetShootState()
    {
        if (playerDistance < 3)
        {
            SetState(State.Shoot);
        }
    }

    void DestroySelf()
    {
        Instantiate(trofeo, new Vector2(transform.position.x, transform.position.y+0.5f), transform.rotation);
        Destroy(this.gameObject);
    }

}
