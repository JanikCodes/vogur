/* ================================================================
   ----------------------------------------------------------------
   Project   :   Apex
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.Apex.Serialization.Collections.Generic
{
    /// <summary>
    /// Represents base class for all serialized LinkedList(T) collections.
    /// </summary>
    public abstract class SerializableLinkedListBase : ISerializationCallbackReceiver
    {
        /// <summary>
        /// Called after engine deserializes this object.
        /// 
        /// Implement this method to receive a callback after engine deserializes this object.
        /// </summary>
        public abstract void OnAfterDeserialize();

        /// <summary>
        /// Called before engine serializes this object.
        /// 
        /// Implement this method to receive a callback before engine serializes this object.
        /// </summary>
        public abstract void OnBeforeSerialize();
    }
}