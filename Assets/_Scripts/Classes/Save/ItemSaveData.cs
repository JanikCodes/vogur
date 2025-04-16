using System;
using System.Collections.Generic;

/// <summary>
/// DataContainer class dependency for <see cref="InventorySaveData"/> saving dynamic data
/// </summary>
[Serializable]
public class ItemSaveData
{
    public string Id;
    public int Count;
    public int SlotIndex;
}