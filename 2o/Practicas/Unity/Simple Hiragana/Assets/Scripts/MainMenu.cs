using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void _fPlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    
    public void _fOptions()
    {
        SceneManager.LoadScene("OptionsMenu");   
    }
    
    public void _fQuitGame()
    {
        Application.Quit();
    }
}
