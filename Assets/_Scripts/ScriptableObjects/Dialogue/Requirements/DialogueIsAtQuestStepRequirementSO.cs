using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueIsAtQuestStepRequirementSO : DialogueRequirementSO
{
    public string QuestStepId;

    public override bool CheckRequirements(Transform self, Transform other)
    {
        QuestStorage questStorage = self.GetComponent<QuestStorage>();

        return questStorage.IsAtQuestStep(QuestStepId);
    }
}
