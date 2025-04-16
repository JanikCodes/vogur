using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(IAstarAI))]
public class ArmySpeed : MonoBehaviour
{
    [SerializeField] private float BaseSpeed;

    private IAstarAI ai;

    private void Awake()
    {
        ai = GetComponent<IAstarAI>();

        HandleSpeedChange();
    }

    private void OnEnable()
    {
        // subscribe to each ISpeedPenalty component
        ISpeedPenalty[] penalties = transform.parent.GetComponentsInChildren<ISpeedPenalty>();
        foreach (ISpeedPenalty penalty in penalties)
        {
            penalty.SpeedPenaltyChanged += HandleSpeedChange;
        }
    }

    private void OnDisable()
    {
        // subscribe to each ISpeedPenalty component
        ISpeedPenalty[] penalties = transform.parent.GetComponentsInChildren<ISpeedPenalty>();
        foreach (ISpeedPenalty penalty in penalties)
        {
            penalty.SpeedPenaltyChanged -= HandleSpeedChange;
        }
    }

    /// <summary>
    /// Is called whenever something changes that could affect speed.
    /// </summary>
    private void HandleSpeedChange()
    {
        float totalPenalty = 0f;
        ISpeedPenalty[] penalties = GetComponentsInChildren<ISpeedPenalty>();

        foreach (ISpeedPenalty penalty in penalties)
        {
            totalPenalty += penalty.GetSpeedPenalty();
        }

        float calculatedSpeed = BaseSpeed - totalPenalty;
        ai.maxSpeed = Mathf.Max(1, calculatedSpeed);
    }
}
