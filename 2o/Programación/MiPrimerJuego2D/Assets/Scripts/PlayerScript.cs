using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update

    int vida;
    string nombre;
    float estatura;
    bool vivo;
    float playerSpeed;
    bool isAttacking = false;

    // Timers



    // Components

    private SpriteRenderer spriteRenderer;
    private Animator animator;


    void Start()
    {
        vida = 100;
        vivo = true;
        playerSpeed = 10;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        
    }

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

    public void SetIsAttackingTrue()
    {
        isAttacking = true;
    }

    public void SetIsAttackingFalse() 
    { 
        isAttacking = false; 
    }
}
