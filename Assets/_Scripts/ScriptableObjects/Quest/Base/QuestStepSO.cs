using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestStepSO : DataSO
{
    public List<QuestObjectiveSO> Objectives = new();
}
