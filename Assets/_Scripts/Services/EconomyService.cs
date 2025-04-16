using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This service will keep track of all item prices and will also update those prices over time. Other components can use <see cref="EconomyServiceReference"/> to check prices while trading or whatever.
/// </summary>
public class EconomyService : MonoBehaviour, ISave, ILoad
{
    [Serializable]
    public struct ItemRecord
    {
        public string ItemId;
        public int BasePrice;
        public int CurrentPrice;
    }

    [SerializeField] private List<ItemRecord> itemRecords = new();

    private ItemService itemService;

    private void Awake()
    {
        itemService = transform.parent.GetComponentInChildren<ItemService>();
    }

    public void InitEconomy()
    {
        List<ItemSO> items = itemService.GetAllScriptableObjects();

        // create record for each existing item in the game
        foreach (ItemSO item in items)
        {
            ItemRecord record = new();
            record.ItemId = item.Id;
            record.BasePrice = item.BaseValue;
            record.CurrentPrice = item.BaseValue;
            itemRecords.Add(record);
        }

        StartCoroutine(ModifyPriceDynamically());
    }

    // TODO: Turn this into a IJob to update 100+ items at once/parralel to prevent performance drops
    private IEnumerator ModifyPriceDynamically()
    {
        while (true)
        {
            int randomWait = Util.GetRandomValue(5, 5);

            for (int i = 0; i < itemRecords.Count; i++)
            {
                // Generate a random percentage change between -5% to +5%
                float percentageChange = UnityEngine.Random.Range(-5f, 5f) / 100f;

                // Calculate the new price based on the percentage change
                float newPrice = itemRecords[i].CurrentPrice * (1 + percentageChange);

                // Clamp the new price to ensure it does not exceed the base price by 20%
                float maxPrice = itemRecords[i].BasePrice * 1.2f;
                float minPrice = itemRecords[i].BasePrice * 0.8f;
                newPrice = Mathf.Clamp(newPrice, minPrice, maxPrice);

                // Update the current price in the item record
                itemRecords[i] = new ItemRecord
                {
                    ItemId = itemRecords[i].ItemId,
                    BasePrice = itemRecords[i].BasePrice,
                    CurrentPrice = Mathf.RoundToInt(newPrice)
                };
            }

            Debug.Log("Economy item prices were adjusted..");

            yield return new WaitForSeconds(randomWait);
        }
    }

    public int GetPrice(string id)
    {
        return itemRecords.Find(item => item.ItemId.Equals(id)).CurrentPrice;
    }

    public void Save(Save save)
    {
        save.Economy = itemRecords;
    }

    public void Load(Save save)
    {
        itemRecords = save.Economy;
    }
}
