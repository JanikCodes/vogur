using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueChoiceItemController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI label;

    private DialogueChoiceSO data;
    private Transform self, other;
    private Button button;

    // events
    public static event OnDialogueJumpDelegate OnDialogueJump;
    public delegate void OnDialogueJumpDelegate(DialogueSO dialogue, Transform self, Transform other);

    private void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);
    }

    public void Setup(DialogueChoiceSO dialogueChoiceSO, Transform self, Transform other)
    {
        data = dialogueChoiceSO;
        label.color = dialogueChoiceSO.TextColor;

        label.text = data.ChoiceText;

        this.self = self;
        this.other = other;
    }

    /// <summary>
    /// Executes all actions attached to the dialogueAction and if available render new dialogue
    /// </summary>
    public void OnClick()
    {
        // execute actions
        data.ExecuteActions(self, other);

        if (data.JumpToDialogue)
        {
            OnDialogueJump?.Invoke(data.JumpToDialogue, self, other);
        }
    }
}
