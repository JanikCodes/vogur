/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022 - 2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.AITree
{
    /// <summary>
    /// Miscellaneous helper attribute for Key field.
    /// <br>Description: Custom link to Behaviour Runner for key fields defined out out Node or BEhaviour Runner game objects.</br>
    /// <br><i>Property has priority over Method.</i></br>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class RunnerLinkAttribute : Attribute
    {
        /// <summary>
        /// Serialized property of runner.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Getter method of runner.
        /// <br>Example: <i>BehaviourRunner GetRunner();</i></br>
        /// </summary>
        public string Method { get; set; }
    }
}