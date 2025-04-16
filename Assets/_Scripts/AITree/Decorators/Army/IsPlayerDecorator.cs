using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent(name: "Is Player", path: "Base/Army/Is Player", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class IsPlayerDecorator : ConditionDecorator
{
    [Header("Variables")]
    [SerializeField]
    private TransformKey transform;

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
        // only the player can contain this component, therefore it's a save check to see if the transform is the player.
        PlayerNearbyEmitter playerNearbyEmitter = transform.GetValue().GetComponent<PlayerNearbyEmitter>();

        return playerNearbyEmitter != null;
    }

    protected override void OnFlowUpdate() { }
}
