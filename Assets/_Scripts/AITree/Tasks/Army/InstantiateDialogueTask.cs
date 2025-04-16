using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Instantiate Dialogue", "Tasks/Base/Army/Instantiate Dialogue", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class InstantiateDialogueTask : TaskNode
{
    [Header("Variables")]
    [SerializeField]
    private TransformKey target;

    [SerializeField]
    private DialogueType dialogueType;

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

        // instantiate dialogue
        dialogueHandler.TalkTo(target.GetValue(), dialogueType);

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}
