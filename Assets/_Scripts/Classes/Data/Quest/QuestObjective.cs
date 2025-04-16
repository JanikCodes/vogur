using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="QuestObjectiveSO"/> at runtime from.
/// </summary>
[Serializable]
public class QuestObjectiveData
{
    public string Text;
    public string Type;
    public Dictionary<string, object> Properties;
}

[Serializable]
public class QuestObjective
{
    public QuestObjectiveSO QuestObjectiveBaseData;
    public Transform Self;

    [Header("Dynamid Objective Data")]
    // maybe change this simply to bool, int ? Because what else would I need usually?
    public bool BoolCheck;
    public int IntCheck;
    // add more if neccesary . . 

    public QuestObjective(Transform self, QuestObjectiveSO data)
    {
        QuestObjectiveBaseData = data;
        Self = self;
    }

    public bool IsCompleted()
    {
        return QuestObjectiveBaseData.IsComplete(this);
    }

    public void UpdateObjective()
    {
        QuestObjectiveBaseData.UpdateObjective(this);
    }

    public void Initialize()
    {
        QuestObjectiveBaseData.Initialize(this);
    }
}