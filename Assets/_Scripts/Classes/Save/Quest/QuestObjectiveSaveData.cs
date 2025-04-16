using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DataContainer class dependency for <see cref="QuestStepSaveData"/> saving dynamic data
/// </summary>
[Serializable]
public class QuestObjectiveSaveData
{
    // public string Id;

    [Header("Dynamid Objective Data")]
    public bool VisitedTarget;
    public int CurrencyRemaining;

    public QuestObjectiveSaveData()
    {
        // empty constructor for serialization
    }

    public QuestObjectiveSaveData(QuestObjective questObjective)
    {
        VisitedTarget = questObjective.BoolCheck;
        CurrencyRemaining = questObjective.IntCheck;
    }
}