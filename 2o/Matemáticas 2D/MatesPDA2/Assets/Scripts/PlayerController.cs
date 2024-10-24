using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    public float playerVida = 5;

    float inputHorizontal;
    float inputVertical;

    Rigidbody rb;
    public GameObject GameOver;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        if (playerVida <= 0)
        {
            Destroy(gameObject);
        }
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
        if (transform.position.x >= -4.5 && inputVertical > 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed, Space.World);
        }
        if (transform.position.x <= 4.5 && inputVertical < 0)
        {
            transform.Translate(Vector3.back * Time.deltaTime * playerSpeed, Space.World);
        }
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
