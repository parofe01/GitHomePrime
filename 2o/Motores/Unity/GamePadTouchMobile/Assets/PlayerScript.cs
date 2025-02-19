using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private PlayerInput playerinput;

    // Start is called before the first frame update
    void Start()
    {
        playerinput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = playerinput.actions["Move"].ReadValue<Vector2>();
        transform.Translate(new Vector3(input.x, input.y, 0) * Time.deltaTime, Space.World);

        if (playerinput.actions["Attack"].triggered)
        {
            transform.position = Vector3.zero;
        }
    }
}
