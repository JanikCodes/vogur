using System.Collections;
using System.Collections.Generic;
using RenownedGames.AITree;
using UnityEngine;

[KeyColor(0.631f, 1.0f, 0.27f)]
public class FactionArmySpawnTypeKey : Key<FactionArmySpawnType>
{
    public override bool Equals(FactionArmySpawnType other)
    {
        return GetValue() == other;
    }
}