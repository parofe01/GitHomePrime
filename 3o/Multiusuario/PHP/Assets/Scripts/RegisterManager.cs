using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


public class RegisterManager : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public TextMeshProUGUI resultText;

    public void StartRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/register.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                resultText.text = "Error: " + www.error;
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log(responseText);
                if (responseText.Contains("success"))
                {
                    resultText.text = "Register successful!";
                }
                //==Este else if es añadido para ver si esta duplicado===
                else if (responseText.Contains("duplicate"))
                {
                    resultText.text = "Register failed! Duplicated value";
                }
                //===================================================
                else
                {
                    resultText.text = "Register failed!";
                }
            }
        }
    }
}