using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService : ScriptableObjectService<PlayerSO>, ISave, ILoad
{
    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;

    private FactionService factionService;
    private EconomyService economyService;
    private SoldierService soldierService;
    private ItemService itemService;
    private QuestService questService;
    private TimeService timeService;

    private GameObject playerReference;
    private const string PLAYER_ROOT_NAME = "PlayerRoot";

    private void Awake()
    {
        factionService = GetOtherService<FactionService>();
        economyService = GetOtherService<EconomyService>();
        soldierService = GetOtherService<SoldierService>();
        itemService = GetOtherService<ItemService>();
        questService = GetOtherService<QuestService>();
        timeService = GetOtherService<TimeService>();
    }

    public override void CreateScriptableObjects()
    {
        List<PlayerData> rawData = DataService.CreateDataFromFilesAndMods<PlayerData>("Player");

        foreach (PlayerData data in rawData)
        {
            if (!data.Active) { continue; }

            PlayerSO player = ScriptableObject.CreateInstance<PlayerSO>();
            player.name = data.Id;
            player.Id = data.Id;
            player.InitiallyOwnedByFaction = data.InitiallyOwnedByFaction;
            player.SpawnPosition = new Vector3(data.SpawnPosition[0], data.SpawnPosition[1], data.SpawnPosition[2]);

            scriptableObjects.Add(player);
        }

        base.CreateScriptableObjects();
    }

    public void SpawnPlayerIntoWorld()
    {
        PlayerSO playerData = scriptableObjects[0];
        playerReference = factionService.CreateAndSpawnArmy(playerData.SpawnPosition, playerData.InitiallyOwnedByFaction, playerPrefab);

        EconomyServiceReference economyServiceReference = playerReference.GetComponentInChildren<EconomyServiceReference>();
        economyServiceReference.Service = economyService;

        TimeServiceReference timeServiceReference = playerReference.GetComponentInChildren<TimeServiceReference>();
        timeServiceReference.Service = timeService;

        QuestStorage questStorage = playerReference.GetComponentInChildren<QuestStorage>();
        questStorage.InstantiateQuests(questService.GetAllScriptableObjects());
    }

    public void Save(Save save)
    {
        Transform playerRoot = playerReference.transform.Find(PLAYER_ROOT_NAME);
        GUID guid = playerRoot.GetComponent<GUID>();
        FactionAssociation factionAssociation = playerRoot.GetComponent<FactionAssociation>();
        FactionSO factionSO = factionAssociation.AssociatedFactionTransform.GetComponent<ObjectStorage>().GetObject<FactionSO>();
        CurrencyStorage currencyStorage = playerRoot.GetComponent<CurrencyStorage>();
        SquadStorage squadStorage = playerRoot.GetComponent<SquadStorage>();
        Inventory inventory = playerRoot.GetComponent<Inventory>();
        QuestStorage questStorage = playerRoot.GetComponent<QuestStorage>();

        PlayerSaveData playerSaveData = new();
        playerSaveData.GUID = guid.Id;
        playerSaveData.OwnedByFactionId = factionSO.Id;
        playerSaveData.Position = Util.GetFloatArray(playerRoot.position);
        playerSaveData.Rotation = Util.GetFloatArray(playerRoot.rotation);
        playerSaveData.Currency = currencyStorage.Currency;

        // save squads with soldiers
        foreach (Squad squad in squadStorage.Squads)
        {
            SquadSaveData squadSaveData = new(squad.GetSoldiers());
            playerSaveData.Squads.Add(squadSaveData);
        }

        // save inventory
        playerSaveData.Inventory = new InventorySaveData(inventory.Items);

        // save quest progress
        foreach (Quest quest in questStorage.Quests)
        {
            QuestSaveData questSaveData = new(quest);
            playerSaveData.Quests.Add(questSaveData);
        }

        save.Player = playerSaveData;
    }

    public void Load(Save save)
    {
        SpawnPlayerIntoWorld();

        PlayerSaveData playerSaveData = save.Player;
        Transform playerRoot = playerReference.transform.Find(PLAYER_ROOT_NAME);

        // load position & rotation
        playerRoot.position = Util.GetVector3FromFloatArray(playerSaveData.Position);
        playerRoot.rotation = Util.GetQuaternionFromFloatArray(playerSaveData.Rotation);

        // load currency
        CurrencyStorage currencyStorage = playerRoot.GetComponent<CurrencyStorage>();
        currencyStorage.SetCurrency(playerSaveData.Currency);

        // load guid
        GUID guid = playerRoot.GetComponent<GUID>();
        guid.OverwriteId(playerSaveData.GUID);

        // load squads & soldiers
        SquadStorage squadStorage = playerRoot.GetComponent<SquadStorage>();
        foreach (SquadSaveData squadSaveData in playerSaveData.Squads)
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
        Inventory inventory = playerRoot.GetComponent<Inventory>();

        foreach (ItemSaveData itemSaveData in playerSaveData.Inventory.Items)
        {
            Item item = new Item(itemService.GetScriptableObject(itemSaveData.Id), itemSaveData);
            items.Add(item);
        }

        inventory.SetItems(items);

        // load quest progress
        QuestStorage questStorage = playerRoot.GetComponent<QuestStorage>();
        foreach (QuestSaveData questSaveData in playerSaveData.Quests)
        {
            questStorage.LoadQuestProgress(questSaveData);
        }
    }
}
