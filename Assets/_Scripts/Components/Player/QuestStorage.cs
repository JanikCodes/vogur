using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStorage : MonoBehaviour
{
    [field: SerializeField] public List<Quest> Quests { get; private set; }

    private void Awake()
    {
        Quests = new();
    }

    /// <summary>
    /// Converts all available quests from <see cref="QuestService"/> to real <see cref="Quest"/> objects.
    /// </summary>
    /// <param name="data"></param>
    public void InstantiateQuests(List<QuestSO> data)
    {
        foreach (QuestSO questSO in data)
        {
            Quest quest = new Quest(transform, questSO);
            Quests.Add(quest);
        }
    }

    public bool IsAtQuestStep(string stepId)
    {
        foreach (Quest quest in Quests)
        {
            if (quest.IsAtQuestStep(stepId))
            {
                return true;
            }
        }

        return false;
    }

    public void LoadQuestProgress(QuestSaveData questSaveData)
    {
        Quest quest = Quests.Find(quest => quest.QuestBaseData.Id.Equals(questSaveData.Id));
        if (quest == null)
        {
            Debug.LogWarning("Couldn't find quest ID: " + questSaveData.Id + " to update the progress from SaveData");
            return;
        }

        foreach (QuestStepSaveData questStepSaveData in questSaveData.Steps)
        {
            QuestStep questStep = quest.Steps.Find(step => step.QuestStepBaseData.Id.Equals(questStepSaveData.Id));
            if (questStep == null)
            {
                Debug.LogWarning("Couldn't find quest step ID: " + questStepSaveData.Id + " to update the progress from SaveData");
                continue;
            }

            for (int i = 0; i < questStepSaveData.Objectives.Count; i++)
            {
                QuestObjectiveSaveData questObjectiveSaveData = questStepSaveData.Objectives[i];
                QuestObjective questObjective = questStep.Objectives[i];
                if (questObjective == null)
                {
                    Debug.LogWarning("Couldn't find quest objective at index: " + i + " to update the progress from SaveData");
                    continue;
                }

                questObjective.IntCheck = questObjectiveSaveData.CurrencyRemaining;
                questObjective.BoolCheck = questObjectiveSaveData.VisitedTarget;
            }
        }
    }

    private void Update()
    {
        UpdateQuests();
    }

    /// <summary>
    /// Handles the updating for quest objectives based on if a quest step is not completed.
    /// </summary>
    private void UpdateQuests()
    {
        foreach (Quest quest in Quests)
        {
            quest.UpdateSteps();
        }
    }
}
