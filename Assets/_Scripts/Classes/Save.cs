using System;
using System.Collections.Generic;
using UnityEngine;
using static EconomyService;

[Serializable]
public class Save
{
    public float Version;
    public long LastTimePlayed;
    public PlayerSaveData Player = new();
    public List<FactionSaveData> Factions = new();
    public List<StructureSaveData> Structures = new();
    public List<ItemRecord> Economy = new();
    public List<QuestSaveData> Quests = new();
    public CalenderSaveData Calender = new();
}