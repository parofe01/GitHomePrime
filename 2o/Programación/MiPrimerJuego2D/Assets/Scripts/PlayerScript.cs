using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Variables

    public float playerSpeed;

    // Components

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Enumeracion

    public enum State { Idle, Walk, Attack_Sword, Attack_Bow};
    public State myState;


    void Start()
    { 

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        SetState(State.Idle);

    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
    }
    /*
    private void Inputs()
    {

        // Movimento Horizontal
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        bool inputAttack1 = Input.GetButtonDown("Fire1");

        // Animación idle
        if (inputHorizontal == 0 && !isAttacking)
        {
            animator.Play("idle_Player");
        }

        // Movimiento Horizontal + Animación andar
        if (inputHorizontal != 0 && !isAttacking)
        {
            // Space.World hace que el personaje se mueva en base al eje de coordenasa del mundo, usado sobretodo en plataformas 2D
            // puesto que independientemente de la rotacion del personaje, las coordenadas que cambian son con respecto a la rotación del mundo
            // Space.Self hace que el personaje se mueva en base a su propio eje de coordenas, usado sobretodo en juegos 3D
            // puesto que al rotar el personaje te interesa que vaya a la dirección a la que mira
            transform.Translate(Vector2.right * Time.deltaTime * playerSpeed * inputHorizontal, Space.World);

            // Movimiento a derecha
            if (inputHorizontal > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }

            // Movimiento a izquierda
            if (inputHorizontal < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }

            animator.Play("walk_Player");
        }

        // Animación Ataque 1
        if (inputAttack1)
        {
            animator.Play("attack1_Player");
        }
    }
    */
    private void StateMachine()
    {
        switch (myState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Walk:
                Walk();
                break;
            case State.Attack_Sword:
                Attack_Sword();
                break;
            case State.Attack_Bow:
                Attack_Bow();
                break;
        }
    }

    private void Idle()
    {
        animator.Play("idle_Player");

        /////////////////////////////////////////

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            SetState(State.Walk);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            SetState(State.Attack_Sword);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            SetState(State.Attack_Bow);
        }
    }

    private void Walk()
    {
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector2.right * Time.deltaTime * playerSpeed * inputHorizontal, Space.World);

        // Movimiento a derecha
        if (inputHorizontal > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        // Movimiento a izquierda
        if (inputHorizontal < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

            animator.Play("walk_Player");

        /////////////////////////////////////////

        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            SetState(State.Idle);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            SetState(State.Attack_Sword);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            SetState(State.Attack_Bow);
        }
    }
    private void Attack_Sword()
    {
        animator.Play("attack1_Player");
    }
    private void Attack_Bow()
    {
        animator.Play("bow_Player");
    }

    private void SetState(State s)
    {
        myState = s;
    }

}
