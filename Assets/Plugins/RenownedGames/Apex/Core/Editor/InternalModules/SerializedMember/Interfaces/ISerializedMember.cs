/* ================================================================
   ----------------------------------------------------------------
   Project   :   Apex
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEditor;

namespace RenownedGames.ApexEditor
{
    public interface ISerializedMember
    {
        /// <summary>
        /// Target serialized object reference of serialized member.
        /// </summary>
        SerializedObject GetSerializedObject();
    }
}