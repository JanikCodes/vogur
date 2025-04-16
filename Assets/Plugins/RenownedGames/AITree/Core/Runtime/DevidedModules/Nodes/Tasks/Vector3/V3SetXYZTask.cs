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
    [NodeContent("Set XYZ", "Tasks/Vector 3/Set XYZ", IconPath = "Images/Icons/Node/Vector3Icon.png")]
    public class V3SetXYZTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector3Key storeResult;

        [SerializeField]
        private FloatKey setX;

        [SerializeField]
        private FloatKey setY;

        [SerializeField]
        private FloatKey setZ;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if(storeResult == null)
            {
                return State.Failure;
            }

            Vector3 result = storeResult.GetValue();
            if(setX != null)
            {
                result.x = setX.GetValue();
            }

            if (setY != null)
            {
                result.y = setY.GetValue();
            }

            if (setZ != null)
            {
                result.z = setZ.GetValue();
            }

            storeResult.SetValue(result);
            return State.Success;
        }
    }
}