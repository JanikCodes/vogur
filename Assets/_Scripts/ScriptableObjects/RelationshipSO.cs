using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RelationshipData;

[Serializable]
public class RelationshipSO : DataSO
{
    public List<RelationshipConnectionData> Relationships;
}
