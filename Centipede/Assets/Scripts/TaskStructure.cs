using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "ScriptableObjects/TasksScriptableObject", order = 1)]
public class TasksScriptableObject : ScriptableObject
{
    public string taskName;
    public float duration;
    public float timeSpent;
}



