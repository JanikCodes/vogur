using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using RenownedGames.Apex;
using UnityEngine;

[NodeContent(name: "Is Army Spawn Type", path: "Base/Faction/Is Army Spawn Type", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class IsArmySpawnTypeDecorator : ConditionDecorator
{
    [Header("Variables")]
    [SerializeField]
    private FactionArmySpawnTypeKey firstInput;

    [SerializeField]
    private FactionArmySpawnTypeKey secondInput;

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
        if (firstInput.GetValue().Equals(secondInput.GetValue()))
        {
            return true;
        }

        return false;
    }

    protected override void OnFlowUpdate() { }
}
