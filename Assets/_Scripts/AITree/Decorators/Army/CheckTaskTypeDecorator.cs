using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent(name: "Check Task Type", path: "Base/Army/Check Task Type", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class CheckTaskTypeDecorator : ConditionDecorator
{
    [Header("Variables")]
    [SerializeField]
    private TaskType taskType;

    [SerializeField]
    private TaskKey task;

    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override bool CalculateResult()
    {
        return taskType.Equals(task.GetValue().TaskType);
    }

    protected override void OnFlowUpdate() { }
}
