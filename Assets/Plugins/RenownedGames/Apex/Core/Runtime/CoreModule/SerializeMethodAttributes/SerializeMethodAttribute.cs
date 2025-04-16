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
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [Obsolete("Use the [Button] attributes instead. [SerializeMethod] will be removed in future versions.")]
    public sealed class SerializeMethodAttribute : ApexAttribute { }
}