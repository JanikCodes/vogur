using System;
using System.Collections.Generic;

/// <summary>
/// DataContainer class dependency for <see cref="FactionData"/> saving dynamic data
/// </summary>
[Serializable]
public class ArmySaveData
{
    public string GUID;
    public float[] Position;
    public float[] Rotation;
    public int Currency;
    public List<SquadSaveData> Squads = new();
    public InventorySaveData Inventory;
    public bool DialogueActive = false;
    public string DialogueType;
    public string DialogueOtherGUID;
}