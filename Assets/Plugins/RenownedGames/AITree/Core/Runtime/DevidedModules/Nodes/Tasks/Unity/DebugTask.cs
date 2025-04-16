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
    [System.Obsolete("Use Debug Log task instead")]
    [NodeContent("Debug", "Tasks/Unity/Debug", IconPath = "Images/Icons/Node/DebugIcon.png", Hide = true)]
    public class DebugTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private string text = "Text";

        protected override State OnUpdate()
        {
            Debug.Log($"{GetBehaviourTree().name}: {text}");
            return State.Success;
        }
    }
}
