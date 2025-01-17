using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public enum State { alive, dead };
    public State state;
    public float playerSpeed;
    public float playerVida;
    public float playerTiempo;

    float inputHorizontal;
    float inputVertical;

    Rigidbody rb;
    Animator animator;
    public GameObject Canvas;
    public GameObject GameOver;
    public GameObject TextVida;
    public Text LabelVida;
    public GameObject TextTiempo;
    public Text LabelTiempo;
    public GameObject Dif;
    public Dificulty DifScript;

    public GameObject CamCP;
    public GameObject CamFB;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        Canvas = GameObject.Find("Canvas");
        GameOver = GameObject.Find("GameOver");
        TextVida = GameObject.Find("Text Vida");
        LabelVida = TextVida.GetComponent<Text>();
        TextTiempo = GameObject.Find("Text Tiempo");
        LabelTiempo = TextTiempo.GetComponent<Text>();
        Dif = GameObject.Find("Dificulty");
        DifScript = Dif.GetComponent<Dificulty>();

        SetState(State.alive);
        playerVida = 5;
        playerTiempo = 0;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        GameOver.SetActive(false);

        if (DifScript.dificulty)
        {
            CamCP.SetActive(true);
            CamFB.SetActive(false);
        }
        else
        {
            CamCP.SetActive(false);
            CamFB.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.alive)
        {
            Inputs();
            UpdateCanvas();
            playerTiempo += Time.deltaTime;
        }
        if (playerVida <= 0)
        {
            GoToGameOver();
        }
    }

    void SetState(State s)
    {
        state = s;
    }
    void Inputs()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        // Movimiento a derecha
        
        if (transform.position.x <= 4.5 && inputHorizontal > 0)
        {
            transform.Translate(Vector3.right * Time.deltaTime * playerSpeed, Space.World);
            animator.Play("player_sw_right");
        }
        if (transform.position.x >= -4.5 && inputHorizontal < 0)
        {
            transform.Translate(Vector3.left * Time.deltaTime * playerSpeed, Space.World);
            animator.Play("player_sw_left");
        }
        if (inputHorizontal == 0)
        {
            animator.Play("player_idle");
        }
        if (inputVertical > 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed, Space.World);
        }
        if (inputVertical < 0)
        {
            transform.Translate(Vector3.back * Time.deltaTime * playerSpeed, Space.World);
        }
    }

    private void UpdateCanvas()
    {
        LabelVida.text = "VIDA: " + playerVida;
        LabelTiempo.text = "TIEMPO " + (int)playerTiempo;
    }

    void GoToGameOver()
    {
        GameOver.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            playerVida += 0.5f;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Bullet"))
        {
            playerVida--;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Indestructible"))
        {
            playerVida = 0;
        }
    }
}
