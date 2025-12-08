using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;


public class NewGameManager : MonoBehaviour
{
    public TextMeshProUGUI idText;
    public TMP_InputField winnerField;
    public TMP_InputField loserField;
    public TMP_InputField bulletsField;
    public TMP_InputField utilityField;
    public TMP_InputField DeathsField;
    public TextMeshProUGUI resultText;
    
    private GameObject accountManagerGameObject;
    private AccountManager accountManagerScript;
    
    private int userId;

    void Start()
    {
        try
        {
            accountManagerGameObject = GameObject.Find("AccountManager");
            accountManagerScript = accountManagerGameObject.GetComponent<AccountManager>();
            userId = accountManagerScript.GetId();
            
            idText.text = "Logged ID: " + userId.ToString();
        }
        catch(Exception e)
        {
            idText.text = "Logged ID: 0";
        }
    }
    public void StartNewGame()
    {
        StartCoroutine(NewGame());
    }

    IEnumerator NewGame()
    {
        WWWForm form = new WWWForm();
        form.AddField("winner_id", winnerField.text);
        form.AddField("loser_id", loserField.text);
        form.AddField("shoots_performed", bulletsField.text);
        form.AddField("utility_used", utilityField.text);
        form.AddField("deaths_amount", DeathsField.text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/game.php", form))
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