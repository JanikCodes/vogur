/* ================================================================
   ---------------------------------------------------
   Project   :    Apex
   Publisher :    Renowned Games
   Developer :    Tamerlan Shakirov
   ---------------------------------------------------
   Copyright 2020-2023 Renowned Games All rights reserved.
   ================================================================ */

using System.Collections.Generic;

namespace RenownedGames.ApexEditor
{
    public interface IContainer
    {
        /// <summary>
        /// Loop through all entities of the entity container.
        /// </summary>
        IEnumerable<VisualEntity> Entities { get; }
    }
}