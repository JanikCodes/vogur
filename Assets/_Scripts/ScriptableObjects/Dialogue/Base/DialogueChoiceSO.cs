using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueChoiceSO : DataSO
{
    [NonSerialized] public DialogueSO JumpToDialogue;
    public string ChoiceText;
    public string RawJumpToDialogueId;
    public Color TextColor;
    public List<DialogueRequirementSO> Requirements = new();
    public List<DialogueActionSO> Actions = new();

    public bool MeetsRequirements(Transform self, Transform other)
    {
        bool valid = true;

        // Check if any requirement is false
        foreach (DialogueRequirementSO requirementSO in Requirements)
        {
            if (!requirementSO.CheckRequirements(self, other))
            {
                valid = false;
                break;
            }
        }

        return valid;
    }

    public void ExecuteActions(Transform self, Transform other)
    {
        foreach (DialogueActionSO action in Actions)
        {
            action.ExecuteEffect(self, other);
        }
    }
}
