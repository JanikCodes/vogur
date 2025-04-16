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
    [NodeContent("Debug", "Debug", IconPath = "Images/Icons/Node/DebugIcon.png")]
    public class DebugService : IntervalServiceNode
    {
        [Title("Service")]
        [SerializeField]
        private string text = "Text";

        protected override void OnTick()
        {
            Debug.Log($"{GetBehaviourTree().name}: {text}");
        }
    }
}