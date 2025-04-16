using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent(name: "Check Dialogue Immunity", path: "Base/Army/Check Dialogue Immunity", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class CheckDialogueImmunityDecorator : ConditionDecorator
{
    [Header("Variables")]
    [SerializeField]
    private TransformKey transform;

    [SerializeField]
    private BoolKey value;

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
        DialogueImmunity dialogueImmunity = transform.GetValue().GetComponent<DialogueImmunity>();

        return dialogueImmunity.Immune.Equals(value.GetValue());
    }

    protected override void OnFlowUpdate() { }
}