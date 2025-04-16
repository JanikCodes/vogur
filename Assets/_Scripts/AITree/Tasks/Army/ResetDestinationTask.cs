using Pathfinding;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Reset Destination", "Tasks/Base/Army/Reset Destination", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class ResetDestinationTask : TaskNode
{
    [Header("Variables")]
    [SerializeField]
    [NonLocal]
    private TransformKey transform;


    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override State OnUpdate()
    {
        IAstarAI ai = transform.GetValue().GetComponent<IAstarAI>();
        if (ai == null)
        {
            Debug.LogError("Transform does not have an instance of IStarAI!");
            return State.Failure;
        }

        // set destination to themself
        ai.destination = transform.GetValue().position;
        ai.SearchPath();

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}
