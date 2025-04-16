using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using RenownedGames.AITree;
using RenownedGames.Apex;
using UnityEngine;

[NodeContent("Move To Position", "Tasks/Base/Army/Move To Position", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class MoveToPositionTask : TaskNode
{
    [Title("Blackboard")]
    [SerializeField] 
    private Vector3Key key;

    [Title("Node")]
    [SerializeField]
    private float acceptableRadius = 0.1f;

    [SerializeField]
    private bool includeAIRadius = true;

    private IAstarAI ai;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        ai = GetOwner().GetComponent<IAstarAI>();
    }

    protected override State OnUpdate()
    {
        if (!ai.pathPending || ai.hasPath || ai.reachedEndOfPath)
        {
            float tolerance = acceptableRadius;

            if (includeAIRadius)
            {
                tolerance += ai.radius;
            }

            if (key.TryGetPosition(Space.World, out Vector3 value))
            {
                ai.destination = value;
            }

            if (ai.remainingDistance <= tolerance)
            {
                return State.Success;
            }
        }

        return State.Running;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}
