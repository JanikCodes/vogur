/* ================================================================
   ----------------------------------------------------------------
   Project   :   Apex
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.ApexEditor
{
    public interface IMemberDeclaringObject
    {
        /// <summary>
        /// Declaring object of serialized member.
        /// </summary>
        object GetDeclaringObject();
    }
}