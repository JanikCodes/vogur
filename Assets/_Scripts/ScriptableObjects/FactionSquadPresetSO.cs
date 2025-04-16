using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FactionSquadPresetSO : DataSO
{
    public List<SoldierSO> Soldiers;
    public string AssignedFactionId;
}
