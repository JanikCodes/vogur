using System;
using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Get Transform Position", "Base/Army/Get Transform Position", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class GetTransformPositionService : IntervalServiceNode
{
    [Header("Variables")]
    [SerializeField]
    [NonLocal]
    private TransformKey transform;

    [SerializeField]
    [NonLocal]
    private Vector3Key outputPosition;

    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnTick()
    {
        if (transform.GetValue() == null) { return; }

        Vector3 targetPos = transform.GetValue().position;
        outputPosition.SetValue(targetPos);
    }
}
