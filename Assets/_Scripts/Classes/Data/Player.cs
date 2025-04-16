using System;

/// <summary>
/// Class that is used to generate the ScriptableObject <see cref="PlayerSO"/> at runtime from.
/// </summary>
[Serializable]
public class PlayerData : BaseData
{
    public string InitiallyOwnedByFaction;
    public float[] SpawnPosition;
}