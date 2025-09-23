using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float rotationSpeed = 10f;

    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && player != null)
        {
            // Calcular dirección hacia el jugador
            Vector3 direction = (player.position - transform.position);
            direction.y = 0f; // Para que no gire hacia arriba o abajo
            direction = direction.normalized;

            // Mover al enemigo
            transform.position += direction * speed * Time.deltaTime;

            // Rotar en la dirección del movimiento
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
