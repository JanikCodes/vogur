using System;
using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Get Transform Distance", "Base/Army/Get Transform Distance", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class GetTransformDistanceService : IntervalServiceNode
{
    [Header("Variables")]
    [SerializeField]
    private TransformKey firstTransform;

    [SerializeField]
    private TransformKey secondTransform;

    [SerializeField]
    [NonLocal]
    private FloatKey outputDistance;

    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnTick()
    {
        if (firstTransform.GetValue() == null || secondTransform.GetValue() == null) { return; }

        Vector3 firstPos = firstTransform.GetValue().position;
        Vector3 secondPos = secondTransform.GetValue().position;

        // calculate the squared distance between two positions
        float squaredDistance = (firstPos - secondPos).sqrMagnitude;

        outputDistance.SetValue(squaredDistance);
    }
}
