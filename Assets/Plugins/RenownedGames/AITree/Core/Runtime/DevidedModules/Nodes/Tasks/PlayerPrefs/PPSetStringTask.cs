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
    [NodeContent("Player Prefs Set String", "Tasks/Player Prefs/Set String", IconPath = "Images/Icons/Node/PlayerPrefsIcon.png")]
    public class PPSetStringTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private StringKey key;

        [SerializeField]
        private StringKey value;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (key == null || value == null)
            {
                return State.Failure;
            }

            PlayerPrefs.GetString(key.GetValue(), value.GetValue());
            return State.Success;
        }
    }
}