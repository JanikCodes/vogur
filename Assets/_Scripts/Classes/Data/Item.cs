using System;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="ItemSO"/> at runtime from.
/// </summary>
[Serializable]
public class ItemData : BaseData
{
    public string Name;
    public string Description;
    public string ItemType;
    public int BaseValue;
    public int MaxStackSize;
    public string Rarity;
    public int FoodValue;
}

[Serializable]
public class Item
{
    public ItemSO ItemBaseData;

    [Header("Dynamic Data")]
    public int Count;
    public int SlotIndex;

    public Item()
    {
        // empty constructor for serialization
    }

    public Item(ItemSO data, ItemSaveData save)
    {
        ItemBaseData = data;
        Count = save.Count;
        SlotIndex = save.SlotIndex;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}