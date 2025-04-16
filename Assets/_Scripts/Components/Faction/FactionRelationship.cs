using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RelationshipData;

public class FactionRelationship : MonoBehaviour
{
    [field: SerializeField] public List<RelationshipConnectionData> Relationships { get; set; }

    public void AlterRelationshipWithFaction(string factionId, int relationValue)
    {
        RelationshipConnectionData found = Relationships.Find(x => x.FactionId.Equals(factionId));
        found.RelationshipValue += relationValue;
    }

    public int GetRelationshipValueTowardsFaction(string factionId)
    {
        return Relationships.Find(x => x.FactionId.Equals(factionId)).RelationshipValue;
    }

    public RelationshipType GetRelationshipTypeTowardsFaction(string factionId)
    {
        int value = GetRelationshipValueTowardsFaction(factionId);

        if (value > 25)
        {
            return RelationshipType.Friendly;
        }
        else if (value <= 25 && value > -25)
        {
            return RelationshipType.Neutral;
        }
        else
        {
            return RelationshipType.Hostile;
        }
    }
}
