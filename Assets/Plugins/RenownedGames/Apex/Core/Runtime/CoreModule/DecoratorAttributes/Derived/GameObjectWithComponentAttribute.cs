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
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class GameObjectWithComponentAttribute : DecoratorAttribute
    {
        public readonly Type type;

        public GameObjectWithComponentAttribute(Type type)
        {
            this.type = type;
            Format = "{name} required {type} component!";
            Style = MessageStyle.Error;
        }

        #region [Parameters]
        /// <summary>
        /// Custom message format. 
        /// Arguments: {name}, {type}
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Height of help box message.
        /// </summary>
        [Obsolete("The Height property is deprecated, the height is calculated automatically.")]
        public float Height { get; set; }

        /// <summary>
        /// Help box message style.
        /// </summary>
        public MessageStyle Style { get; set; }
        #endregion
    }
}