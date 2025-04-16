using System;
using System.Collections.Generic;

/// <summary>
/// DataContainer class dependency for <see cref="QuestSaveData"/> saving dynamic data
/// </summary>
[Serializable]
public class QuestStepSaveData
{
    public string Id;
    public List<QuestObjectiveSaveData> Objectives = new();

    public QuestStepSaveData()
    {
        // empty constructor for serialization
    }

    public QuestStepSaveData(QuestStep questStep)
    {
        Id = questStep.QuestStepBaseData.Id;

        foreach (QuestObjective questObjective in questStep.Objectives)
        {
            QuestObjectiveSaveData questObjectiveSaveData = new(questObjective);
            Objectives.Add(questObjectiveSaveData);
        }
    }
}