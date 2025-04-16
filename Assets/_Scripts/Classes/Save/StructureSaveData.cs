using System;
using System.Collections.Generic;

/// <summary>
/// DataContainer class for <see cref="StructureData"/> saving dynamic data
/// </summary>
[Serializable]
public class StructureSaveData
{
    public string GUID;
    public string Id;
    public string OwnedByFactionId;
    public int Currency;
    public InventorySaveData Inventory;
    public List<BuildingSaveData> Buildings = new();
}