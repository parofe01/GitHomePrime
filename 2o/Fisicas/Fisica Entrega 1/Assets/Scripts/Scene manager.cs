using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    // Llama esta funci�n desde el bot�n
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}