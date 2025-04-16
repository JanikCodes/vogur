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
    [NodeContent("Get Position", "Tasks/Transform/Get Position", IconPath = "Images/Icons/Node/TransformIcon.png")]
    public class TrfmGetPositionTask : TaskNode
    {
        private enum TargetKeyType
        {
            Vector,
            XYZ
        }

        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private TransformKey transform;

        [Title("Node")]
        [SerializeField]
        private Space space = Space.World;

        [SerializeField]
        private TargetKeyType targetKeyType = TargetKeyType.Vector;

        [SerializeField]
        [ShowIf("targetKeyType", TargetKeyType.Vector)]
        [NonLocal]
        private Vector3Key vector;

        [SerializeField]
        [ShowIf("targetKeyType", TargetKeyType.XYZ)]
        [NonLocal]
        private FloatKey x;

        [SerializeField]
        [ShowIf("targetKeyType", TargetKeyType.XYZ)]
        [NonLocal]
        private FloatKey y;

        [SerializeField]
        [ShowIf("targetKeyType", TargetKeyType.XYZ)]
        [NonLocal]
        private FloatKey z;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (transform == null)
            {
                return State.Failure;
            }

            Transform _transform = transform.GetValue();
            if (_transform == null)
            {
                return State.Failure;
            }

            Vector3 position = _transform.position;
            if (space == Space.Self)
            {
                position = _transform.localPosition;
            }

            if (targetKeyType == TargetKeyType.Vector)
            {
                if (vector == null)
                {
                    return State.Failure;
                }

                vector.SetValue(position);
            }
            else
            {
                if (x == null || y == null || z == null)
                {
                    return State.Failure;
                }

                x.SetValue(position.x);
                y.SetValue(position.y);
                z.SetValue(position.z);
            }

            return State.Success;
        }
    }
}