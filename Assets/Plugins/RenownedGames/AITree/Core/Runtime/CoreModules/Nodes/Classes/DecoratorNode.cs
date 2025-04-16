/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022 - 2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.AITree
{
    public abstract class DecoratorNode : AuxiliaryNode, IProcessableNode
    {
        // Stored required properties.
        // Temporary solution, after the update, need to move this field in WrapNode.
        private bool processing;

        #region [Getter / Setter]
        public bool IsProcessing()
        {
            return processing;
        }

        internal void IsProcessing(bool value)
        {
            processing = value;
        }
        #endregion
    }
}
