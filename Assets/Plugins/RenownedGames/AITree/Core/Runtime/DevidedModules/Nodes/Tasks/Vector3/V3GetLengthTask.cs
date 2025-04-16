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
    [NodeContent("Get Vector3 Length", "Tasks/Vector 3/Get Length", IconPath = "Images/Icons/Node/Vector3Icon.png")]
    public class V3GetLengthTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector3Key vector3;

        [SerializeField]
        [NonLocal]
        private FloatKey storeLength;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if(vector3 == null || storeLength == null)
            {
                return State.Failure;
            }

            storeLength.SetValue(vector3.GetValue().magnitude);
            return State.Success;
        }
    }
}