/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;

namespace RenownedGames.ApexEditor
{
    public interface IInlineDecoratorSide
    {
        /// <summary>
        /// On which side should the space be reserved?
        /// </summary>
        InlineDecoratorSide GetSide();
    }
}