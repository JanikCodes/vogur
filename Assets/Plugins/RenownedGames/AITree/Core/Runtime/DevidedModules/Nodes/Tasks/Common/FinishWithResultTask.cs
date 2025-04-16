/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System.Collections.Generic;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Finish With Result", "Tasks/Common/Finish with Result", IconPath = "Images/Icons/Node/FinishWithResultIcon.png")]
    public class FinishWithResultTask : TaskNode
    {
        [Title("Result")]
        [SerializeField]
        [Enum(HideValues = "GetHiddenState")]
        private State result;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            return result;
        }

#if UNITY_EDITOR
        private IEnumerable<State> GetHiddenState()
        {
            yield return State.Aborted;
            yield return State.Running;
        }
#endif
    }
}