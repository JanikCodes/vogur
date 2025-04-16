using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureService : ScriptableObjectService<StructureSO>, ISave, ILoad
{
    [SerializeField] private Transform structureParentTransform;
    [Header("Prefabs")]
    [SerializeField] private GameObject structureRootPrefab;

    private FactionService factionService;
    private EconomyService economyService;
    private ItemService itemService;
    private DialogueService dialogueService;
    private StructureBuildingService structureBuildingService;
    private BuildingService buildingService;

    private void Awake()
    {
        factionService = GetOtherService<FactionService>();
        economyService = GetOtherService<EconomyService>();
        itemService = GetOtherService<ItemService>();
        dialogueService = GetOtherService<DialogueService>();
        structureBuildingService = GetOtherService<StructureBuildingService>();
        buildingService = GetOtherService<BuildingService>();
    }

    public void CreateStructureObjects()
    {
        foreach (StructureSO data in scriptableObjects)
        {
            GameObject obj = Instantiate(structureRootPrefab, structureParentTransform);
            obj.name = data.Name;
            obj.transform.localScale = new Vector3(1, 1, 1);

            Vector3 calculatedPosition = Util.CalculatePositionOnTerrain(data.Position);

            obj.transform.rotation = data.Rotation;
            obj.transform.position = calculatedPosition;

            ObjectStorage objectStorage = obj.GetComponent<ObjectStorage>();
            objectStorage.SetObject<StructureSO>(data);

            FactionAssociation factionAssociation = obj.GetComponent<FactionAssociation>();
            factionAssociation.AssociatedFactionTransform = factionService.GetFactionTransform(data.InitiallyOwnedByFaction);

            EconomyServiceReference economyServiceReference = obj.GetComponent<EconomyServiceReference>();
            economyServiceReference.Service = economyService;

            DialogueTrigger dialogueTrigger = obj.GetComponent<DialogueTrigger>();
            dialogueTrigger.Dialogue = data.AssignedDialogue;

            // populate buildings in structure that have field 'AutoBuildOnStart' as true
            BuildingStorage buildingStorage = obj.GetComponent<BuildingStorage>();
            buildingStorage.Buildings = structureBuildingService.GetAutoBuildAtStartStructureBuildings(data.Id);

            // set items based on buildings
            Inventory inventory = obj.GetComponent<Inventory>();
            inventory.SetItems(itemService.GetEconomyInventory(buildingStorage: buildingStorage));
        }
    }

    public override void CreateScriptableObjects()
    {
        List<StructureData> rawData = DataService.CreateDataFromFilesAndMods<StructureData>("Structures");

        foreach (StructureData data in rawData)
        {
            if (!data.Active) { continue; }

            StructureSO structure = ScriptableObject.CreateInstance<StructureSO>();
            structure.name = data.Id;
            structure.Id = data.Id;
            structure.Name = data.Name;
            structure.Description = data.Description;
            structure.Position = new Vector3(data.Position[0], data.Position[1], data.Position[2]);
            structure.Rotation = Quaternion.Euler(0, data.Rotation, 0);
            structure.InitiallyOwnedByFaction = data.OwnedByFactionId;
            structure.AssignedDialogue = dialogueService.GetScriptableObject(data.AssignedDialogueId);
            structure.PossibleBuildings = structureBuildingService.GetPossibleStructureBuildings(data.Id);
            // structure.AssignedPrefab = modelCore.GetPrefabFromId(data.AssignedPrefabId);

            scriptableObjects.Add(structure);
        }

        base.CreateScriptableObjects();
    }

    public List<Transform> GetFactionOwnedStructures(FactionSO faction)
    {
        List<Transform> result = new();

        foreach (Transform structure in structureParentTransform)
        {
            FactionAssociation factionAssociation = structure.GetComponent<FactionAssociation>();

            FactionSO factionSO = factionAssociation.AssociatedFactionTransform.GetComponent<ObjectStorage>().GetObject<FactionSO>();
            if (factionSO.Id.Equals(faction.Id))
            {
                result.Add(structure);
            }
        }

        return result;
    }

    public List<Transform> GetAllStructureTransforms()
    {
        List<Transform> result = new();

        foreach (Transform structure in structureParentTransform)
        {
            result.Add(structure);
        }

        return result;
    }

    public void Save(Save save)
    {
        foreach (Transform structureTransform in structureParentTransform)
        {
            FactionAssociation factionAssociation = structureTransform.GetComponent<FactionAssociation>();
            FactionSO factionSO = factionAssociation.AssociatedFactionTransform.GetComponent<ObjectStorage>().GetObject<FactionSO>();
            Inventory inventory = structureTransform.GetComponent<Inventory>();

            StructureSaveData structureSaveData = new();
            structureSaveData.Id = structureTransform.GetComponent<ObjectStorage>().GetObject<StructureSO>().Id;
            structureSaveData.GUID = structureTransform.GetComponent<GUID>().Id;
            structureSaveData.OwnedByFactionId = factionSO.Id;
            structureSaveData.Currency = structureTransform.GetComponent<CurrencyStorage>().Currency;

            // save inventory
            structureSaveData.Inventory = new InventorySaveData(inventory.Items);

            // save buildings
            BuildingStorage buildingStorage = structureTransform.GetComponent<BuildingStorage>();
            foreach (Building building in buildingStorage.Buildings)
            {
                BuildingSaveData buildingSaveData = new(building);
                structureSaveData.Buildings.Add(buildingSaveData);
            }

            save.Structures.Add(structureSaveData);
        }
    }

    public void Load(Save save)
    {
        foreach (Transform structureTransform in structureParentTransform)
        {
            StructureSO structureSO = structureTransform.GetComponent<ObjectStorage>().GetObject<StructureSO>();
            // figure out save data for this structure
            StructureSaveData structureSaveData = save.Structures.Find(x => x.Id.Equals(structureSO.Id));

            // load faction association
            FactionAssociation factionAssociation = structureTransform.GetComponent<FactionAssociation>();
            factionAssociation.AssociatedFactionTransform = factionService.GetFactionTransform(structureSaveData.OwnedByFactionId);

            // load inventory
            Inventory inventory = structureTransform.GetComponent<Inventory>();
            List<Item> items = new();
            foreach (ItemSaveData itemSaveData in structureSaveData.Inventory.Items)
            {
                Item item = new Item(itemService.GetScriptableObject(itemSaveData.Id), itemSaveData);
                items.Add(item);
            }
            inventory.SetItems(items);

            // load currency
            CurrencyStorage currencyStorage = structureTransform.GetComponent<CurrencyStorage>();
            currencyStorage.SetCurrency(structureSaveData.Currency);

            // load guid
            GUID guid = structureTransform.GetComponent<GUID>();
            guid.OverwriteId(structureSaveData.GUID);

            // load buildings
            BuildingStorage buildingStorage = structureTransform.GetComponent<BuildingStorage>();
            // clear existing buildings
            buildingStorage.Buildings.Clear();

            foreach (BuildingSaveData buildingSaveData in structureSaveData.Buildings)
            {
                BuildingSO buildingSO = buildingService.GetScriptableObject(buildingSaveData.BuildingId);
                Building building = new(buildingSO, buildingSaveData);
                buildingStorage.Buildings.Add(building);
            }
        }
    }
}
