/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.ApexEditor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class DrawerTarget : Attribute
    {
        public readonly Type type;

        public DrawerTarget(Type type)
        {
            this.type = type;
        }

        #region [Optional Parameters]
        public bool Subclasses { get; set; }
        #endregion
    }
}