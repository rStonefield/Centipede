using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro

public class ButtonListHandler : MonoBehaviour
{
    public GameObject buttonPrefab; // The button prefab to instantiate
    public Transform buttonListContent; // The content container for the buttons
    public TMP_InputField taskInputField; // Input field to read text from

    // This function is called when the "Send" button is clicked
    public void OnSendButtonClicked()
    {
        // Check if the buttonPrefab and buttonListContent are assigned
        if (buttonPrefab == null || buttonListContent == null)
        {
            Debug.LogError("Button prefab or list content is not assigned!");
            return;
        }

        // Get the input text from the input field
        string inputText = taskInputField.text;

        // Only create the button if the input text is not empty
        if (!string.IsNullOrEmpty(inputText))
        {
            // Instantiate a new button from the prefab
            GameObject newButton = Instantiate(buttonPrefab, buttonListContent);

            // Find the Text (TMP) component inside the new button and set its text
            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = "  "+inputText;
            }
            else
            {
                Debug.LogError("The button prefab is missing a TMP_Text component.");
            }

            // Optionally, clear the input field after sending the message
            taskInputField.text = "";
        }
        else
        {
            Debug.LogWarning("Input field is empty!");
        }
    }
}