using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="QuestStepSO"/> at runtime from.
/// </summary>
[Serializable]
public class QuestStepData : BaseData
{
    public QuestObjectiveData[] Objectives;
}

[Serializable]
public class QuestStep
{
    public QuestStepSO QuestStepBaseData;
    public List<QuestObjective> Objectives = new();

    public QuestStep(Transform self, QuestStepSO data)
    {
        QuestStepBaseData = data;
        
        foreach (QuestObjectiveSO questObjectiveSO in data.Objectives)
        {
            QuestObjective questObjective = new(self, questObjectiveSO);
            Objectives.Add(questObjective);
        }
    }

    public void InitializeObjectives()
    {
        foreach (QuestObjective questObjective in Objectives)
        {
            questObjective.Initialize();
        }
    }

    public void UpdateObjectives()
    {
        foreach (QuestObjective questObjective in Objectives)
        {
            questObjective.UpdateObjective();
        }
    }

    public bool IsCompleted()
    {
        foreach (var objective in Objectives)
        {
            if (!objective.IsCompleted())
            {
                return false;
            }
        }
        return true;
    }
}