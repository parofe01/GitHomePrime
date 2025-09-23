using UnityEngine;

public class DestroyOnGround : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            Destroy(gameObject);
        }
    }
}