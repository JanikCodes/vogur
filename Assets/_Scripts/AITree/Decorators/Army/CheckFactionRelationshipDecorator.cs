using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RenownedGames.AITree;
using UnityEngine;

[NodeContent(name: "Check Faction Relationship", path: "Base/Generic/Check Faction Relationship", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class CheckFactionRelationshipDecorator : ConditionDecorator
{
    [Header("Variables")]
    [SerializeField]
    private TransformKey transform;

    [SerializeField]
    private RelationshipType[] targetType;

    // Stored required components.
    private FactionAssociation factionAssociation;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        factionAssociation = GetOwner().GetComponent<FactionAssociation>();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override bool CalculateResult()
    {
        if(!transform.GetValue()) 
        {
            return false;
        }
        
        // get own factionRelationship
        FactionRelationship factionRelationship = factionAssociation.AssociatedFactionTransform.GetComponent<FactionRelationship>();

        // get opposite faction transform
        FactionAssociation transformFactionAssociation = transform.GetValue().GetComponent<FactionAssociation>();
        if (!transformFactionAssociation)
        {
            return false;
        }
        Transform factionRootTransform = transformFactionAssociation.AssociatedFactionTransform;
        // get opposite faction id
        string transformFactionId = factionRootTransform.GetComponent<ObjectStorage>().GetObject<FactionSO>().Id;

        // check relationship type
        RelationshipType relationshipType = factionRelationship.GetRelationshipTypeTowardsFaction(transformFactionId);

        // does type exist in targetType
        return targetType.Any(target => target.Equals(relationshipType));
    }

    protected override void OnFlowUpdate() { }
}