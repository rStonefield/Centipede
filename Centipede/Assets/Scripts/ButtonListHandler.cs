using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro
using UnityEngine.SceneManagement; // For scene management
using System;

public class ButtonListHandler : MonoBehaviour
{
    public GameObject buttonPrefab; // The button prefab to instantiate
    public Transform buttonListContent; // The content container for the buttons
    public TMP_InputField taskInputField; // Input field to read text from

    public void OnSendButtonClickedTask()
    {
        // Check if the buttonPrefab and buttonListContent are assigned
        if (buttonPrefab == null || buttonListContent == null)
        {
            Debug.LogError("Button prefab or list content is not assigned!");
            return;
        }

        // Get the input text from the input field
        string inputText = taskInputField.text;

        // Parse the task and duration input
        string TaskInput = inputText.Substring("Task:\n Write Here:".Length).Trim();
        string durationInput = inputText.Substring("Task:\n Write Here:\n \n \n TIME:\n Write in format\n(e.g 01:32 ~ 1 hour and 32 mins) :  ".Length).Trim();
        TaskInput = TaskInput.Split('\n')[0].Trim(); // Take only the first line after trimming
        durationInput = durationInput.Substring(Math.Max(0, durationInput.Length - 5)).Trim(); // Take only the last 4 characters

        // Only create the button if the input text is not empty
        if (!string.IsNullOrEmpty(inputText))
        {
            // Instantiate a new button from the prefab
            GameObject newButton = Instantiate(buttonPrefab, buttonListContent);

            // Find the Text (TMP) component inside the new button and set its text
            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = "  " + TaskInput + "                     " + durationInput;
            }
            else
            {
                Debug.LogError("The button prefab is missing a TMP_Text component.");
            }

            // Add a click listener to the new button
            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() =>
                {
                    OnGeneratedButtonClick(buttonText.text);
                });
            }

            // Optionally, clear the input field after sending the message
            taskInputField.text = "TASK: \n Write Here: \n \n \n TIME:\n Write in format\n(e.g 01:32 ~ 1 hour and 32 mins) : ";
        }
        else
        {
            Debug.LogWarning("Input field is empty!");
        }
    }

    private void OnGeneratedButtonClick(string buttonText)
    {
        // Store the button text in the static class
        SceneDataHolder.ButtonString = buttonText;

        // Load the target scene
        SceneManager.LoadScene("Home"); // Replace with the name of your destination scene
    }
}
