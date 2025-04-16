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
    [NodeContent("Add XYZ", "Tasks/Vector 3/Add XYZ", IconPath = "Images/Icons/Node/Vector3Icon.png")]
    public class V3AddXYZTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector3Key vector3;

        [SerializeField]
        private FloatKey addX;

        [SerializeField]
        private FloatKey addY;

        [SerializeField]
        private FloatKey addZ;

        [SerializeField]
        [NonLocal]
        private Vector3Key storedResult;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (vector3 == null  || storedResult == null)
            {
                return State.Failure;
            }

            Vector3 result = vector3.GetValue(); 
            if(addX != null)
            {
                result.x += addX.GetValue();
            }

            if (addY != null)
            {
                result.y += addY.GetValue();
            }

            if (addZ != null)
            {
                result.z += addZ.GetValue();
            }

            storedResult.SetValue(result);
            return State.Success;
        }
    }
}