/* ================================================================
   ----------------------------------------------------------------
   Project   :   Apex
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.ApexEditor
{
    public interface IMemberType
    {
        /// <summary>
        /// Type of serialized member.
        /// </summary>
        Type GetMemberType();
    }
}