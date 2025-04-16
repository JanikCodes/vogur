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
    [NodeContent("Player Prefs Set Float", "Tasks/Player Prefs/Set Float", IconPath = "Images/Icons/Node/PlayerPrefsIcon.png")]
    public class PPSetFloatTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private StringKey key;

        [SerializeField]
        private FloatKey value;

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

            PlayerPrefs.SetFloat(key.GetValue(), value.GetValue());
            return State.Success;
        }
    }
}