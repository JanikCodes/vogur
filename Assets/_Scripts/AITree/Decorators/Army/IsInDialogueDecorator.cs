using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent(name: "Is In Dialogue", path: "Base/Army/Is In Dialogue", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class IsInDialogueDecorator : ConditionDecorator
{
    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override bool CalculateResult()
    {
        IDialogueHandler dialogueHandler = GetOwner().GetComponent<IDialogueHandler>();

        if (dialogueHandler == null)
        {
            Debug.LogError("Army has no component that inherites from IDialogueHandler!");
            return false;
        }

        return dialogueHandler.IsInDialogue();
    }

    protected override void OnFlowUpdate() { }
}
