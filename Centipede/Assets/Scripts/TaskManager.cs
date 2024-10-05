using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    public TMP_InputField taskNameInput;  // TextMesh Pro Input Field for task name
    public TMP_InputField durationInput;  // TextMesh Pro Input Field for duration
    public Transform taskListParent;      // A UI parent to hold task entries
    public GameObject taskUIPrefab;       // Prefab for displaying a task

    // The task template (set in the Inspector) which serves as a blueprint for creating tasks
    public TasksScriptableObject taskTemplate;

    // A list to hold the created tasks
    private List<TasksScriptableObject> taskList = new List<TasksScriptableObject>();

    public void AddTask()
    {
        // Create a new instance of the task based on the ScriptableObject template
        TasksScriptableObject newTask = ScriptableObject.Instantiate(taskTemplate);

        // Set task properties from user input
        newTask.taskName = taskNameInput.text;

        if (float.TryParse(durationInput.text, out float duration))
        {
            newTask.duration = duration;
        }
        else
        {
            Debug.LogError("Invalid input for duration");
            return; // Exit if the input is invalid
        }

        newTask.timeSpent = 0;

        // Add the new task to the list
        taskList.Add(newTask);

        // Display the task in the UI
        DisplayTask(newTask);
    }

    // Display the task in the UI
    void DisplayTask(TasksScriptableObject task)
    {
        // Instantiate the UI prefab for the task
        GameObject taskUI = Instantiate(taskUIPrefab, taskListParent);

        // Update the text of the UI element
        taskUI.GetComponentInChildren<TextMeshProUGUI>().text = $"{task.taskName} - {task.duration} mins";

        // Optional: Start a timer for each task (if needed)
    }
}
