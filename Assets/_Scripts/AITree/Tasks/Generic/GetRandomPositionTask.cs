using UnityEngine;
using System.Collections;
using Pathfinding;
using RenownedGames.AITree;

[NodeContent("Get Random Position", "Tasks/Base/Generic/Get Random Position", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class GetRandomPositionTask : TaskNode
{
    [Header("Variables")]
    [SerializeField]
    [NonLocal]
    private Vector3Key outputPosition;

    [SerializeField]
    private FloatKey radius;

    // Stored required components.
    private IAstarAI ai;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        ai = GetOwner().GetComponent<IAstarAI>();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override State OnUpdate()
    {
        outputPosition.SetValue(PickRandomPoint());
        
        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    private Vector3 PickRandomPoint()
    {
        Vector3 point = Random.insideUnitSphere * radius.GetValue();

        point.y = 0;
        point += ai.position;
        return point;
    }
}
