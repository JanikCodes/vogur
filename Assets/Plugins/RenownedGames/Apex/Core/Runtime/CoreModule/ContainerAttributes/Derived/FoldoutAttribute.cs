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
    public sealed class FoldoutAttribute : ContainerAttribute
    {
        public FoldoutAttribute(string name) : base(name)
        {
            Style = string.Empty;
        }

        #region [Optional Parameters]
        public string Style { get; set; }
        #endregion
    }
}