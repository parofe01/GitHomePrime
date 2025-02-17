using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ejercicios1_8 : MonoBehaviour
{

    public enum Exercice { Cero, Uno, Dos, Tres, Cuatro, Cinco, Seis, Siete, Ocho }
    public Exercice ex;

    // Inputs
    float _IAxisH;
    float _IAxisV;
    bool _IJump;

    // Components
    Rigidbody _cRigidbody;

    // GameObjects
    public GameObject _goPlayer;

    // Variables
    public float _vForce;
    public float _vVForce;
    public float _vExploRadius;
    public bool _vGrounded;

    // Start is called before the first frame update
    void Start()
    {
        _goPlayer = GameObject.Find("Player");
        _cRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    void FixedUpdate()
    {
        PhisicsMachine();
    }

    void Inputs()
    {
        _IAxisH = Input.GetAxis("Horizontal");
        _IAxisV = Input.GetAxis("Vertical");
        _IJump = Input.GetButtonDown("Jump");
    }
    void PhisicsMachine()
    {
        switch (ex)
        {
            case Exercice.Cero: break;
            case Exercice.Uno: Uno(); break;
            case Exercice.Dos: Dos(); break;
            case Exercice.Tres: Tres(); break;
            case Exercice.Cuatro: Cuatro(); break;
            case Exercice.Cinco: Cinco(); break;
            case Exercice.Seis: Seis(); break;
            case Exercice.Siete: Siete(); break;
            case Exercice.Ocho: Ocho(); break;

        }
    }

    void Uno()
    {
        if(_IAxisV > 0)
        {
            _cRigidbody.AddForce(Vector3.forward * _vForce, ForceMode.Force);
        }
    }

    void Dos()
    {
        if (_IAxisV != 0)
        {
            _cRigidbody.AddForce(Vector3.forward * _IAxisV * _vForce, ForceMode.Force);
        }
        if (_IAxisH != 0)
        {
            _cRigidbody.AddForce(Vector3.right * _IAxisH * _vForce, ForceMode.Force);
        }
    }
    void Tres()
    {
        if ( _IAxisV > 0)
        {
            _cRigidbody.AddForce(Vector3.forward * _vForce, ForceMode.Force);
        }
        if (_IAxisV < 0)
        {
            _cRigidbody.AddForce(Vector3.back * _vForce/2, ForceMode.Force);
        }
    }
    void Cuatro()
    {
        if (_IAxisV != 0)
        {
            _cRigidbody.linearVelocity = Vector3.forward * _IAxisV * _vForce;
        }
        if (_IAxisH != 0)
        {
            _cRigidbody.linearVelocity = Vector3.right * _IAxisH * _vForce;
        }
    }
    void Cinco()
    {
        if (_IJump && _vGrounded)
        {
            _cRigidbody.AddForce(Vector3.up * _vForce, ForceMode.Impulse);
        }
    }
    void Seis()
    {

        _cRigidbody.AddForce((_goPlayer.transform.position - transform.position) * _vForce, ForceMode.Acceleration);
        
    }
    void Siete()
    {
        if (_IAxisH != 0)
        {
            _cRigidbody.AddTorque(new Vector3(0, _vForce * _IAxisH, 0), ForceMode.Force);
        }
    }

    void Ocho()
    {
        if (_IJump)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _vExploRadius); 

            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(_vForce, transform.position, _vExploRadius, _vVForce, ForceMode.Impulse);
                }
            }
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _vGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            _vGrounded = false;
        }
    }
}
