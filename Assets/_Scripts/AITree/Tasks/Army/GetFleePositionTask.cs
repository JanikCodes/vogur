using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Get Flee Position", "Tasks/Base/Army/Get Flee Position", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class GetFleePositionTask : TaskNode
{
    [Header("Variables")]
    [SerializeField]
    [NonLocal]
    private TransformKey targetToFleeFrom;

    [SerializeField]
    private FloatKey length;

    [SerializeField]
    [NonLocal]
    private Vector3Key outputPosition;

    // Stored required components.
    private Transform transform;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        transform = GetOwner().transform;
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override State OnUpdate()
    {
        outputPosition.SetValue(CalculateFleePosition());

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    private Vector3 CalculateFleePosition()
    {
        Vector3 playerPos = transform.position;
        Vector3 targetPos = targetToFleeFrom.GetValue().position;

        Vector3 fleePosition = -(targetPos - playerPos);

        Vector3 normalizedFleePosition = Vector3.Normalize(fleePosition);
        return normalizedFleePosition * length.GetValue();
    }
}
