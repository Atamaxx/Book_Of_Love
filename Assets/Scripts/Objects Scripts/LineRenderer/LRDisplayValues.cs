using UnityEngine;
using UnityEngine.UI;

public class LRDisplayValues : MonoBehaviour
{
    public Text numberText;
    public int number;

    [ContextMenu("DisplayNumbers")]
    void DisplayNumbers()
    {
        numberText.text = number.ToString();
    }

    // Example method to update the number value
    public void SetNumber(int newNumber)
    {
        number = newNumber;
    }
}
