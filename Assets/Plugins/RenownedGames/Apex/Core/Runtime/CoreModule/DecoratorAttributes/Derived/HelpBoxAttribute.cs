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
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class HelpBoxAttribute : DecoratorAttribute
    {
        public readonly string text;

        public HelpBoxAttribute(string text)
        {
            this.text = text;
            Style = MessageStyle.Info;
            Height = 20;
        }

        public MessageStyle Style { get; set; }

        public float Height { get; set; }
    }
}