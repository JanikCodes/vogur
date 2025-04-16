using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierService : ScriptableObjectService<SoldierSO>
{
    public override void CreateScriptableObjects()
    {
        List<SoldierData> rawData = DataService.CreateDataFromFilesAndMods<SoldierData>("Soldiers");

        foreach (SoldierData data in rawData)
        {
            if (!data.Active) { continue; }

            SoldierSO soldier = ScriptableObject.CreateInstance<SoldierSO>();
            soldier.name = data.Id;
            soldier.Id = data.Id;
            soldier.Name = data.Name;
            soldier.Description = data.Description;
            soldier.MovementSpeed = data.MovementSpeed;
            soldier.MaxHealth = data.MaxHealth;

            scriptableObjects.Add(soldier);
        }

        base.CreateScriptableObjects();
    }
}
