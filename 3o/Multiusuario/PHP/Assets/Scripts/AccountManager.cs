using UnityEngine;

public class AccountManager : MonoBehaviour
{
    private int id;
    private string username;
    
    void Start()
    {
        if (gameObject)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SaveAccount(int id, string user)
    {
        this.id = id;
        username = user;
    }

    public int GetId()
    {
        return id;
    }

    public string GetUsername()
    {
        return username;
    }
    
    public void LogOut()
    {
        username = string.Empty;
    }
}
