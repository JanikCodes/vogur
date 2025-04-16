using System;
using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Reduce Float", "Base/Generic/Reduce Float", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class ReduceFloatService : IntervalServiceNode
{
    [Header("Variables")]
    [SerializeField]
    private FloatKey value;

    [SerializeField]
    private float reduceValue = 1f;

    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnTick()
    {
        float remainingValue = Mathf.Max(0, value.GetValue() - reduceValue);
        value.SetValue(remainingValue);

    }
}
