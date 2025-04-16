using System;
using System.Collections.Generic;

/// <summary>
/// DataContainer class dependency for <see cref="ArmySaveData"/> saving dynamic data
/// </summary>
[Serializable]
public class SquadSaveData
{
    public List<SoldierSaveData> Soldiers = new();

    public SquadSaveData()
    {
        // empty constructor for serialization
    }

    public SquadSaveData(List<Soldier> soldiers)
    {
        foreach (Soldier soldier in soldiers)
        {
            SoldierSaveData soldierSaveData = new SoldierSaveData();
            soldierSaveData.Id = soldier.SoldierBaseData.Id;
            soldierSaveData.Name = soldier.Name;
            soldierSaveData.Health = soldier.Health;
            soldierSaveData.Moral = soldier.Moral;

            Soldiers.Add(soldierSaveData);
        }
    }
}