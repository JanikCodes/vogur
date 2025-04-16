using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemService : ScriptableObjectService<ItemSO>
{
    public override void CreateScriptableObjects()
    {
        List<ItemData> rawData = DataService.CreateDataFromFilesAndMods<ItemData>("Items");

        foreach (ItemData data in rawData)
        {
            if (!data.Active) { continue; }

            ItemSO item = ScriptableObject.CreateInstance<ItemSO>();
            item.name = data.Id;
            item.Id = data.Id;
            item.Name = data.Name;
            item.Description = data.Description;
            item.BaseValue = data.BaseValue;
            item.MaxStackSize = data.MaxStackSize;
            item.ItemType = Util.ReturnEnumValueFromStringValue<ItemType>(data.ItemType);
            item.Rarity = Util.ReturnEnumValueFromStringValue<RarityType>(data.Rarity);
            item.FoodValue = data.FoodValue;


            scriptableObjects.Add(item);
        }

        base.CreateScriptableObjects();
    }

    /// <summary>
    /// This methode can be used for structures or armies to receive random items that make sense for said object based on existing buildings and soldier strength. 
    /// </summary>
    /// <returns>Random item list which represent the inventory for said object.</returns>
    public List<Item> GetEconomyInventory(BuildingStorage buildingStorage = null, SquadStorage squadStorage = null)
    {
        List<Item> items = new();

        if (squadStorage)
        {
            // fill inventory based on soldiers
            int soldierCount = squadStorage.GetTotalSoldierCount();

            int foodItemCount = soldierCount / 3;
            int resourceItemCount = soldierCount / 3;
            int luxuryItemCount = 0;

            // calculate luxury chance
            int luxuryChance = Util.GetRandomValue(0, 100);
            if (luxuryChance <= 25)
            {
                luxuryItemCount = Util.GetRandomValue(1, 2);
            }

            // TODO: calculate available item rarities based on army strength/size
            List<RarityType> availableRarities = new();
            availableRarities.Add(RarityType.Common);

            items.AddRange(GetRandomItems(ItemType.Food, availableRarities, foodItemCount));
            items.AddRange(GetRandomItems(ItemType.Resource, availableRarities, resourceItemCount));
            items.AddRange(GetRandomItems(ItemType.Luxury, availableRarities, luxuryItemCount));
        }

        if (buildingStorage)
        {
            // retrieve all stored buildingSO's
            List<BuildingSO> buildingSOs = buildingStorage.Buildings.ConvertAll(x => x.BuildingBaseData);
            // loop through every building and add produceable items to the pool
            foreach (BuildingSO buildingSO in buildingSOs)
            {
                // loop through every produceable item in building
                foreach (BuildingProductionItem buildingProductionItem in buildingSO.ProduceItems)
                {
                    // add item based on BuildingProductionItem chance value
                    if (Util.GetRandomValue(0, 100) <= buildingProductionItem.Chance)
                    {
                        Item item = new();
                        item.ItemBaseData = buildingProductionItem.ItemBaseData;
                        item.Count = Util.GetRandomValue(buildingProductionItem.MinAmount, buildingProductionItem.MaxAmount);
                        items.Add(item);
                    }
                }
            }
        }

        return items;
    }

    /// <summary>
    /// Returns random items from all existing ones based on a given type, possible rarities and a count
    /// </summary>
    public List<Item> GetRandomItems(ItemType itemType, List<RarityType> possibleRarities, int uniqueItems)
    {
        List<Item> items = new();

        // get random unique items from count
        List<ItemSO> availableItems = Util.GetRandomValues(GetItems(itemType, possibleRarities), uniqueItems);

        foreach (ItemSO itemSO in availableItems)
        {
            int minCount = Mathf.Max(1, itemSO.MaxStackSize / 2);
            int itemCount = Util.GetRandomValue(minCount, itemSO.MaxStackSize);

            Item newItem = new();
            newItem.ItemBaseData = itemSO;
            newItem.Count = itemCount;
            items.Add(newItem);
        }

        return items;
    }

    /// <summary>
    /// Returns all existing itemSO's based on a given type and possible rarities
    /// </summary>
    private List<ItemSO> GetItems(ItemType itemType, List<RarityType> possibleRarities)
    {
        return scriptableObjects.FindAll(item => item.ItemType.Equals(itemType) && possibleRarities.Contains(item.Rarity));
    }
}
