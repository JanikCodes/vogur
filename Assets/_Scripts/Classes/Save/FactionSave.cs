using System;
using System.Collections.Generic;

/// <summary>
/// DataContainer class for <see cref="FactionData"/> saving dynamic data
/// </summary>
[Serializable]
public class FactionSaveData
{
    public string Id;
    public List<ArmySaveData> Armies = new();
    public RelationshipSaveData Relationships;
}