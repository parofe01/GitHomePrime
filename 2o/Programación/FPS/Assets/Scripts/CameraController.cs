using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Configuración de Sensibilidad")]
    public float mouseSensitivity = 2f;

    [Header("Referencias")]
    public Transform playerBody; // Referencia al cuerpo del jugador

    private float rotationX = 0f;

    void Start()
    {
        // Bloquear el cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Capturar entrada del ratón
        float inputCameraX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputCameraY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Actualizar rotaciones acumulativas
        rotationX -= inputCameraY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Limitar el movimiento vertical de la cámara

        // Rotar la cámara en el eje X (vertical)
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // Rotar el cuerpo del jugador en el eje Y (horizontal)
        playerBody.Rotate(Vector3.up * inputCameraX);
    }
}
