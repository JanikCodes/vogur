using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestSO : DataSO
{
    public string Title;
    public string Description;
    public List<QuestStepSO> Steps = new();
    public bool AcceptedFromStart;
    public bool AutoComplete;
}
