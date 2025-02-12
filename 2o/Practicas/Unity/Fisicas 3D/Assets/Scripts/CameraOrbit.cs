using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public float rotationSpeed = 5f; // Velocidad de rotaci�n
    public float minVerticalAngle = -30f; // �ngulo m�nimo vertical
    public float maxVerticalAngle = 60f; // �ngulo m�ximo vertical
    public float distance = 5f; // Distancia de la c�mara al objeto

    private float currentX = 0f; // Rotaci�n acumulada en el eje vertical
    private float currentY = 0f; // Rotaci�n acumulada en el eje horizontal

    public float mouseSensitivity = 2f;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Capturar entrada del rat�n
            float inputCameraX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float inputCameraY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Actualizar las rotaciones acumuladas
            currentY += inputCameraX; // Rotaci�n horizontal
            currentX -= inputCameraY; // Rotaci�n vertical
            currentX = Mathf.Clamp(currentX, minVerticalAngle, maxVerticalAngle); // Limitar el �ngulo vertical

            // Aplicar las rotaciones
            transform.rotation = Quaternion.Euler(currentX, currentY, 0f);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
