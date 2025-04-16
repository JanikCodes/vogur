using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="BuildingSO"/> at runtime from.
/// </summary>
[Serializable]
public class BuildingData : BaseData
{
    public string Name;
    public string Description;
    public int BuildPrice;
    public string BuildingType;
    public BuildingProductionItemData[] ProduceItems;
    public BuildingProductionSoldierData[] ProduceSoldiers;
    public int Intervall;

    [Serializable]
    public class BuildingProductionItemData
    {
        public string ItemId;
        public int MinAmount;
        public int MaxAmount;
        public int Chance;
    }

    [Serializable]
    public class BuildingProductionSoldierData
    {
        public string SoldierId;
        public int MinAmount;
        public int MaxAmount;
        public int Chance;
    }
}

[Serializable]
public class Building
{
    public BuildingSO BuildingBaseData;
    public int BuildingProgress;

    public Building()
    {
        // empty constructor for serialization
    }

    public Building(BuildingSO buildingSO, BuildingSaveData buildingSaveData)
    {
        BuildingBaseData = buildingSO;
        BuildingProgress = buildingSaveData.BuildingProgress;
    }
}

[Serializable]
public class BuildingProductionItem
{
    public ItemSO ItemBaseData;
    public int MinAmount;
    public int MaxAmount;
    public int Chance;

    public BuildingProductionItem()
    {
        // empty constructor for serialization
    }

    public BuildingProductionItem(BuildingData.BuildingProductionItemData buildingProductionItemData, ItemSO itemSO)
    {
        ItemBaseData = itemSO;
        MinAmount = buildingProductionItemData.MinAmount;
        MaxAmount = buildingProductionItemData.MaxAmount;
        Chance = buildingProductionItemData.Chance;
    }
}

[Serializable]
public class BuildingProductionSoldier
{
    public SoldierSO SoldierBaseData;
    public int MinAmount;
    public int MaxAmount;
    public int Chance;

    public BuildingProductionSoldier()
    {
        // empty constructor for serialization
    }

    public BuildingProductionSoldier(BuildingData.BuildingProductionSoldierData buildingProductionSoldierData, SoldierSO soldierSO)
    {
        SoldierBaseData = soldierSO;
        MinAmount = buildingProductionSoldierData.MinAmount;
        MaxAmount = buildingProductionSoldierData.MaxAmount;
        Chance = buildingProductionSoldierData.Chance;
    }
}