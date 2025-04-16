/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.AITree
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class KeyTypesAttribute : Attribute
    {
        public Type[] types;

        public KeyTypesAttribute(params Type[] types)
        {
            this.types = types;
        }
    }
}