using System;
using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Get Closest Army", "Base/Army/Get Closest Army", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class GetClosestArmyService : IntervalServiceNode
{
    [Header("Variables")]
    [SerializeField]
    private FloatKey sightRadius;

    [SerializeField]
    private LayerMask scanLayer;

    [SerializeField]
    [NonLocal]
    private TransformKey outputClosestTransform;

    // Stored required components.
    private Transform transform;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        transform = GetOwner().transform;
    }

    protected override void OnTick()
    {
        float currentClosestRange = Mathf.Infinity;
        Collider[] armyColliders = new Collider[10];
        Transform currentClosestTransform = null;

        int foundArmies = Physics.OverlapSphereNonAlloc(transform.position, sightRadius.GetValue(), armyColliders, scanLayer);

        for (int i = 0; i < foundArmies; i++)
        {
            // ignore itself
            if (transform.Equals(armyColliders[i].transform)) { continue; }

            // ignore armies that are in dialogue
            IDialogueHandler dialogueHandler = armyColliders[i].GetComponent<IDialogueHandler>();
            if (dialogueHandler == null) { continue; }
            if (dialogueHandler.IsInDialogue()) { continue; }

            // ignore armies that are immune to dialogue
            DialogueImmunity dialogueImmunity = armyColliders[i].GetComponent<DialogueImmunity>();
            if (dialogueImmunity == null) { continue; }
            if (dialogueImmunity.Immune) { continue; }

            float distanceSqr = (transform.position - armyColliders[i].transform.position).sqrMagnitude;
            if (distanceSqr < currentClosestRange)
            {
                currentClosestRange = distanceSqr;
                currentClosestTransform = armyColliders[i].transform;
            }
        }

        outputClosestTransform.SetValue(currentClosestTransform);
    }
}