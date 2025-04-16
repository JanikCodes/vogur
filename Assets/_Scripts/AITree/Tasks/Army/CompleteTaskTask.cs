using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Complete Task", "Tasks/Base/Army/Complete Task", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class CompleteTaskTask : TaskNode
{
    // Stored required components.
    private TaskQueue taskQueue;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        taskQueue = GetOwner().GetComponent<TaskQueue>();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override State OnUpdate()
    {
        if (taskQueue.GetCurrentTask() == null)
        {
            return State.Failure;
        }

        taskQueue.CompleteCurrentTask();

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}
