using UnityEngine;
using System.Collections;
using Pathfinding;
using RenownedGames.AITree;

[NodeContent("Get Soldier Count", "Tasks/Base/Generic/Get Soldier Count", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class GetSoldierCountTask : TaskNode
{
    [Header("Variables")]
    [SerializeField]
    private TransformKey transformKey;

    [SerializeField]
    [NonLocal]
    private IntKey outputCount;

    // Stored required components.
    private SquadStorage squadStorage;

    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnEntry()
    {
        base.OnEntry();

        if (!squadStorage)
        {
            squadStorage = transformKey.GetValue().GetComponent<SquadStorage>();
        }
    }

    protected override State OnUpdate()
    {
        if(!squadStorage)
        {
            Debug.LogError("No SquadStorage could be found on transform. Unable to get total soldier count.");
            return State.Failure;
        }

        int soldierCount = squadStorage.GetTotalSoldierCount();
        outputCount.SetValue(soldierCount);

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}
