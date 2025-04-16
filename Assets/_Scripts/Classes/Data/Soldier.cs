using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="SoldierSO"/> at runtime from.
/// </summary>
[Serializable]
public class SoldierData : BaseData
{
    public string Name;
    public string Description;
    public float MovementSpeed;
    public int MaxHealth;
}

[Serializable]
public class Soldier
{
    public SoldierSO SoldierBaseData;

    [Header("Dynamic Data")]
    public string Name;
    public int Health;
    public int Moral;

    public Soldier()
    {
        // empty constructor for serialization
    }

    public Soldier(SoldierSO data)
    {
        SoldierBaseData = data;
        Name = data.Name;
        Health = data.MaxHealth;
        Moral = 100;
    }

    public Soldier(SoldierSO data, SoldierSaveData save)
    {
        SoldierBaseData = data;
        Name = save.Name;
        Health = save.Health;
        Moral = save.Moral;
    }
}