using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    // Llama esta función desde el botón
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}