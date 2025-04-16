using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Complete Dialogue", "Tasks/Base/Army/Complete Dialogue", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class CompleteDialogueTask : TaskNode
{
    // Stored required components.
    private IDialogueHandler dialogueHandler;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        dialogueHandler = GetOwner().GetComponent<IDialogueHandler>();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override State OnUpdate()
    {
        if (dialogueHandler == null)
        {
            return State.Failure;
        }

        dialogueHandler.ExitDialogue();

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}
