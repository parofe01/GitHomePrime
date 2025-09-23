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
        if (collision.gameObject.CompareTag("Enemy 1")) // Aseg�rate de que los enemigos tengan este tag
        {
            TakeDamage(15); // Ajusta el valor de da�o que quieras
        }

        if (collision.gameObject.CompareTag("Enemy 2")) // Aseg�rate de que los enemigos tengan este tag
        {
            TakeDamage(25); // Ajusta el valor de da�o que quieras
        }

        if (collision.gameObject.CompareTag("Enemy 3")) // Aseg�rate de que los enemigos tengan este tag
        {
            TakeDamage(50); // Ajusta el valor de da�o que quieras
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
        // Aqu� puedes desactivar al jugador, mostrar un men�, etc.
    }
}
