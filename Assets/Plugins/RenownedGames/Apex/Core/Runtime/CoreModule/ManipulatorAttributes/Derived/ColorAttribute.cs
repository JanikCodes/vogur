/* ================================================================
  ---------------------------------------------------
  Project   :    Apex
  Publisher :    Renowned Games
  Developer :    Tamerlan Shakirov
  ---------------------------------------------------
  Copyright 2020-2023 Renowned Games All rights reserved.
  ================================================================ */

namespace RenownedGames.Apex
{
    public sealed class ColorAttribute : ManipulatorAttribute
    {
        public readonly float r;
        public readonly float g;
        public readonly float b;
        public readonly float a;

        public ColorAttribute(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        #region [Optional Parameters]
        public ColorTarget Target { get; set; }
        #endregion
    }
}