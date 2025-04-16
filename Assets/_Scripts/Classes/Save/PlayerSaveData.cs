using System;
using System.Collections.Generic;

/// <summary>
/// DataContainer class dependency for <see cref="PlayerData"/> saving dynamic data
/// </summary>
[Serializable]
public class PlayerSaveData
{
    public string GUID;
    public string OwnedByFactionId;
    public float[] Position;
    public float[] Rotation;
    public int Currency;
    public List<SquadSaveData> Squads = new();
    public InventorySaveData Inventory;
    public List<QuestSaveData> Quests = new();

    public PlayerSaveData()
    {
        // empty constructor for serialization
    }
}