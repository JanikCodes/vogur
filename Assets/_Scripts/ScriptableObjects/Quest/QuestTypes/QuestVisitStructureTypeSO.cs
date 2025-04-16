using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestVisitStructureTypeSO : QuestObjectiveSO
{
    public string Target;

    private QuestObjective data;
    private DataSO otherDataSO;

    public override void Initialize(QuestObjective questObjective)
    {
        data = questObjective;

        PlayerDialogueHandler.OnDialogueInstantiated += HandleInstantiatedDialogue;
    }

    public override void UpdateObjective(QuestObjective questObjective)
    {
        if (otherDataSO == null) { return; }

        if (otherDataSO.Id.Equals(Target))
        {
            data.BoolCheck = true;
        }
        else
        {
            otherDataSO = null;
        }
    }

    public override bool IsComplete(QuestObjective questObjective)
    {
        return questObjective.BoolCheck;
    }

    private void HandleInstantiatedDialogue(Transform self, Transform other)
    {
        DataSO dataSO = other.GetComponent<ObjectStorage>().GetObject<DataSO>();
        if (!dataSO) { return; }

        otherDataSO = dataSO;
    }
}
