using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLab1 : MonoBehaviour
{
    
    /// Variables
    // Inputs
    [SerializeField] float entradaHorizontal = 0.0f;
    [SerializeField] float entradaVertical = 0.0f;
    [SerializeField] float entradaSalto = 0.0f;
    // Velocidades
    [SerializeField] float velocidadHorizontal = 0.0f;
    [SerializeField] float velocidadVertical = 0.0f;
    [SerializeField] float velocidadSalto = 0.0f;
    [SerializeField] float gravedad = 0.0f;
    [SerializeField] float movimientoVertical;
    // Estado
    [SerializeField] bool suelo = false;
    Rigidbody rb;


    // Awake is called before the Start()
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,0,0);
        entradaHorizontal = Input.GetAxis("Horizontal") / velocidadHorizontal;
        entradaVertical = Input.GetAxis("Vertical") / velocidadHorizontal;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            entradaSalto = 1;
        }
        else
        {
            entradaSalto = 0;
        }
    }

    private void FixedUpdate()
    {
        Jump();
        Mover();
    }

    void Mover()
    {
        rb.velocity = new Vector3(entradaHorizontal * velocidadHorizontal * 10, gravedad, entradaVertical * velocidadVertical * 10);
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && suelo)
        {
            rb.AddForce(0, velocidadSalto*1000, 0);
        }
        else
        {
            if(!suelo) movimientoVertical = gravedad;
        }

    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Floor"))
        {
            movimientoVertical = 0;
            suelo = true;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Floor"))
        {
            suelo = false;
        }
    }
}