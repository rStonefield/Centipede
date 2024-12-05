using TMPro;
using UnityEngine;

public class TextBoxUpdater : MonoBehaviour
{
    public TMP_Text generatedTextBox;  // The text box containing "gun 00:10"
    public TMP_Text timeTextBox;  // The text box containing "25:00"

    public void UpdateTimeText()
    {
        if (generatedTextBox == null || timeTextBox == null)
        {
            Debug.LogError("Text boxes are not assigned!");
            return;
        }

        string generatedText = generatedTextBox.text;  // Get the text from the generated text box
        Debug.Log("Generated Text: " + generatedText);

        // Extract the time value from the generated text
        string extractedTime = ExtractTime(generatedText);
        Debug.Log("Extracted Time: " + extractedTime);

        if (!string.IsNullOrEmpty(extractedTime))
        {
            // Overwrite the time text box with the extracted time
            timeTextBox.text = extractedTime;
            Debug.Log("Time text box updated to: " + extractedTime);
        }
        else
        {
            Debug.LogWarning("No valid time found in the generated text!");
        }
    }

    private string ExtractTime(string input)
    {
        // Check for a valid time in the format "00:10"
        System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(input, @"\b\d{2}:\d{2}\b");
        if (match.Success)
        {
            return match.Value;  // Return the matched time value
        }
        return null;  // No valid time found
    }
}
