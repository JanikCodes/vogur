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
    [NodeContent("Player Prefs Delete Key", "Tasks/Player Prefs/Delete Key", IconPath = "Images/Icons/Node/PlayerPrefsIcon.png")]
    public class PPDeleteKeyTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private StringKey key;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (key == null)
            {
                return State.Failure;
            }

            PlayerPrefs.DeleteKey(key.GetValue());
            return State.Success;
        }
    }
}