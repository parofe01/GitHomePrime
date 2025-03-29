using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 StartPos;
    public Vector3 EndPos;
    public float Speed;

    public bool ida;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ida == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, EndPos, Speed * Time.deltaTime);
            if (transform.position == EndPos)
            {
                ida = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, StartPos, Speed * Time.deltaTime);
            if (transform.position == StartPos)
            {
                ida = true;
            }
        }
    }
}
