using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent(name: "Has Reached Max Army Amount", path: "Base/Faction/Has Reached Max Army Amount", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class HasReachedMaxArmyAmountDecorator : ConditionDecorator
{
    // Stored required components.
    private ObjectStorage objectStorage;
    private FactionArmyReference factionArmyReference;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        objectStorage = GetOwner().GetComponent<ObjectStorage>();
        factionArmyReference = GetOwner().GetComponent<FactionArmyReference>();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override bool CalculateResult()
    {
        return factionArmyReference.ReferencedArmies.Count >= objectStorage.GetObject<FactionSO>().MaxArmyCountOnOverworld;
    }

    protected override void OnFlowUpdate() { }
}
