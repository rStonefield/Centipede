using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scene2Handler : MonoBehaviour
{
    public TMP_Text displayText; // Reference to the TMP_Text element in Scene 2

    private void Start()
    {
        // Display the stored button string in the text component
        if (displayText != null)
        {
            displayText.text = SceneDataHolder.ButtonString;
        }
        else
        {
            Debug.LogError("No TMP_Text component assigned in Scene2Handler.");
        }
    }
}
