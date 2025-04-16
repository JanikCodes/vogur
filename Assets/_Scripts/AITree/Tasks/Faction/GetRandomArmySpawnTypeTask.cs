using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using RenownedGames.Apex;
using UnityEngine;

[NodeContent("Get Random Army Spawn Type", "Tasks/Base/Faction/Get Random Army Spawn Type", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class GetRandomArmySpawnTypeTask : TaskNode
{
    [Header("Variables")]
    [SerializeField]
    [NonLocal]
    private FactionArmySpawnTypeKey outputArmySpawnType;

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

        if (!factionData || factionData.FactionArmySpawnType.Length == 0)
        {
            return State.Failure;
        }

        outputArmySpawnType.SetValue(Util.GetRandomValue<FactionArmySpawnType>(factionData.FactionArmySpawnType));

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}