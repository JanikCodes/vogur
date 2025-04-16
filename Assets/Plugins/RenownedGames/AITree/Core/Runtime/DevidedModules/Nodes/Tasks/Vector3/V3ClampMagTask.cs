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
    [NodeContent("Vector3 Clamp Magnitude", "Tasks/Vector 3/Clamp Magnitude", IconPath = "Images/Icons/Node/Vector3Icon.png")]
    public class V3ClampMagTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private Vector3Key vector3;

        [SerializeField]
        private FloatKey maxLength;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (vector3 == null || maxLength == null)
            {
                return State.Failure;
            }

            vector3.SetValue(Vector3.ClampMagnitude(vector3.GetValue(), maxLength.GetValue()));
            return State.Success;
        }
    }
}