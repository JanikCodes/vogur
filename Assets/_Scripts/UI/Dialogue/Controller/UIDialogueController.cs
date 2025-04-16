using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class UIDialogueController : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] private float defaultXPosition;
    [SerializeField] private float activeXPosition;
    [SerializeField] private float tweenDuration;

    [Header("References")]
    [SerializeField] private Transform choiceParent;
    [SerializeField] private TextMeshProUGUI textlabel;
    [SerializeField] private TextMeshProUGUI partnerNameLabel;

    [Header("Prefabs")]
    [SerializeField] private GameObject choicePrefab;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        PlayerDialogueHandler.OnDialogueInstantiated += HandleDialogueInstantiated;
        UIDialogueChoiceItemController.OnDialogueJump += HandleDialogueJump;
        PlayerDialogueHandler.OnDialogueDismiss += HanndleDialogueDismiss;
    }

    private void OnDisable()
    {
        PlayerDialogueHandler.OnDialogueInstantiated -= HandleDialogueInstantiated;
        UIDialogueChoiceItemController.OnDialogueJump += HandleDialogueJump;
        PlayerDialogueHandler.OnDialogueDismiss -= HanndleDialogueDismiss;
    }

    public void CallDialogueWindow()
    {
        rectTransform.DOLocalMoveX(activeXPosition, tweenDuration).SetUpdate(true);
    }

    public void DismissDialogueWindow()
    {
        rectTransform.DOLocalMoveX(defaultXPosition, tweenDuration).SetUpdate(true);
    }

    private void HandleDialogueInstantiated(Transform self, Transform other)
    {
        DialogueTrigger dialogueTrigger = other.GetComponent<DialogueTrigger>();
        if (!dialogueTrigger)
        {
            Debug.LogWarning("Couldn't instantiate dialogue UI because DialogueTrigger is missing on the partner.");
            return;
        }

        RenderDialogue(dialogueTrigger.Dialogue, self, other);

        CallDialogueWindow();
    }

    private void HandleDialogueJump(DialogueSO dialogue, Transform self, Transform other)
    {
        RenderDialogue(dialogue, self, other);
    }

    private void HanndleDialogueDismiss(Transform self, Transform other)
    {
        DismissDialogueWindow();
    }

    private void RenderDialogue(DialogueSO dialogue, Transform self, Transform other)
    {
        // clear previous choices
        Util.ClearChildren(choiceParent);

        // set text
        textlabel.text = dialogue.Text;

        // set name
        partnerNameLabel.text = other.name;

        foreach (DialogueChoiceSO dialogueChoiceSO in dialogue.GetChoices(self, other))
        {
            GameObject choice = Instantiate(choicePrefab, choiceParent);
            UIDialogueChoiceItemController choiceController = choice.GetComponent<UIDialogueChoiceItemController>();
            choiceController.Setup(dialogueChoiceSO, self, other);
        }
    }
}
