using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;

    float inputHorizontal;
    float inputVertical;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    void Inputs()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        // Movimiento a derecha
        
            transform.Translate(Vector3.right * Time.deltaTime * playerSpeed * inputHorizontal, Space.World);
            transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed * inputVertical, Space.World);
    }
}
