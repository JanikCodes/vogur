using System.Collections.Generic;
using UnityEngine;
using static BuildingData;

public class BuildingService : ScriptableObjectService<BuildingSO>
{
    private ItemService itemService;
    private SoldierService soldierService;

    private void Awake()
    {
        itemService = GetOtherService<ItemService>();
        soldierService = GetOtherService<SoldierService>();
    }

    public override void CreateScriptableObjects()
    {
        List<BuildingData> rawData = DataService.CreateDataFromFilesAndMods<BuildingData>("Buildings");

        foreach (BuildingData data in rawData)
        {
            if (!data.Active) { continue; }

            BuildingSO building = ScriptableObject.CreateInstance<BuildingSO>();
            building.name = data.Id;
            building.Id = data.Id;
            building.Name = data.Name;
            building.Description = data.Description;
            building.BuildPrice = data.BuildPrice;
            building.BuildingType = data.BuildingType;

            // add all produceable items
            foreach (BuildingProductionItemData buildingProductionItemData in data.ProduceItems)
            {
                ItemSO itemSO = itemService.GetScriptableObject(buildingProductionItemData.ItemId);
                BuildingProductionItem buildingProductionItem = new(buildingProductionItemData, itemSO);
                if (buildingProductionItem == null)
                {
                    Debug.LogWarning("Building Production Item with Id: " + buildingProductionItemData.ItemId + " couldn't be found.");
                    continue;
                }

                building.ProduceItems.Add(buildingProductionItem);
            }

            // add all produceable soldiers
            foreach (BuildingProductionSoldierData buildingProductionSoldierData in data.ProduceSoldiers)
            {
                SoldierSO soldierSO = soldierService.GetScriptableObject(buildingProductionSoldierData.SoldierId);
                BuildingProductionSoldier buildingProductionSoldier = new(buildingProductionSoldierData, soldierSO);
                if (buildingProductionSoldier == null)
                {
                    Debug.LogWarning("Building Production Soldier with Id: " + buildingProductionSoldierData.SoldierId + " couldn't be found.");
                    continue;
                }

                building.ProduceSoldiers.Add(buildingProductionSoldier);
            }

            building.Intervall = data.Intervall;

            scriptableObjects.Add(building);
        }

        base.CreateScriptableObjects();
    }
}
