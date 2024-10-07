using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Include TextMesh Pro namespace
using System.Text.RegularExpressions;

public class Tasks_Manager : MonoBehaviour
{
    public TMP_InputField taskNameInput; // TextMesh Pro Input Field for task name
    //public TMP_InputField durationInput;  // TextMesh Pro Input Field for duration
    //public Transform taskListParent;      // A UI parent to hold task entries
    //public GameObject taskUIPrefab;       // Prefab for displaying a task

    // Start is called before the first frame update
    void Start()
    {
        // Add listener to the input field to check input
        taskNameInput.onEndEdit.AddListener(ValidateInput);
        
    }

    // This method is called when the user finishes editing the input field
    public void ValidateInput(string input)
    {
        // Use a regular expression to check if the input contains only letters and numbers
        if (!Regex.IsMatch(input, "^[a-zA-Z0-9]*$"))
        {
            // If invalid, replace the text with the valid input
            taskNameInput.text = Regex.Replace(input, "[^a-zA-Z0-9]", ""); // Remove invalid characters
            // Move the caret to the end of the input field
            taskNameInput.MoveTextEnd(false);
        }
    }

 

    // Update is called once per frame
    void Update()
    {
        // Typically, Update is not needed for this functionality
    }
}