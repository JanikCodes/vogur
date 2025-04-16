/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022 - 2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.AITree
{
    public abstract class TaskNode : WrapNode
    {
        [SerializeField]
        private bool ignoreAbortSelf;

        #region [Getter / Setter]
        public bool IgnoreAbortSelf()
        {
            return ignoreAbortSelf;
        }

        public void IgnoreAbortSelf(bool ignoreAbortSelf)
        {
            this.ignoreAbortSelf = ignoreAbortSelf;
        }
        #endregion
    }
}
