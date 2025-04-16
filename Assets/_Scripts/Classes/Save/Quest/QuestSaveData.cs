using System;
using System.Collections.Generic;

/// <summary>
/// DataContainer class for <see cref="QuestData"/> saving dynamic data
/// </summary>
[Serializable]
public class QuestSaveData
{
    public string Id;
    public List<QuestStepSaveData> Steps = new();
    public bool Accepted;

    public QuestSaveData()
    {
        // empty constructor for serialization
    }

    public QuestSaveData(Quest quest)
    {
        Id = quest.QuestBaseData.Id;

        foreach (QuestStep questStep in quest.Steps)
        {
            QuestStepSaveData questStepSaveData = new(questStep);
            Steps.Add(questStepSaveData);
        }

        Accepted = quest.Accepted;
    }
}