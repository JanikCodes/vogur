/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace RenownedGames.Apex
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class MessageAttribute : DecoratorAttribute
    {
        public readonly string text;

        public MessageAttribute(string text)
        {
            this.text = text;
            Style = MessageStyle.Info;
            FontSize = 11;
            FontStyle = FontStyle.Normal;
            Alignment = TextAnchor.MiddleLeft;
            RichText = false;
            Height = 20;
        }

        public float Height { get; set; }

        public MessageStyle Style { get; set; }

        public int FontSize { get; set; }

        public FontStyle FontStyle { get; set; }

        public TextAnchor Alignment { get; set; }

        public bool RichText { get; set; }
    }
}