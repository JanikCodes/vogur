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
    [NodeContent("Player Prefs Has Key", "Player Prefs/Has Key", IconPath = "Images/Icons/Node/PlayerPrefsIcon.png")]
    public class PlayerPrefsHasKeyDecorator : ConditionDecorator
    {
        [SerializeField]
        private StringKey key;

        /// <summary>
        /// Calculates the result of the condition.
        /// </summary>
        protected override bool CalculateResult()
        {
            return key != null && PlayerPrefs.HasKey(key.GetValue());
        }
    }
}