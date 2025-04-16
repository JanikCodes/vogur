/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.AITree
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class HideSelfKeyAttribute : Attribute { }
}