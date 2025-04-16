/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev, Vladimir Deryabin
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Vector2 Clamp Magnitude", "Tasks/Vector 2/Clamp Magnitude", IconPath = "Images/Icons/Node/Vector2Icon.png")]
    public class V2ClampMagTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private Vector2Key vector2;

        [SerializeField]
        private FloatKey maxLength;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (vector2 == null || maxLength == null)
            {
                return State.Failure;
            }

            vector2.SetValue(Vector2.ClampMagnitude(vector2.GetValue(), maxLength.GetValue()));
            return State.Success;
        }
    }
}