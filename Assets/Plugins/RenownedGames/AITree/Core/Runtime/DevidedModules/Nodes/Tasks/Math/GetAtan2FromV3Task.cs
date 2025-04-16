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
    [NodeContent("Get Atan2 From Vector3", "Tasks/Math/Trigonometry/Get Atan2 From Vector3", IconPath = "Images/Icons/Node/TrigonometryIcon.png")]
    public class GetAtan2FromV3Task : TaskNode
    {
        private enum Axis
        {
            X,
            Y,
            Z
        }

        [Title("Node")]
        [SerializeField]
        private Vector3Key vector3;

        [SerializeField]
        private Axis XAxis = Axis.X;

        [SerializeField]
        private Axis YAxis = Axis.Y;

        [SerializeField]
        [NonLocal]
        private FloatKey storeResult;

        [SerializeField]
        private bool radToDeg;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (vector3 == null || storeResult == null)
            {
                return State.Failure;
            }

            Vector3 vector = vector3.GetValue();

            float x = XAxis switch
            {
                Axis.X => vector.x,
                Axis.Y => vector.y,
                Axis.Z => vector.z,
                _ => 0
            };

            float y = YAxis switch
            {
                Axis.X => vector.x,
                Axis.Y => vector.y,
                Axis.Z => vector.z,
                _ => 0
            };

            float result = Mathf.Atan2(y, x);
            storeResult.SetValue(radToDeg ? Mathf.Rad2Deg * result : result);
            return State.Success;
        }
    }
}