using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FactionSO : DataSO
{
    public string Name;
    public string Description;
    public bool Visible;
    public int MaxArmyCountOnOverworld;
    public int StartCurrencyAmount;
    public int MinSquadAmount;
    public int MaxSquadAmount;
    public FactionArmySpawnType[] FactionArmySpawnType;
    public List<FactionSquadPresetSO> SquadPresets;
    public DialogueSO AssignedArmyDialogue;
}
