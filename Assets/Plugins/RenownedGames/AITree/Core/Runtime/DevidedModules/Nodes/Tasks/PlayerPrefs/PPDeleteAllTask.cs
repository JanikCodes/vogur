/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Player Prefs Delete All", "Tasks/Player Prefs/Delete All", IconPath = "Images/Icons/Node/PlayerPrefsIcon.png")]
    public class PPDeleteAllTask : TaskNode
    {
        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            PlayerPrefs.DeleteAll();
            return State.Success;
        }
    }
}