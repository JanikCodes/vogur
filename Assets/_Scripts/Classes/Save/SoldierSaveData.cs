using System;
using System.Collections.Generic;

/// <summary>
/// DataContainer class dependency for <see cref="SquadSaveData"/> saving dynamic data
/// </summary>
[Serializable]
public class SoldierSaveData
{
    public string Id;
    public string Name;
    public int Health;
    public int Moral;
}