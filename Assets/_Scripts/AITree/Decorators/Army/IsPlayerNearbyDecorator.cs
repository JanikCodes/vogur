using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent(name: "Is Player Nearby", path: "Base/Army/Is Player Nearby", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class IsPlayerNearbyDecorator : ConditionDecorator
{
    // Stored required components.
    private PlayerNearby playerNearby;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        playerNearby = GetOwner().GetComponent<PlayerNearby>();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override bool CalculateResult()
    {
        return playerNearby.IsPlayerNearby;
    }

    protected override void OnFlowUpdate() { }
}
