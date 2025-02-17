using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SphereScript : MonoBehaviour
{

    public GameObject[] bolos;
    public Transform[] posIniBolos;
    public Transform posIniBola;

    [SerializeField] bool launched = false;
    Rigidbody rb;

    bool jumped = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        posIniBola = transform;
    }

    private void Update()
    {
        jumped = Input.GetButtonDown("Jump");
        if (Input.GetKey(KeyCode.R))
        {
            resetLevel();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(jumped && !launched)
        {
            launched = true;
            rb.AddForce(Vector3.forward * 50, ForceMode.Impulse);
        }
    }

    private void resetLevel()
    {
        for (int i = 0; i < bolos.Length; i++)
        {
            Rigidbody rbb = bolos[i].GetComponent<Rigidbody>();
            rbb.linearVelocity = Vector3.zero;
            rbb.angularVelocity = Vector3.zero;

            bolos[i].transform.position = posIniBolos[i].position;
            bolos[i].transform.rotation = posIniBolos[i].rotation;
        }
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = posIniBola.position;
        transform.rotation = posIniBola.rotation;

        launched = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Reseter"))
        {
            resetLevel();
        }
    }

}
