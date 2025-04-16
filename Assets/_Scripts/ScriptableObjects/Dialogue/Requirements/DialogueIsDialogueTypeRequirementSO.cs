using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueIsDialogueTypeRequirementSO : DialogueRequirementSO
{
    public string Type;

    public override bool CheckRequirements(Transform self, Transform other)
    {
        IDialogueHandler dialogueHandler = self.GetComponent<IDialogueHandler>();
        return dialogueHandler.GetDialogueType().ToString().Equals(Type);
    }
}
