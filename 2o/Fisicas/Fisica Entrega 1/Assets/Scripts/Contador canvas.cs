using UnityEngine;
using TMPro;

public class CounterUI : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    private int count = 0;

    void Start()
    {
        UpdateText();
    }

    public void IncreaseCount()
    {
        count++;
        UpdateText();
    }

    private void UpdateText()
    {
        counterText.text = "Queijos: " + count.ToString();
    }
}
