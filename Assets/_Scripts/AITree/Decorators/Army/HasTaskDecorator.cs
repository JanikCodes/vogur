using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent(name: "Has Task", path: "Base/Army/Has Task", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class HasTaskDecorator : ConditionDecorator
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

    protected override bool CalculateResult()
    {
        return taskQueue.Tasks.Count > 0;
    }

    protected override void OnFlowUpdate() { }
}
