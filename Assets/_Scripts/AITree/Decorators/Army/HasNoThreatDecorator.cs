using System;
using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

/// <summary>
/// This decorator can be used to stop the army from doing innocent tasks while for example it found a nearby hostile army.
/// </summary>
[NodeContent(name: "Has No Threat", path: "Base/Army/Has No Threat", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class HasNoThreatDecorator : ObserverDecorator
{
    [Header("Variables")]
    [SerializeField]
    private TransformKey transform;

    // Stored required components.
    private FactionAssociation factionAssociation;

    public override event Action OnValueChange;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        factionAssociation = GetOwner().GetComponent<FactionAssociation>();

        if (transform != null)
        {
            transform.ValueChanged += OnValueChange;
        }
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override void OnFlowUpdate() { }

    public override bool CalculateResult()
    {
        if (transform.GetValue() == null)
        {
            return true;
        }

        // get own factionRelationship
        FactionRelationship factionRelationship = factionAssociation.AssociatedFactionTransform.GetComponent<FactionRelationship>();

        // get opposite faction transform
        FactionAssociation transformFactionAssociation = transform.GetValue().GetComponent<FactionAssociation>();
        if (!transformFactionAssociation)
        {
            return true;
        }
        Transform factionRootTransform = transformFactionAssociation.AssociatedFactionTransform;
        // get opposite faction id
        string transformFactionId = factionRootTransform.GetComponent<ObjectStorage>().GetObject<FactionSO>().Id;

        // check relationship type
        RelationshipType relationshipType = factionRelationship.GetRelationshipTypeTowardsFaction(transformFactionId);

        // if relation is hostile, abort children nodes
        if (relationshipType.Equals(RelationshipType.Hostile))
        {
            return false;
        }

        return true;
    }
}
