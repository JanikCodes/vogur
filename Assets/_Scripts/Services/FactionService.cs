using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FactionService : ScriptableObjectService<FactionSO>, ISave, ILoad
{
    public static OnNewArmySpawnedDelegate OnArmySpawned;
    public delegate void OnNewArmySpawnedDelegate(Transform armyTransform, string factionId);

    [field: SerializeField] public Transform FactionParentTransform { get; private set; }
    [field: SerializeField] public Transform ArmyParentTransform { get; private set; }
    [field: SerializeField] public GameObject FactionRootPrefab { get; private set; }
    [field: SerializeField] public GameObject ArmyRootPrefab { get; private set; }

    private StructureService structureService;
    private FactionSquadPresetService factionSquadPresetService;
    private RelationshipService relationshipService;
    private EconomyService economyService;
    private SoldierService soldierService;
    private ItemService itemService;
    private DialogueService dialogueService;

    private void Awake()
    {
        structureService = GetOtherService<StructureService>();
        factionSquadPresetService = GetOtherService<FactionSquadPresetService>();
        relationshipService = GetOtherService<RelationshipService>();
        economyService = GetOtherService<EconomyService>();
        soldierService = GetOtherService<SoldierService>();
        itemService = GetOtherService<ItemService>();
        dialogueService = GetOtherService<DialogueService>();
    }

    public override void CreateScriptableObjects()
    {
        List<FactionData> rawData = DataService.CreateDataFromFilesAndMods<FactionData>("Factions");

        foreach (FactionData data in rawData)
        {
            if (!data.Active) { continue; }

            FactionSO faction = ScriptableObject.CreateInstance<FactionSO>();
            faction.name = data.Id;
            faction.Id = data.Id;
            faction.Name = data.Name;
            faction.Visible = data.Visible;
            faction.Description = data.Description;
            faction.MaxArmyCountOnOverworld = data.MaxArmyCountOnOverworld;
            faction.StartCurrencyAmount = data.StartCurrencyAmount;
            faction.MinSquadAmount = data.MinSquadAmount;
            faction.MaxSquadAmount = data.MaxSquadAmount;
            faction.FactionArmySpawnType = Util.ReturnEnumValuesFromStringValues<FactionArmySpawnType>(data.FactionArmySpawnType);
            faction.SquadPresets = factionSquadPresetService.GetFactionSquadPresets(data.Id);
            faction.AssignedArmyDialogue = dialogueService.GetScriptableObject(data.AssignedArmyDialogueId);

            scriptableObjects.Add(faction);
        }

        base.CreateScriptableObjects();
    }

    public void CreateFactionObjects()
    {
        foreach (FactionSO data in scriptableObjects)
        {
            GameObject obj = Instantiate(FactionRootPrefab, FactionParentTransform);
            obj.name = data.Name;

            ObjectStorage objectStorage = obj.GetComponent<ObjectStorage>();
            objectStorage.SetObject<FactionSO>(data);

            FactionServiceReference factionServiceReference = obj.GetComponent<FactionServiceReference>();
            factionServiceReference.Service = this;

            StructureServiceReference structureServiceReference = obj.GetComponent<StructureServiceReference>();
            structureServiceReference.Service = structureService;

            CurrencyStorage currencyStorage = obj.GetComponent<CurrencyStorage>();
            currencyStorage.ModifyCurrency(data.StartCurrencyAmount);

            FactionRelationship factionRelationship = obj.GetComponent<FactionRelationship>();
            factionRelationship.Relationships = relationshipService.GetScriptableObject(data.Id).Relationships;
        }
    }

    public Transform GetFactionTransform(string factionId)
    {
        foreach (Transform faction in FactionParentTransform)
        {
            if (faction.GetComponent<ObjectStorage>().GetObject<FactionSO>().Id.Equals(factionId))
            {
                return faction;
            }
        }

        Debug.LogError("Couldn't find faction transform where objectStorage FactionSO has ID: " + factionId);

        return null;
    }

    public GameObject CreateAndSpawnArmy(Vector3 rawPosition, string factionId, GameObject prefab = null)
    {
        // do we take the default prefab or the optional?
        GameObject selectedPrefab = prefab ? prefab : ArmyRootPrefab;
        // get faction transform based on given factionId
        Transform factionTransform = GetFactionTransform(factionId);

        FactionSO factionSO = factionTransform.GetComponent<ObjectStorage>().GetObject<FactionSO>();

        GameObject army = Instantiate(selectedPrefab, ArmyParentTransform);
        army.transform.position = Util.CalculatePositionOnTerrain(rawPosition);

        // populate components ( when getting components we need to use GetComponentInChildren() due to the possibility of PlayerStructure or maybe create a specific component that points to the armyRoot of the playerStructure )
        FactionAssociation factionAssociation = army.GetComponentInChildren<FactionAssociation>();
        factionAssociation.AssociatedFactionTransform = factionTransform;

        EconomyServiceReference economyServiceReference = army.GetComponentInChildren<EconomyServiceReference>();
        economyServiceReference.Service = economyService;

        OnArmySpawned?.Invoke(army.transform, factionId);

        return army;
    }

    public Transform GetArmyTransform(string guid)
    {
        foreach (Transform army in ArmyParentTransform)
        {
            GUID armyGUID = army.GetComponentInChildren<GUID>();
            if (armyGUID == null) { continue; }

            if (armyGUID.Id.Equals(guid))
            {
                return army;
            }
        }

        Debug.LogError("Couldn't find army transform where GUID Id is: " + guid);

        return null;
    }

    public void Save(Save save)
    {
        foreach (Transform faction in FactionParentTransform)
        {
            FactionRelationship factionRelationship = faction.GetComponent<FactionRelationship>();

            FactionSaveData factionSaveData = new();
            factionSaveData.Id = faction.GetComponent<ObjectStorage>().GetObject<FactionSO>().Id;
            factionSaveData.Relationships = new RelationshipSaveData(factionRelationship.Relationships);

            FactionArmyReference factionArmyReference = faction.GetComponent<FactionArmyReference>();
            foreach (Transform armyTransform in factionArmyReference.ReferencedArmies)
            {
                // ignore the player to be saved here, we save him seperately
                if (armyTransform.GetComponentInChildren<PlayerNearbyEmitter>() != null) { continue; }

                SquadStorage squadStorage = armyTransform.GetComponent<SquadStorage>();
                Inventory inventory = armyTransform.GetComponent<Inventory>();
                CurrencyStorage currencyStorage = armyTransform.GetComponent<CurrencyStorage>();
                ArmyDialogueHandler armyDialogueHandler = armyTransform.GetComponent<ArmyDialogueHandler>();

                ArmySaveData armySaveData = new();
                armySaveData.GUID = armyTransform.GetComponent<GUID>().Id;
                armySaveData.Currency = currencyStorage.Currency;
                armySaveData.Position = Util.GetFloatArray(armyTransform.transform.position);
                armySaveData.Rotation = Util.GetFloatArray(armyTransform.transform.rotation);

                // save army dialogue
                if (armyDialogueHandler.IsInDialogue())
                {
                    armySaveData.DialogueActive = true;
                    armySaveData.DialogueType = armyDialogueHandler.dialogueType.ToString();
                    // save dialogue partner guid
                    GUID otherGUID = armyDialogueHandler.other.GetComponent<GUID>();
                    if (otherGUID)
                    {
                        armySaveData.DialogueOtherGUID = otherGUID.Id;
                    }
                }

                // save squads with soldiers
                foreach (Squad squad in squadStorage.Squads)
                {
                    SquadSaveData squadSaveData = new(squad.GetSoldiers());
                    armySaveData.Squads.Add(squadSaveData);
                }

                // save inventory
                armySaveData.Inventory = new InventorySaveData(inventory.Items);

                factionSaveData.Armies.Add(armySaveData);
            }

            save.Factions.Add(factionSaveData);
        }
    }

    public void Load(Save save)
    {
        // clean up armies
        foreach (Transform armyTransform in ArmyParentTransform)
        {
            Destroy(armyTransform.gameObject);
        }

        foreach (Transform faction in FactionParentTransform)
        {
            FactionSO factionSO = faction.GetComponent<ObjectStorage>().GetObject<FactionSO>();
            // figure out save data for this faction
            FactionSaveData factionSaveData = save.Factions.Find(x => x.Id.Equals(factionSO.Id));

            // load relationships
            FactionRelationship factionRelationship = faction.GetComponent<FactionRelationship>();
            factionRelationship.Relationships = factionSaveData.Relationships.Relationships;

            FactionArmyReference factionArmyReference = faction.GetComponent<FactionArmyReference>();
            factionArmyReference.ReferencedArmies = new();

            foreach (ArmySaveData armySaveData in factionSaveData.Armies)
            {
                Vector3 position = Util.GetVector3FromFloatArray(armySaveData.Position);

                // load root and position
                GameObject armyRoot = CreateAndSpawnArmy(position, factionSO.Id);

                // load currency
                CurrencyStorage currencyStorage = armyRoot.GetComponent<CurrencyStorage>();
                currencyStorage.SetCurrency(armySaveData.Currency);

                // load guid
                GUID guid = armyRoot.GetComponent<GUID>();
                guid.OverwriteId(armySaveData.GUID);

                // load squads & soldiers
                SquadStorage squadStorage = armyRoot.GetComponent<SquadStorage>();
                foreach (SquadSaveData squadSaveData in armySaveData.Squads)
                {
                    Squad squad = new();

                    foreach (SoldierSaveData soldierSaveData in squadSaveData.Soldiers)
                    {
                        SoldierSO soldierSO = soldierService.GetScriptableObject(soldierSaveData.Id);
                        Soldier soldier = new Soldier(soldierSO, soldierSaveData);
                        squad.AddSoldier(soldier);
                    }

                    squadStorage.AddSquad(squad);
                }

                // load inventory
                List<Item> items = new();
                Inventory inventory = armyRoot.GetComponent<Inventory>();

                foreach (ItemSaveData itemSaveData in armySaveData.Inventory.Items)
                {
                    Item item = new Item(itemService.GetScriptableObject(itemSaveData.Id), itemSaveData);
                    items.Add(item);
                }

                inventory.SetItems(items);
            }
        }

        // load the dialogue for each army in each faction after everything basic has been loaded
        foreach (FactionSaveData factionSave in save.Factions)
        {
            foreach (ArmySaveData armySaveData in factionSave.Armies)
            {
                // is army not in dialogue?
                if (!armySaveData.DialogueActive) { continue; }

                // find armySaveData transform
                Transform armyTransform = GetArmyTransform(armySaveData.GUID);
                if (armyTransform == null) { continue; }

                // find transform based on guid
                Transform other = GetArmyTransform(armySaveData.DialogueOtherGUID);
                if (other == null) { continue; }

                ArmyDialogueHandler armyDialogueHandler = armyTransform.GetComponent<ArmyDialogueHandler>();
                if (armyDialogueHandler == null) { continue; }

                DialogueType dialogueType = Util.ReturnEnumValueFromStringValue<DialogueType>(armySaveData.DialogueType);

                armyDialogueHandler.TalkTo(other, dialogueType);
            }
        }

    }
}
