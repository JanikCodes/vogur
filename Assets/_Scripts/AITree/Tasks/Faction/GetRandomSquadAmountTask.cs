using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Get Random Squad Amount", "Tasks/Base/Faction/Get Random Squad Amount", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class GetRandomSquadAmountTask : TaskNode
{
    [Header("Variables")]
    [SerializeField]
    [NonLocal]
    private IntKey outputSquadAmount;

    // Stored required components.
    private ObjectStorage objectStorage;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        objectStorage = GetOwner().GetComponent<ObjectStorage>();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override State OnUpdate()
    {
        FactionSO factionData = objectStorage.GetObject<FactionSO>();

        if (!factionData)
        {
            return State.Failure;
        }

        int squadAmount = Util.GetRandomValue(factionData.MinSquadAmount, factionData.MaxSquadAmount);
        if (squadAmount < 0)
        {
            return State.Failure;
        }

        outputSquadAmount.SetValue(squadAmount);

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}
