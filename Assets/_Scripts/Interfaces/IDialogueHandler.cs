using UnityEngine;

public interface IDialogueHandler
{
    void TalkTo(Transform other, DialogueType type);
    void BeingTalkedTo(Transform other, DialogueType type);
    bool IsInDialogue();
    DialogueType GetDialogueType();
    void ExitDialogue();
}