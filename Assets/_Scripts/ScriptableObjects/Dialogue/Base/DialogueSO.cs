using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueSO : DataSO
{
    public string Text;
    public List<DialogueChoiceSO> Choices = new();

    public List<DialogueChoiceSO> GetChoices(Transform self, Transform other)
    {
        List<DialogueChoiceSO> choices = new List<DialogueChoiceSO>();

        foreach (DialogueChoiceSO choice in Choices)
        {
            if (choice.MeetsRequirements(self, other))
            {
                choices.Add(choice);
            }
        }

        return choices;
    }
}
