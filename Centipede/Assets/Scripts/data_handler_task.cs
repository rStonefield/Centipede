using UnityEngine;
using TMPro;

public class Scene2Handler : MonoBehaviour
{
    public TMP_Text displayText; // Reference to the TMP_Text element in Scene 2
    public TMP_Text timeTextBox; // Reference to the timer text box (e.g., "25:00")

    public string extractedTime;
    public string extractedTimeInFormat;
    public float totalMinutes;
    public void Start()
    {
        //Debug.Log("SceneDataHolder.ButtonString: " + SceneDataHolder.ButtonString);
        // Display the stored button string in the text component
        if (displayText != null)
        {
            // Set the text to display the stored button string
            displayText.text = SceneDataHolder.ButtonString;

            // Extract and update the time in the timeTextBox
            if (timeTextBox != null)
            {
                string extractedTime = ExtractTime(SceneDataHolder.ButtonString);
                if (!string.IsNullOrEmpty(extractedTime))
                {
                    timeTextBox.text = extractedTime; // Overwrite the timer
                    
                    extractedTimeInFormat = extractedTime;
                    Debug.Log("Value" + totalMinutes);

                    string[] timeParts = extractedTimeInFormat.Split(':');
                    int hours = int.Parse(timeParts[0]);
                    int minutes = int.Parse(timeParts[1]);
                    totalMinutes = hours * 60 + minutes;
                    

                }
                else
                {
                    Debug.LogWarning("No valid time found in the stored string!");
                }
            }
            else
            {
                Debug.LogError("No TMP_Text component assigned for timeTextBox.");
            }
        }
        else
        {
            Debug.LogError("No TMP_Text component assigned in Scene2Handler.");
        }
    }

    private string ExtractTime(string input)
    {
        // Regex to match time format (e.g., "00:10")
        System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(input, @"\b\d{2}:\d{2}\b");
        if (match.Success)
        {
            return match.Value; // Return the matched time
        }
        return null;
    }
}
