using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthSlider; // Referencia al Slider en el Canvas

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy 1")) // Asegúrate de que los enemigos tengan este tag
        {
            TakeDamage(15); // Ajusta el valor de daño que quieras
        }

        if (collision.gameObject.CompareTag("Enemy 2")) // Asegúrate de que los enemigos tengan este tag
        {
            TakeDamage(25); // Ajusta el valor de daño que quieras
        }

        if (collision.gameObject.CompareTag("Enemy 3")) // Asegúrate de que los enemigos tengan este tag
        {
            TakeDamage(50); // Ajusta el valor de daño que quieras
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    void Die()
    {
        Debug.Log("El jugador ha muerto");
        SceneManager.LoadScene("Menu");
        // Aquí puedes desactivar al jugador, mostrar un menú, etc.
    }
}
