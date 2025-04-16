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
    [NodeContent("Player Prefs Set Int", "Tasks/Player Prefs/Set Int", IconPath = "Images/Icons/Node/PlayerPrefsIcon.png")]
    public class PPSetIntTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private StringKey key;

        [SerializeField]
        private IntKey value;

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

            PlayerPrefs.SetInt(key.GetValue(), value.GetValue());
            return State.Success;
        }
    }
}