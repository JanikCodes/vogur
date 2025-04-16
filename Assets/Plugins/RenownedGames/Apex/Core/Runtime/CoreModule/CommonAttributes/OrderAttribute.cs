/* ================================================================
  ----------------------------------------------------------------
  Project   :   Apex
  Publisher :   Renowned Games
  Developer :   Tamerlan Shakirov
  ----------------------------------------------------------------
  Copyright 2020-2023 Renowned Games All rights reserved.
  ================================================================ */

using System;

namespace RenownedGames.Apex
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class OrderAttribute : ApexAttribute
    {
        public readonly int order;

        public OrderAttribute(int order)
        {
            this.order = order;
        }
    }
}