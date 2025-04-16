/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.AITree
{
    [System.Obsolete]
    public interface IKeyCastable
    {
        /// <summary>
        /// Try cast undefined generic value to specified type.
        /// </summary>
        /// <typeparam name="T">Desired type.</typeparam>
        /// <param name="value">Output reference of value.</param>
        /// <returns>True if value has been casted. Otherwise false.</returns>
        bool TryCastValueTo<T>(out T value);

        /// <summary>
        /// Try to cast the key to the specified type, taking into account the name.
        /// </summary>
        /// <typeparam name="TKey">Specified key type.</typeparam>
        /// <param name="name">Key name.</param>
        /// <param name="value">Output specified key.</param>
        /// <returns>True if value has been casted. Otherwise false</returns>
        bool TryCastByName<TKey>(string name, out TKey key) where TKey : Key;
    }
}