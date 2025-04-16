using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;
using static IntOperatorDecorator;

[NodeContent(name: "Float Operator", path: "Base/Generic/Float Operator", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class FloatOperatorDecorator : ConditionDecorator
{
    [Header("Variables")]
    [SerializeField]
    private FloatKey firstInput;

    [SerializeField]
    private OperationTypes operation;

    [SerializeField]
    private FloatKey secondInput;

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
        float firstValue = firstInput.GetValue();
        float secondValue = secondInput.GetValue();

        switch (operation)
        {
            case OperationTypes.Greater:
                return firstValue > secondValue;
            case OperationTypes.Smaller:
                return firstValue < secondValue;
            case OperationTypes.GreaterEqual:
                return firstValue >= secondValue;
            case OperationTypes.SmallerEqual:
                return firstValue <= secondValue;
            case OperationTypes.Equal:
                return firstValue == secondValue;
            case OperationTypes.NotEqual:
                return firstValue != secondValue;
        }

        return false;
    }

    protected override void OnFlowUpdate() { }
}
