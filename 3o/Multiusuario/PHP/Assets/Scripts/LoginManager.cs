using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public TextMeshProUGUI resultText;

    public AccountManager accountManager;
    
    public void StartLogin()
    {
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                resultText.text = "Error: " + www.error;
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Respuesta del servidor: " + responseText);

                // Convertir el JSON en un objeto C#
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(responseText);

                if (response != null && response.status == "success")
                {
                    // AQUÍ TIENES EL ID
                    int userId = response.id;
                    Debug.Log("ID del usuario: " + userId);

                    resultText.text = "Login successful!";
                
                    // Guarda lo que necesites
                    accountManager.SaveAccount(userId, usernameField.text);

                    SceneManager.LoadScene("Level1");
                }
                else
                {
                    resultText.text = "Login failed!";
                }
            }
        }
    }

}