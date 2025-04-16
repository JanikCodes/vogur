using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="RelationshipSO"/> at runtime from.
/// </summary>
[Serializable]
public class RelationshipData : BaseData
{
    public List<RelationshipConnectionData> Relationships;

    [Serializable]
    public struct RelationshipConnectionData
    {
        public string FactionId;
        public int RelationshipValue;
    }
}