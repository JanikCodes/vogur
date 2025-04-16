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
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class SliderAttribute : ViewAttribute
    {
        public readonly float minValue;
        public readonly float maxValue;
        public readonly string minProperty;
        public readonly string maxProperty;

        public SliderAttribute(float minValue, float maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public SliderAttribute(string minProperty, string maxProperty)
        {
            this.minProperty = minProperty;
            this.maxProperty = maxProperty;
        }

        public SliderAttribute(string minProperty, float maxValue)
        {
            this.minProperty = minProperty;
            this.maxValue = maxValue;
        }

        public SliderAttribute(float minValue, string maxProperty)
        {
            this.minValue = minValue;
            this.maxProperty = maxProperty;
        }
    }
}