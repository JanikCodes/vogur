using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AIDestinationSetter))]
public class PlayerDialogueHandler : MonoBehaviour, IDialogueHandler
{
    [SerializeField] private float dialogueTriggerDistance;
    [Tooltip("True if a dialogue is actively running.")]
    [SerializeField, ReadOnlyField] private bool active;

    private AIDestinationSetter aIDestinationSetter;
    private TimeServiceReference timeServiceReference;
    private DialogueImmunity dialogueImmunity;

    private Transform other;
    private DialogueType dialogueType;

    // events
    public static event DialogueDelegate OnDialogueInstantiated;
    public static event DialogueDelegate OnDialogueDismiss;
    public delegate void DialogueDelegate(Transform self, Transform other);

    private void Awake()
    {
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        timeServiceReference = GetComponent<TimeServiceReference>();
        dialogueImmunity = GetComponent<DialogueImmunity>();
    }

    private void Update()
    {
        if (active) { return; }
        if (!aIDestinationSetter.target) { return; }

        bool trigger = ShouldTriggerDialogue();

        if (trigger)
        {
            TalkTo(aIDestinationSetter.target, DialogueType.Talking);
        }
    }

    public void BeingTalkedTo(Transform other, DialogueType type)
    {
        if (active) { return; }

        DialogueTrigger dialogueTrigger = other.GetComponent<DialogueTrigger>();
        if (!dialogueTrigger)
        {
            Debug.LogWarning("Couldn't instantiate dialogue because DialogueTrigger is missing on the partner.");
            return;
        }

        if (!dialogueTrigger.Dialogue)
        {
            Debug.LogWarning("Couldn't instantiate dialogue because the dialogue is missing on the partner.");
            return;
        }

        // pause time hard till dialogue is resolved
        timeServiceReference.Service.SetTime(TimeState.Paused, true);

        // set states
        active = true;
        dialogueType = type;
        this.other = other;

        // notify subscribers and quest progress
        OnDialogueInstantiated?.Invoke(transform, other);

        Debug.Log("Player is being talked to by ... " + other.name);
    }

    public bool IsInDialogue()
    {
        return active;
    }

    /// <summary>
    /// This methode is ONLY executed by the player.
    /// </summary>
    public void TalkTo(Transform other, DialogueType type)
    {
        if (active) { return; }

        DialogueTrigger dialogueTrigger = other.GetComponent<DialogueTrigger>();
        if (!dialogueTrigger)
        {
            Debug.LogWarning("Couldn't instantiate dialogue because DialogueTrigger is missing on the partner.");
            return;
        }

        if (!dialogueTrigger.Dialogue)
        {
            Debug.LogWarning("Couldn't instantiate dialogue because the dialogue is missing on the partner.");
            return;
        }

        IDialogueHandler otherDialogueHandler = other.GetComponent<IDialogueHandler>();
        if (otherDialogueHandler != null)
        {
            // notify other that we're talking to him
            otherDialogueHandler.BeingTalkedTo(transform, dialogueType);
        }

        // pause time hard till dialogue is resolved
        timeServiceReference.Service.SetTime(TimeState.Paused, true);

        // set states
        active = true;
        dialogueType = type;
        this.other = other;

        // notify subscribers and quest progress
        OnDialogueInstantiated?.Invoke(transform, other);

        Debug.Log("Player is talking to ... " + other.name);
    }

    public DialogueType GetDialogueType()
    {
        return dialogueType;
    }

    public void ExitDialogue()
    {
        OnDialogueDismiss?.Invoke(transform, other);

        // resume time
        timeServiceReference.Service.SetTime(TimeState.Playing, true);

        dialogueImmunity.SetImmunity(25f);
        active = false;

        NotifyOtherAboutExit();
    }

    private bool ShouldTriggerDialogue()
    {
        return Vector3.Distance(aIDestinationSetter.target.position, transform.position) <= dialogueTriggerDistance;
    }

    private void NotifyOtherAboutExit()
    {
        if (other == null) { return; }

        IDialogueHandler otherDialogueHandler = other.GetComponent<IDialogueHandler>();

        if (otherDialogueHandler == null) { return; }
        if (!otherDialogueHandler.IsInDialogue()) { return; }

        otherDialogueHandler.ExitDialogue();
    }
}
