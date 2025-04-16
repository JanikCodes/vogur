using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class ArmyDialogueHandler : MonoBehaviour, IDialogueHandler
{
    [Tooltip("True if a dialogue is actively running.")]
    [field: SerializeField, ReadOnlyField] public bool active { get; private set; }
    [field: SerializeField, ReadOnlyField] public DialogueType dialogueType { get; private set; }
    [field: SerializeField, ReadOnlyField] public Transform other { get; private set; }
    [field: SerializeField, ReadOnlyField] public DialogueImmunity dialogueImmunity { get; private set; }

    private void Awake()
    {
        dialogueImmunity = GetComponent<DialogueImmunity>();
    }

    public void BeingTalkedTo(Transform other, DialogueType type)
    {
        if (active) { return; }

        active = true;
        dialogueType = type;
        this.other = other;

        Debug.Log("AI is being talked by ... " + other.name + " with type: " + type);
    }

    public bool IsInDialogue()
    {
        return active;
    }

    public void TalkTo(Transform other, DialogueType type)
    {
        if (active) { return; }

        IDialogueHandler otherDialogueHandler = other.GetComponent<IDialogueHandler>();
        if (otherDialogueHandler == null)
        {
            Debug.LogWarning("Couldn't instantiate dialogue because the other is missing a component that is inheriting from IDialogueHandler.");
            return;
        }

        // is other not already in dialogue?
        if (otherDialogueHandler.IsInDialogue()) { return; }

        // set states
        active = true;
        dialogueType = type;
        this.other = other;

        Debug.Log("AI is talking to ... " + other.name + " with type: " + type);

        // notify other that we're talking to him
        otherDialogueHandler.BeingTalkedTo(transform, dialogueType);
    }

    public DialogueType GetDialogueType()
    {
        return dialogueType;
    }

    public void ExitDialogue()
    {
        dialogueImmunity.SetImmunity(25f);

        Transform cacheOther = other;

        other = null;
        active = false;

        Debug.Log("AI exited dialogue ...");
        NotifyOtherAboutExit(cacheOther);
    }

    private void NotifyOtherAboutExit(Transform other)
    {
        if (other == null) { return; }

        IDialogueHandler otherDialogueHandler = other.GetComponent<IDialogueHandler>();

        if (otherDialogueHandler == null) { return; }
        if (!otherDialogueHandler.IsInDialogue()) { return; }

        otherDialogueHandler.ExitDialogue();
    }
}
