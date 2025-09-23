using System.Collections;
using UnityEngine;

public class ObjectRemover : MonoBehaviour
{
    public CounterUI counterUI; // Asigna este en el inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Proyectil")) // Usa un tag específico para los objetos que quieres que desaparezcan
        {
            StartCoroutine(RemoveAfterDelay(other.gameObject));
        }
    }

    IEnumerator RemoveAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);

        if (counterUI != null)
        {
            counterUI.IncreaseCount();
        }

        Destroy(obj);
    }
}
