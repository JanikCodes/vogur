using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent(name: "Has Dialogue Type", path: "Base/Army/Has Dialogue Type", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class HasDialogueTypeDecorator : ConditionDecorator
{
    [SerializeField]
    private DialogueType type;

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
        // only the player can contain this component, therefore it's a save check to see if the transform is the player.
        IDialogueHandler dialogueHandler = GetOwner().GetComponent<IDialogueHandler>();

        if (dialogueHandler == null)
        {
            Debug.LogError("Army has no component that inherites from IDialogueHandler!");
            return false;
        }

        return dialogueHandler.GetDialogueType().Equals(type);
    }

    protected override void OnFlowUpdate() { }
}
