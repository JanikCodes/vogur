/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEditor;

namespace RenownedGames.ApexEditor
{
    public interface ISerializedField
    {
        /// <summary>
        /// Target serialized property of serialized field.
        /// </summary>
        SerializedProperty GetSerializedProperty();
    }
}