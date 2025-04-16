using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DialogueService : ScriptableObjectService<DialogueSO>
{
    public override void CreateScriptableObjects()
    {
        List<DialogueData> rawDataDialogues = DataService.CreateDataFromFilesAndMods<DialogueData>("Dialogues");
        List<DialogueChoiceData> rawDataDialogueChoices = DataService.CreateDataFromFilesAndMods<DialogueChoiceData>("DialogueChoices");

        foreach (DialogueData dialogueData in rawDataDialogues)
        {
            if (!dialogueData.Active) { continue; }

            DialogueSO dialogue = ScriptableObject.CreateInstance<DialogueSO>();
            dialogue.name = dialogueData.Id;
            dialogue.Id = dialogueData.Id;
            dialogue.Text = dialogueData.Text;

            // assign choices to dialogue
            foreach (DialogueChoiceData dialogueChoiceData in rawDataDialogueChoices)
            {
                if (!dialogueChoiceData.Active) { continue; }
                // ignore if that choice isnt assigned to this dialogue
                if (!dialogueChoiceData.AssignedDialogueId.Equals(dialogue.Id)) { continue; }

                DialogueChoiceSO dialogueChoice = ScriptableObject.CreateInstance<DialogueChoiceSO>();
                dialogueChoice.name = dialogueChoiceData.Id;
                dialogueChoice.Id = dialogueChoiceData.Id;
                dialogueChoice.ChoiceText = dialogueChoiceData.ChoiceText;
                dialogueChoice.RawJumpToDialogueId = dialogueChoiceData.JumpToDialogueId;
                dialogueChoice.TextColor = Util.GetColorFromIntArray(dialogueChoiceData.TextColor);
                dialogueChoice.Requirements = GenerateRequirements(dialogueChoiceData.Requirements);
                dialogueChoice.Actions = GenerateActions(dialogueChoiceData.Actions);
                dialogue.Choices.Add(dialogueChoice);
            }

            scriptableObjects.Add(dialogue);
        }

        // assign DialogueChoice property 'JumpToDialogue' after all DialogueSO's have been created
        foreach (DialogueSO dialogue in scriptableObjects)
        {
            foreach (DialogueChoiceSO dialogueChoice in dialogue.Choices)
            {
                dialogueChoice.JumpToDialogue = GetScriptableObject(dialogueChoice.RawJumpToDialogueId);
            }
        }

        base.CreateScriptableObjects();
    }

    private List<DialogueRequirementSO> GenerateRequirements(DialogueRequirementData[] requirements)
    {
        List<DialogueRequirementSO> results = new();

        // catch early if requirements are null
        if (requirements == null) { return results; }

        // try adding each requirement scriptableobject
        foreach (DialogueRequirementData requirementData in requirements)
        {
            string type = requirementData.Type;
            Type requirementType = Type.GetType($"Dialogue{type}RequirementSO");

            if (requirementType == null || !typeof(DialogueRequirementSO).IsAssignableFrom(requirementType))
            {
                Debug.LogWarning($"Invalid requirement type: {type}");
                continue;
            }

            // generate object based on requirementType
            DialogueRequirementSO requirementSO = ScriptableObject.CreateInstance(requirementType) as DialogueRequirementSO;

            if (requirementSO == null)
            {
                Debug.LogWarning($"Failed to create an instance of {type}");
                continue;
            }

            // try to populate the object with its properties
            foreach (KeyValuePair<string, object> property in requirementData.Properties)
            {
                if (!Util.WriteValueToField(type, requirementType, requirementSO, property))
                {
                    continue;
                }
            }

            results.Add(requirementSO);
        }

        return results;
    }

    private List<DialogueActionSO> GenerateActions(DialogueActionData[] actions)
    {
        List<DialogueActionSO> results = new();

        // catch early if actions are null
        if (actions == null) { return results; }

        // try adding each action scriptableobject
        foreach (DialogueActionData actionData in actions)
        {
            string type = actionData.Type;
            Type actionType = Type.GetType($"Dialogue{type}ActionSO");

            if (actionType == null || !typeof(DialogueActionSO).IsAssignableFrom(actionType))
            {
                Debug.LogWarning($"Invalid action type: {type}");
                continue;
            }

            // generate object based on actionType
            DialogueActionSO actionSO = ScriptableObject.CreateInstance(actionType) as DialogueActionSO;

            if (actionSO == null)
            {
                Debug.LogWarning($"Failed to create an instance of {type}");
                continue;
            }

            // try to populate the object with its properties
            foreach (KeyValuePair<string, object> property in actionData.Properties)
            {
                if (!Util.WriteValueToField(type, actionType, actionSO, property))
                {
                    continue;
                }
            }

            results.Add(actionSO);
        }

        return results;
    }
}
