using UnityEngine;

public class CannonController : MonoBehaviour
{
    public Transform firePoint;
    public GameObject ballPrefab;
    public float launchForce = 10f;
    
    // lineRenderer
    public LineRenderer lineRenderer; // componente
    public int linePoints = 175; // numero de puntos de la linea a dibujar
    public float timeInterval = 0.01f; // intervalo entre puntos
    
    // mover angulo cañon
    public Camera cam;
    public float rotationSpeed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        if (lineRenderer != null)
        {
            if (Input.GetKey(KeyCode.M))
            {
                DrawTrajectory();
                lineRenderer.enabled = true;
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject ball = Instantiate(ballPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.transform.up * launchForce;
    }

    // LineRenderer
    private void DrawTrajectory()
    {
        lineRenderer.positionCount = linePoints; // numero de puntos que tiene la linea
        
        Vector3 origin = firePoint.position; // origen de la linea
        Vector3 startVelocity = firePoint.transform.up * launchForce;
        float time = 0;
        
        for (int i = 0; i < linePoints; i++)
        {
            Vector3 pos = firePoint.position;
            
            pos.x = startVelocity.x * time;
            pos.y = (startVelocity.y * time) + (Physics.gravity.y/2) * Mathf.Pow(time,2);
            // pos.z = startVelocity.z * time;
            pos.z = 0;
            
            // indicar la posicion calculada a lineRenderer - > SetPosition(punto, posicion)
            
            lineRenderer.SetPosition(i, origin + pos);
            time += timeInterval;
        }
    }

    private void Aim()
    {
        var mousePos = cam.ScreenToViewportPoint(Input.mousePosition);
        mousePos.z = 0;
        
        gameObject.transform.up = Vector3.MoveTowards(transform.up, mousePos, rotationSpeed * Time.deltaTime);
    }
}
