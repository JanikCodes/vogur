using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent(name: "Int Operator", path: "Base/Generic/Int Operator", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class IntOperatorDecorator : ConditionDecorator
{
    [Header("Variables")]
    [SerializeField]
    private IntKey firstInput;

    [SerializeField]
    private OperationTypes operation;

    [SerializeField]
    private IntKey secondInput;

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
        int firstValue = firstInput.GetValue();
        int secondValue = secondInput.GetValue();

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
