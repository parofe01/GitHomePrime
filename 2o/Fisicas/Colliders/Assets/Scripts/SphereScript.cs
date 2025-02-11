using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SphereScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Cube")
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        if (collision.gameObject.name == "Cube (1)")
        {
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 25, ForceMode.Impulse);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Cube")
        {
            collision.gameObject.transform.rotation = Quaternion.Slerp(collision.gameObject.transform.rotation, Quaternion.Euler(0, 0, 179), Time.deltaTime * 0.1f); ;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Cube")
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
}
