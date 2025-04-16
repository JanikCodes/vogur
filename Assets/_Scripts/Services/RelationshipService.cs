using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static RelationshipData;

public class RelationshipService : ScriptableObjectService<RelationshipSO>
{
    public override void CreateScriptableObjects()
    {
        List<RelationshipData> rawData = DataService.CreateDataFromFilesAndMods<RelationshipData>("Relationships");

        foreach (RelationshipData data in rawData)
        {
            if (!data.Active) { continue; }

            RelationshipSO relationship = ScriptableObject.CreateInstance<RelationshipSO>();
            relationship.name = data.Id;
            relationship.Id = data.Id;
            relationship.Relationships = data.Relationships.ToList();
            scriptableObjects.Add(relationship);
        }

        base.CreateScriptableObjects();
    }
}
