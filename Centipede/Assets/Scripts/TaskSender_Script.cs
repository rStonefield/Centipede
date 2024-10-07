using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Include TextMesh Pro namespace
using System.Text.RegularExpressions;

public class Tasks_Sender : MonoBehaviour
{
    public TMP_InputField taskNameInput; // TextMesh Pro Input Field for task name
    public TMP_InputField durationInput;  // Text Mesh Pro Input Field for duration
    public Transform taskListParent;      // A UI parent to hold task entries
    public GameObject taskUIPrefab;       // Prefab for displaying a task

    // Start is called before the first frame update
    void Start()
    {
        // You may not need to add any listeners here if validation is handled elsewhere
    }

    // Method to add task and display it
    public void AddTask()
    {
        // Get the values from the input fields
        string taskName = taskNameInput.text;
        string duration = durationInput.text;

        // Check if inputs are not empty
        if (!string.IsNullOrEmpty(taskName) && !string.IsNullOrEmpty(duration))
        {
            // Instantiate the task UI prefab
            GameObject newTaskUI = Instantiate(taskUIPrefab, taskListParent);

            // Assuming the prefab has a TextMeshProUGUI component for task name and duration
            TMP_Text[] texts = newTaskUI.GetComponentsInChildren<TMP_Text>();
            if (texts.Length >= 2)
            {
                texts[0].text = taskName;      // Set task name
                texts[1].text = duration;       // Set duration
            }

            // Clear the input fields after adding the task
            taskNameInput.text = "";
            durationInput.text = "";
        }
        else
        {
            Debug.LogWarning("Task name or duration is empty.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Typically, Update is not needed for this functionality
    }
}