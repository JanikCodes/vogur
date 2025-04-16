/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022 - 2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.AITree
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class KeyColorAttribute : Attribute
    {
        public readonly float r;
        public readonly float g;
        public readonly float b;
        public readonly float a;

        public KeyColorAttribute(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            a = 1.0f;
        }

        public KeyColorAttribute(float r, float g, float b, float a) : this(r, g, b)
        {
            this.a = a;
        }
    }
}
