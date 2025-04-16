using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactionSquadPresetService : ScriptableObjectService<FactionSquadPresetSO>
{
    private SoldierService soldierService;

    private void Awake()
    {
        soldierService = GetOtherService<SoldierService>();
    }

    public override void CreateScriptableObjects()
    {
        List<FactionSquadPresetData> rawData = DataService.CreateDataFromFilesAndMods<FactionSquadPresetData>("FactionSquadPresets");

        foreach (FactionSquadPresetData data in rawData)
        {
            if (!data.Active) { continue; }

            FactionSquadPresetSO factionSquadPreset = ScriptableObject.CreateInstance<FactionSquadPresetSO>();
            factionSquadPreset.name = data.Id;
            factionSquadPreset.Id = data.Id;
            factionSquadPreset.Soldiers = GetSoldierScriptableObjectsByArray(data.Soldiers);
            factionSquadPreset.AssignedFactionId = data.AssignedFactionId;

            scriptableObjects.Add(factionSquadPreset);
        }

        base.CreateScriptableObjects();
    }

    public List<FactionSquadPresetSO> GetFactionSquadPresets(string factionId)
    {
        return scriptableObjects.FindAll(x => x.AssignedFactionId.Equals(factionId));
    }

    private List<SoldierSO> GetSoldierScriptableObjectsByArray(string[] soldierIds)
    {
        List<SoldierSO> soldierList = new();

        foreach (string soldierId in soldierIds)
        {
            SoldierSO soldier = soldierService.GetScriptableObject(soldierId);

            if (soldier)
            {
                soldierList.Add(soldier);
            }
        }

        return soldierList;
    }
}
