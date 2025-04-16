using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDismissDialogueActionSO : DialogueActionSO
{
    public override void ExecuteEffect(Transform self, Transform other)
    {
        Transform target = Self ? self : other;
        IDialogueHandler dialogueHandler = target.GetComponent<IDialogueHandler>();
        dialogueHandler.ExitDialogue();
    }
}
