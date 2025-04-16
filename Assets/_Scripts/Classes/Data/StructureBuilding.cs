using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="StructureBuildingSO"/> at runtime from.
/// </summary>
[Serializable]
public class StructureBuildingData : BaseData
{
    public string StructureId;
    public string BuildingId;
    public bool AutoBuildAtStart;
}