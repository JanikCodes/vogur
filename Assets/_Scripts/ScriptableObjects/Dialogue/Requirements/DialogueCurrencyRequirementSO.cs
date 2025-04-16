using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCurrencyRequirementSO : DialogueRequirementSO
{
    public int RequiredCurrency;

    public override bool CheckRequirements(Transform self, Transform other)
    {
        Transform target = Self ? self : other;

        CurrencyStorage currencyStorage = target.GetComponent<CurrencyStorage>();
        return currencyStorage.HasEnoughCurrency(RequiredCurrency);
    }
}
