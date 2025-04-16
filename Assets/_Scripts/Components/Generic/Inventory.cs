using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour, ISpeedPenalty
{
    public event Action SpeedPenaltyChanged;

    [SerializeField] private int inventoryMaxSize = 54;

    [field: SerializeField] public List<Item> Items { get; private set; }

    private const float SPEED_PENALTY_PER_ITEM = 0.01f;

    private void Awake()
    {
        Items = new();
    }

    /// <summary>
    /// Completely re-set the inventory. Also clears the previous items.
    /// </summary>
    public void SetItems(List<Item> itemData)
    {
        // clears previous items
        Items.Clear();

        foreach (Item item in itemData)
        {
            AddItem(item);
        }
    }

    /// <summary>
    /// Automatically add a new item to the inventory, taking <see cref="Item.Count"/> and <see cref="Item.MaxStackSize"/> into account.
    /// It also finds the first empty slot and assigns it.
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
        bool alreadyInInventory = false;
        int remainingAmount = item.Count;

        if (item.ItemBaseData.MaxStackSize.Equals(1))
        {
            Item newItem = (Item)item.Clone();
            newItem.SlotIndex = FindFirstEmptySlotIndex();
            Items.Add(newItem);

            SpeedPenaltyChanged?.Invoke();

            return;
        }

        foreach (Item itemData in Items)
        {
            if (itemData.ItemBaseData.Equals(item.ItemBaseData))
            {

                if (itemData.Count + remainingAmount > itemData.ItemBaseData.MaxStackSize)
                {
                    int addingAmount = itemData.ItemBaseData.MaxStackSize - itemData.Count;
                    itemData.Count += addingAmount;
                    remainingAmount -= addingAmount;
                }
                else if (itemData.Count + remainingAmount <= itemData.ItemBaseData.MaxStackSize)
                {
                    itemData.Count += remainingAmount;
                    remainingAmount = 0;
                    alreadyInInventory = true;
                }
            }
        }

        if (!alreadyInInventory)
        {
            int cycleAmount = remainingAmount / item.ItemBaseData.MaxStackSize;

            for (int i = 0; i < cycleAmount; i++)
            {
                Item newItem = (Item)item.Clone();
                newItem.SlotIndex = FindFirstEmptySlotIndex();
                newItem.Count = item.ItemBaseData.MaxStackSize;
                Items.Add(newItem);

                remainingAmount -= newItem.ItemBaseData.MaxStackSize;
            }

            if (remainingAmount > 0)
            {
                Item newItem = (Item)item.Clone();
                newItem.SlotIndex = FindFirstEmptySlotIndex();
                newItem.Count = remainingAmount;
                Items.Add(newItem);
            }
        }

        SpeedPenaltyChanged?.Invoke();
    }

    private int FindFirstEmptySlotIndex()
    {
        for (int i = 0; i < inventoryMaxSize; i++)
        {
            if (!Items.Any(x => x.SlotIndex.Equals(i)))
            {
                return i;
            }
        }

        Debug.LogError("Couldn't find a valid empty slot index!");
        return -99;
    }

    public float GetSpeedPenalty()
    {
        return Items.Count * SPEED_PENALTY_PER_ITEM;
    }
}
