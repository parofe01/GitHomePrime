using UnityEngine;
using TMPro;

public class PlayerItemCollector : MonoBehaviour
{
    public TextMeshProUGUI contadorTexto;
    private int contador = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Recolectable"))
        {
            contador++;
            ActualizarUI();
            Destroy(other.gameObject);
        }
    }

    private void ActualizarUI()
    {
        contadorTexto.text = "Quesos: " + contador;
    }
}