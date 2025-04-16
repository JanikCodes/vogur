using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Get Current Task", "Base/Army/Get Current Task", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class GetCurrentTaskService : IntervalServiceNode
{
    [Header("Variables")]
    [SerializeField]
    [NonLocal]
    private TaskKey outputTask;

    // Stored required components.
    private TaskQueue taskQueue;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        taskQueue = GetOwner().GetComponent<TaskQueue>();
    }

    protected override void OnTick()
    {
        if (taskQueue.Tasks.Count == 0) { return; }

        Task task = taskQueue.GetCurrentTask();
        outputTask.SetValue(task);
    }
}
