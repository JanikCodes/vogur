using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using RenownedGames.Apex;
using UnityEngine;

[NodeContent(name:"Has Enough Currency", path:"Base/Faction/Has Enough Currency", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class HasEnoughCurrencyDecorator : ConditionDecorator
{
    [Header("Variables")]
    [SerializeField]
    private IntKey requiredCurrency;

    // Stored required components.
    private CurrencyStorage currencyStorage;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        currencyStorage = GetOwner().GetComponent<CurrencyStorage>();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }
 
    protected override bool CalculateResult()
    {
        return currencyStorage.HasEnoughCurrency(requiredCurrency.GetValue());
    }

    protected override void OnFlowUpdate() { }
}
