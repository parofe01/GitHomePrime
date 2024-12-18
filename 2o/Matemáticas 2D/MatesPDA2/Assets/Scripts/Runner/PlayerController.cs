using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;

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
    public GameObject Canvas;
    public GameObject GameOver;
    public GameObject TextVida;
    public Text LabelVida;
    public GameObject TextTiempo;
    public Text LabelTiempo;
    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Canvas");
        GameOver = GameObject.Find("GameOver");
        TextVida = GameObject.Find("Text Vida");
        LabelVida = TextVida.GetComponent<Text>();
        TextTiempo = GameObject.Find("Text Tiempo");
        LabelTiempo = TextVida.GetComponent<Text>();
        SetState(State.alive);
        playerVida = 5;
        playerTiempo = 0;
        rb = GetComponent<Rigidbody>();
        GameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.alive)
        {
            Inputs();
            UpdateCanvas();
        }
        if (playerVida <= 0)
        {
            Destroy(gameObject);
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
        }
        if (transform.position.x >= -4.5 && inputHorizontal < 0)
        {
            transform.Translate(Vector3.left * Time.deltaTime * playerSpeed, Space.World);
        }
        if (transform.position.z <= 4.5 && inputVertical > 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed, Space.World);
        }
        if (transform.position.z >= -4.5 && inputVertical < 0)
        {
            transform.Translate(Vector3.back * Time.deltaTime * playerSpeed, Space.World);
        }
    }

    private void UpdateCanvas()
    {
        LabelVida.text = "VIDA: " + playerVida;
        LabelTiempo.text = "TIEMPO " + playerTiempo;
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    void GoToRevive()
    {
        SceneManager.LoadScene("Revive");
    }
    void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

        if (other.CompareTag("Torret"))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            rb.AddForceAtPosition(new Vector3(0, 0, -0.1f), transform.position, ForceMode.Impulse);
        }
    }

    private void OnDestroy()
    {
        GameOver.SetActive(true);
        Time.timeScale = 0f;
    }
}
