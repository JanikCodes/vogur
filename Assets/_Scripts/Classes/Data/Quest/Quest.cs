using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="QuestSO"/> at runtime from.
/// </summary>
[Serializable]
public class QuestData : BaseData
{
    public string Title;
    public string Description;
    public QuestStepData[] Steps;
    public bool AcceptedFromStart;
    public bool AutoComplete;
}

[Serializable]
public class Quest
{
    public QuestSO QuestBaseData;
    public List<QuestStep> Steps = new();
    public bool Accepted;

    public Quest(Transform self, QuestSO data)
    {
        QuestBaseData = data;
        Accepted = data.AcceptedFromStart;

        foreach (QuestStepSO questStepSO in data.Steps)
        {
            QuestStep questStep = new(self, questStepSO);
            questStep.InitializeObjectives();

            Steps.Add(questStep);
        }
    }

    public void UpdateSteps()
    {
        if (!Accepted) { return; }

        // cycle through steps
        foreach (QuestStep questStep in Steps)
        {
            // check if index is completed yet
            if (!questStep.IsCompleted())
            {
                questStep.UpdateObjectives();

                // stop updating other steps
                return;
            }
        }
    }

    public bool IsCompleted()
    {
        foreach (var objective in Steps)
        {
            if (!objective.IsCompleted())
            {
                return false;
            }
        }
        return true;
    }

    public bool IsAtQuestStep(string stepId)
    {
        // do we have this step in our quest?
        QuestStep foundStep = Steps.Find(step => step.QuestStepBaseData.Id.Equals(stepId));
        if (foundStep == null)
        {
            return false;
        }

        // check if we're at the current step
        bool valid = true;
        foreach (QuestStep questStep in Steps)
        {
            if (!questStep.Equals(foundStep))
            {
                if (questStep.IsCompleted())
                {
                    continue;
                }
                else
                {
                    valid = false;
                }
            }
            else
            {
                return valid;
            }
        }

        return valid;
    }
}