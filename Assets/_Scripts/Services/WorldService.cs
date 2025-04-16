using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldService : MonoBehaviour
{
    private CalenderService calenderService;
    private DialogueService dialogueService;
    private ItemService itemService;
    private EconomyService economyService;
    private RelationshipService relationshipService;
    private SoldierService soldierService;
    private BuildingService buildingService;
    private FactionSquadPresetService factionSquadPresetService;
    private FactionService factionService;
    private QuestService questService;
    private PlayerService playerService;
    private StructureBuildingService structureBuildingService;
    private StructureService structureService;

    private void Start()
    {
        calenderService = GetComponentInChildren<CalenderService>();
        dialogueService = GetComponentInChildren<DialogueService>();
        itemService = GetComponentInChildren<ItemService>();
        economyService = GetComponentInChildren<EconomyService>();
        relationshipService = GetComponentInChildren<RelationshipService>();
        soldierService = GetComponentInChildren<SoldierService>();
        buildingService = GetComponentInChildren<BuildingService>();
        factionSquadPresetService = GetComponentInChildren<FactionSquadPresetService>();
        factionService = GetComponentInChildren<FactionService>();
        questService = GetComponentInChildren<QuestService>();
        playerService = GetComponentInChildren<PlayerService>();
        structureBuildingService = GetComponentInChildren<StructureBuildingService>();
        structureService = GetComponentInChildren<StructureService>();

        // created neccesary file directories on startup
        FileService.CreateDirectories();

        calenderService.CreateScriptableObjects();
        calenderService.InstantiateDateTime();

        dialogueService.CreateScriptableObjects();

        itemService.CreateScriptableObjects();

        economyService.InitEconomy();

        relationshipService.CreateScriptableObjects();

        soldierService.CreateScriptableObjects();

        buildingService.CreateScriptableObjects();

        factionSquadPresetService.CreateScriptableObjects();

        factionService.CreateScriptableObjects();
        factionService.CreateFactionObjects();

        questService.CreateScriptableObjects();

        playerService.CreateScriptableObjects();
        playerService.SpawnPlayerIntoWorld();

        structureBuildingService.CreateScriptableObjects();

        structureService.CreateScriptableObjects();
        structureService.CreateStructureObjects();

        if (SaveService.Instance)
        {
            // cache saveable objects
            SaveService.Instance.CacheAllSaveObjects();
            SaveService.Instance.CacheAllLoadObjects();
        }
        else
        {
            Debug.LogWarning("Loaded wrong scene! You're playing without the usage of the SaveService!");
        }
    }
}
