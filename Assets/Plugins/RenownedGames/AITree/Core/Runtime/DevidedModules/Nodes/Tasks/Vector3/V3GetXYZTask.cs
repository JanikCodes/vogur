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
    [NodeContent("Get XYZ", "Tasks/Vector 3/Get XYZ", IconPath = "Images/Icons/Node/Vector3Icon.png")]
    public class V3GetXYZTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector3Key vector3;

        [SerializeField]
        [NonLocal]
        private FloatKey storeX;

        [SerializeField]
        [NonLocal]
        private FloatKey storeY;

        [SerializeField]
        [NonLocal]
        private FloatKey storeZ;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (vector3 == null || storeX == null || storeY == null || storeZ == null) 
            {
                return State.Failure;
            }

            Vector3 source = vector3.GetValue();
            storeX.SetValue(source.x);
            storeY.SetValue(source.y);
            storeZ.SetValue(source.z);

            return State.Success;
        }
    }
}