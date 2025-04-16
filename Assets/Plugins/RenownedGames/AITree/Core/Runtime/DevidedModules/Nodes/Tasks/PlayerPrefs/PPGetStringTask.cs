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
    [NodeContent("Player Prefs Get String", "Tasks/Player Prefs/Get String", IconPath = "Images/Icons/Node/PlayerPrefsIcon.png")]
    public class PPGetStringTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private StringKey key;

        [SerializeField]
        [NonLocal]
        private StringKey storeKey;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (key == null || storeKey == null)
            {
                return State.Failure;
            }

            storeKey.SetValue(PlayerPrefs.GetString(key.GetValue()));
            return State.Success;
        }
    }
}