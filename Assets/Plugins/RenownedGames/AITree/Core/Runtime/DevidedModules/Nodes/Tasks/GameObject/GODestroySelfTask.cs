/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Destroy Self", "Tasks/Game Object/Destroy Self", IconPath = "Images/Icons/Node/GameObjectIcon.png")]
    public class GODestroySelfTask : TaskNode
    {
        [Title("Node")]
        private BoolKey detachChildred;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (detachChildred != null && detachChildred.GetValue())
            {
                foreach (Transform child in GetOwner().transform)
                {
                    child.SetParent(null);
                }
            }

            Destroy(GetOwner().gameObject);
            return State.Success;
        }
    }
}