using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestService : ScriptableObjectService<QuestSO>
{
    public override void CreateScriptableObjects()
    {
        List<QuestData> rawData = DataService.CreateDataFromFilesAndMods<QuestData>("Quests");

        foreach (QuestData data in rawData)
        {
            if (!data.Active) { continue; }

            QuestSO quest = ScriptableObject.CreateInstance<QuestSO>();
            quest.name = data.Id;
            quest.Id = data.Id;
            quest.Title = data.Title;
            quest.Description = data.Description;

            foreach (QuestStepData questStepData in data.Steps)
            {
                if (!questStepData.Active) { continue; }

                QuestStepSO questStep = ScriptableObject.CreateInstance<QuestStepSO>();
                questStep.name = questStepData.Id;
                questStep.Id = questStepData.Id;
                questStep.Objectives = GenerateQuestObjectives(questStepData.Objectives);
                quest.Steps.Add(questStep);
            }

            quest.AcceptedFromStart = data.AcceptedFromStart;
            quest.AutoComplete = data.AutoComplete;

            scriptableObjects.Add(quest);
        }

        base.CreateScriptableObjects();
    }

    private List<QuestObjectiveSO> GenerateQuestObjectives(QuestObjectiveData[] objectives)
    {
        List<QuestObjectiveSO> results = new();

        // catch early if objectives are null
        if (objectives == null) { return results; }

        // try adding each objective scriptableobject
        foreach (QuestObjectiveData objectiveData in objectives)
        {
            string type = objectiveData.Type;
            Type objectiveType = Type.GetType($"Quest{type}TypeSO");

            if (objectiveType == null || !typeof(QuestObjectiveSO).IsAssignableFrom(objectiveType))
            {
                Debug.LogWarning($"Invalid objective type: {type}");
                continue;
            }

            // generate object based on objectiveType
            QuestObjectiveSO objectiveSO = ScriptableObject.CreateInstance(objectiveType) as QuestObjectiveSO;
            objectiveSO.Text = objectiveData.Text;
            objectiveSO.Type = objectiveData.Type;

            if (objectiveSO == null)
            {
                Debug.LogWarning($"Failed to create an instance of {type}");
                continue;
            }

            // try to populate the object with its properties
            foreach (KeyValuePair<string, object> property in objectiveData.Properties)
            {
                if (!Util.WriteValueToField(type, objectiveType, objectiveSO, property))
                {
                    continue;
                }
            }

            results.Add(objectiveSO);
        }

        return results;
    }
}
