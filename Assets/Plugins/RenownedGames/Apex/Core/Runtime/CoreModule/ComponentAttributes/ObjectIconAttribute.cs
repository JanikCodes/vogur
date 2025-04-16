/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.Apex
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ObjectIconAttribute : ApexAttribute
    {
        public readonly string path;
        public readonly bool hierarchyWindow;

        public ObjectIconAttribute(string path)
        {
            this.path = path;
        }

        //public ObjectIconAttribute(string path, bool hierarchyWindow) : this(path)
        //{
        //    this.hierarchyWindow = hierarchyWindow;
        //}
    }
}