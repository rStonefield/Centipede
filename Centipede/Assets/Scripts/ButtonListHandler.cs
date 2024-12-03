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

    private const string TaskListKey = "SavedTaskList"; // Key for saving task list
    private const float doubleClickTime = 0.3f; // Time interval to detect a double-click

    private int clickCount = 0; // Track the number of clicks
    private float lastClickTime = 0f; // Time of the last click

    public  GameObject lastHighlightedButton; // Reference to the last highlighted button
    private Color originalColor; // Store the original button color
    public void UpdateTaskListAfterDeletion()
    {
    // Load the current task list
    List<string> tasks = LoadTaskList();

    // Re-save the updated task list
    SaveTaskList(tasks);
    }


    void Start()
    {
        // Load saved tasks when the scene starts
        if (PlayerPrefs.HasKey(TaskListKey))
        {
            string savedData = PlayerPrefs.GetString(TaskListKey);
            List<string> tasks = DeserializeTaskList(savedData);

            // Recreate buttons for all saved tasks
            foreach (string task in tasks)
            {
                string[] parts = task.Split('|');
                if (parts.Length == 2)
                {
                    string taskText = parts[0];
                    string durationText = parts[1];
                    CreateButton(taskText, durationText);
                }
            }
        }
    }

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
        if (!string.IsNullOrEmpty(TaskInput) && !string.IsNullOrEmpty(durationInput))
        {
            // Save the task and duration in the list
            List<string> tasks = LoadTaskList();
            tasks.Add($"{TaskInput}|{durationInput}");
            SaveTaskList(tasks);

            // Create the button
            CreateButton(TaskInput, durationInput);

            // Optionally, clear the input field after sending the message
            taskInputField.text = "TASK:\n Write Here: \n \n \n TIME:\n Write in format\n(e.g 01:32 ~ 1 hour and 32 mins) : ";
        }
        else
        {
            Debug.LogWarning("Input field is empty!");
        }
    }

    private void CreateButton(string taskText, string durationText)
    {
        // Instantiate a new button from the prefab
        GameObject newButton = Instantiate(buttonPrefab, buttonListContent);

        // Find the Text (TMP) component inside the new button and set its text
        TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            buttonText.text = "  " + taskText + "                     " + durationText;
        }
        else
        {
            Debug.LogError("The button prefab is missing a TMP_Text component.");
        }

        // Add listeners to the new button
        Button buttonComponent = newButton.GetComponent<Button>();
        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(() =>
            {
                HandleSingleClick(newButton); // Highlight the button
                HandleDoubleClick(() => OnGeneratedButtonClick(buttonText.text));
            });
        }
    }

    private void HandleSingleClick(GameObject button)
    {
        // Reset the last highlighted button, if any
        if (lastHighlightedButton != null && lastHighlightedButton != button)
        {
            Image lastButtonImage = lastHighlightedButton.GetComponent<Image>();
            if (lastButtonImage != null)
            {
                lastButtonImage.color = originalColor; // Restore the original color
            }
        }

        // Highlight the current button
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            if (lastHighlightedButton == null || lastHighlightedButton != button)
            {
                originalColor = buttonImage.color; // Store the original color
            }
            buttonImage.color = Color.yellow; // Set highlight color
        }

        // Update the last highlighted button
        lastHighlightedButton = button;
    }

    private void HandleDoubleClick(Action action)
    {
        float currentTime = Time.time;

        if (currentTime - lastClickTime <= doubleClickTime)
        {
            clickCount++;
            if (clickCount == 2)
            {
                clickCount = 0; // Reset click count
                action(); // Execute the action
            }
        }
        else
        {
            clickCount = 1; // Reset to single click
        }

        lastClickTime = currentTime; // Update the last click time
    }

    private void OnGeneratedButtonClick(string buttonText)
    {
        // Store the button text in the static class
        SceneDataHolder.ButtonString = buttonText;

        // Load the target scene
        SceneManager.LoadScene("Home"); // Replace with the name of your destination scene
    }

    private List<string> LoadTaskList()
    {
        string savedData = PlayerPrefs.GetString(TaskListKey, "");
        return DeserializeTaskList(savedData);
    }

    private void SaveTaskList(List<string> tasks)
    {
        string serializedData = SerializeTaskList(tasks);
        PlayerPrefs.SetString(TaskListKey, serializedData);
        PlayerPrefs.Save();
    }

    private string SerializeTaskList(List<string> tasks)
    {
        return string.Join(";", tasks);
    }

    private List<string> DeserializeTaskList(string data)
    {
        if (string.IsNullOrEmpty(data)) return new List<string>();
        return new List<string>(data.Split(';'));
    }
}
