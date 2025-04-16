using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionArmyReference : MonoBehaviour
{
    [field: SerializeField] public List<Transform> ReferencedArmies { get; set; }

    private ObjectStorage objectStorage;

    private void Awake()
    {
        objectStorage = GetComponent<ObjectStorage>();

        ReferencedArmies = new();
    }

    private void OnEnable()
    {
        FactionService.OnArmySpawned += ArmySpawnedHandler;
        RemoveFromFactionArmyReferenceTask.OnArmyDestroyed += ArmyDestroyedHandler;

    }

    private void OnDisable()
    {
        FactionService.OnArmySpawned -= ArmySpawnedHandler;
        RemoveFromFactionArmyReferenceTask.OnArmyDestroyed -= ArmyDestroyedHandler;
    }

    private void ArmySpawnedHandler(Transform armyTransform, string factionId)
    {
        if (objectStorage.GetObject<FactionSO>().Id.Equals(factionId))
        {
            ReferencedArmies.Add(armyTransform);

            Debug.Log("Added new army reference to referencedArmies");
        }
    }

    private void ArmyDestroyedHandler(Transform armyTransform, string factionId)
    {
        if (objectStorage.GetObject<FactionSO>().Id.Equals(factionId))
        {
            ReferencedArmies.Remove(armyTransform);

            Debug.Log("Removed army reference from referencedArmies");
        }
    }
}
