using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingSO : DataSO
{
    public string Name;
    public string Description;
    public int BuildPrice;
    public string BuildingType;
    public List<BuildingProductionItem> ProduceItems = new();
    public List<BuildingProductionSoldier> ProduceSoldiers = new();
    public int Intervall;
}
