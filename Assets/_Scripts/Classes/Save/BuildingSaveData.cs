using System;

/// <summary>
/// DataContainer class for <see cref="Building"/> saving dynamic data
/// </summary>
[Serializable]
public class BuildingSaveData
{
    public string BuildingId;
    public int BuildingProgress;

    public BuildingSaveData()
    {
        // empty constructor for serialization
    }

    public BuildingSaveData(Building building)
    {
        BuildingId = building.BuildingBaseData.Id;
        BuildingProgress = building.BuildingProgress;
    }
}