/* ================================================================
   ----------------------------------------------------------------
   Project   :   Apex
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System.Reflection;

namespace RenownedGames.ApexEditor
{
    public interface IMemberInfo
    {
        /// <summary>
        /// Member info of serialized member.
        /// </summary>
        MemberInfo GetMemberInfo();
    }
}