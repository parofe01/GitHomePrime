using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Easy()
    {
        Dificulty.Instance.dificulty = false; // Establece la dificultad como f�cil
        GoToRunner();
    }

    public void Hard()
    {
        Dificulty.Instance.dificulty = true; // Establece la dificultad como dif�cil
        GoToRunner();
    }

    public void GoToRunner()
    {
        SceneManager.LoadScene("Runner");
    }
}
