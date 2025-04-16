using Pathfinding;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Remove Random Soldiers", "Tasks/Base/Army/Remove Random Soldiers", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class RemoveRandomSoldiersTask : TaskNode
{
    [Header("Variables")]
    [SerializeField]
    private IntKey Amount;

    // Stored required components.
    private SquadStorage squadStorage;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        squadStorage = GetOwner().GetComponent<SquadStorage>();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override State OnUpdate()
    {
        for (int i = 0; i < Amount.GetValue(); i++)
        {
            bool result = squadStorage.RemoveRandomSoldier();
            if (!result)
            {
                return State.Failure;
            }
        }

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}
