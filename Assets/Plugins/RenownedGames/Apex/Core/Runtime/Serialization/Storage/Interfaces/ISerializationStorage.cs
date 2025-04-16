/* ================================================================
   ----------------------------------------------------------------
   Project   :   Apex
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.Apex.Serialization.Collections.Generic
{
    public interface ISerializationStorage<T>
    {
        T GetStorageData();
    }
}