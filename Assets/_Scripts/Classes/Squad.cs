using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Squad
{
    [SerializeField] private List<Soldier> soldiers = new();

    public void AddSoldier(Soldier soldier)
    {
        soldiers.Add(soldier);
    }

    public void RemoveSoldier(Soldier soldier)
    {
        soldiers.Remove(soldier);
    }

    public List<Soldier> GetSoldiers()
    {
        return soldiers;
    }

    public int GetSoldierCount()
    {
        return soldiers.Count;
    }
}
