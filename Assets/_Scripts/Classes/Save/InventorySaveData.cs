using System;
using System.Collections.Generic;

/// <summary>
/// Class that bundles the <see cref="ItemSaveData"/> for easier saving/loading
/// </summary>
[Serializable]
public class InventorySaveData
{
    public List<ItemSaveData> Items = new();

    public InventorySaveData()
    {
        // empty constructor for serialization
    }

    public InventorySaveData(List<Item> items)
    {
        foreach (Item item in items)
        {
            ItemSaveData newData = new ItemSaveData();
            newData.Id = item.ItemBaseData.Id;
            newData.Count = item.Count;
            newData.SlotIndex = item.SlotIndex;

            Items.Add(newData);
        }
    }

    
}