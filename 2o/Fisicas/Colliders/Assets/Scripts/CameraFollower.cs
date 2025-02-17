using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public float max;
    public GameObject bola;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bola.transform.position.z < max)
        {
            transform.position = bola.transform.position;
        }
    }
}
