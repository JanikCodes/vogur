using System;
using System.Collections.Generic;
using static RelationshipData;

/// <summary>
/// DataContainer class for <see cref="RelationshipData"/> saving dynamic data
/// </summary>
[Serializable]
public class RelationshipSaveData
{
    public List<RelationshipConnectionData> Relationships;

    public RelationshipSaveData()
    {
        // empty constructor for serialization
    }

    public RelationshipSaveData(List<RelationshipConnectionData> relationships)
    {
        Relationships = relationships;
    }
}