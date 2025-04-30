using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField]private TestScriptableObject testScriptableObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(testScriptableObject.myString);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
