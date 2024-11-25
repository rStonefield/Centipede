using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro
using System;

public class ButtonListHandler : MonoBehaviour
{
    public GameObject buttonPrefab; // The button prefab to instantiate
    public Transform buttonListContent; // The content container for the buttons
    public TMP_InputField taskInputField; // Input field to read text from
    //public TMP_InputField taskDurationField;// input time to read from 
    // This function is called when the "Send" button is clicked
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
        //string durationText = taskDurationField.text;
        // get 2 different data ! 
        string TaskInput = inputText.Substring("Task:\n Write Here:".Length).Trim();
        string durationInput = inputText.Substring("Task:\n Write Here:\n \n \n TIME:\n Write Here:".Length).Trim();
        TaskInput = TaskInput.Split('\n')[0].Trim(); // Take only the first line after trimming
        durationInput = durationInput.Substring(Math.Max(0, durationInput.Length - 4)).Trim(); // Take only the last 4 characters
        // Only create the button if the input text is not empty
        if (!string.IsNullOrEmpty(inputText))// && !string.IsNullOrEmpty(durationText))
        {
            // Instantiate a new button from the prefab
            GameObject newButton = Instantiate(buttonPrefab, buttonListContent);

            // Find the Text (TMP) component inside the new button and set its text
            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                
                buttonText.text = TaskInput + "                     "+ durationInput ;        //durationText;//inputText+"     "+durationText;
            }
            else
            {
                Debug.LogError("The button prefab is missing a TMP_Text component.");
            }

            // Optionally, clear the input field after sending the message
            taskInputField.text = "TASK: \n Write Here: \n \n \n TIME: \n Write Here:";
        }
        else
        {
            Debug.LogWarning("Input field is empty!");
        }
    }
    
}