using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestAquireCurrencyTypeSO : QuestObjectiveSO
{
    public int Amount;

    private QuestObjective data;

    public override void Initialize(QuestObjective questObjective)
    {
        data = questObjective;

        data.IntCheck = Amount;
        data.Self.GetComponent<CurrencyStorage>().OnCurrencyChanged += HandleCurrenyChange;
    }

    public override void UpdateObjective(QuestObjective questObjective)
    {
        // not needed
    }

    public override bool IsComplete(QuestObjective questObjective)
    {
        return data.IntCheck == 0;
    }

    private void HandleCurrenyChange(int changedAmount)
    {
        if (changedAmount > 0)
        {
            int remaining = Mathf.Max(0, data.IntCheck - changedAmount);
            data.IntCheck = remaining;
        }
    }
}
