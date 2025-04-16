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
    [NodeContent("Get Rotation", "Tasks/Transform/Get Rotation", IconPath = "Images/Icons/Node/TransformIcon.png")]
    public class TrfmGetRotationTask : TaskNode
    {
        private enum TargetKeyType
        {
            Quaternion,
            EulerAngles,
            EulerAnglesXYZ
        }

        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private TransformKey transform;

        [Title("Node")]
        [SerializeField]
        private Space space = Space.World;

        [SerializeField]
        private TargetKeyType targetKeyType = TargetKeyType.Quaternion;

        [SerializeField]
        [ShowIf("targetKeyType", TargetKeyType.Quaternion)]
        [NonLocal]
        private QuaternionKey quaternion;

        [SerializeField]
        [ShowIf("targetKeyType", TargetKeyType.EulerAngles)]
        [NonLocal]
        private Vector3Key vector;

        [SerializeField]
        [ShowIf("targetKeyType", TargetKeyType.EulerAnglesXYZ)]
        [NonLocal]
        private FloatKey xAngle;

        [SerializeField]
        [ShowIf("targetKeyType", TargetKeyType.EulerAnglesXYZ)]
        [NonLocal]
        private FloatKey yAngle;

        [SerializeField]
        [ShowIf("targetKeyType", TargetKeyType.EulerAnglesXYZ)]
        [NonLocal]
        private FloatKey zAngle;

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

            Quaternion rotation = _transform.rotation;
            if (space == Space.Self)
            {
                rotation = _transform.localRotation;
            }

            if (targetKeyType == TargetKeyType.Quaternion)
            {
                if (quaternion == null)
                {
                    return State.Failure;
                }

                quaternion.SetValue(rotation);
            }
            else if (targetKeyType == TargetKeyType.EulerAngles)
            {
                if (vector == null)
                {
                    return State.Failure;
                }

                vector.SetValue(rotation.eulerAngles);
            }
            else
            {
                if (xAngle == null || yAngle == null || xAngle == null)
                {
                    return State.Failure;
                }

                xAngle.SetValue(rotation.eulerAngles.x);
                yAngle.SetValue(rotation.eulerAngles.y);
                zAngle.SetValue(rotation.eulerAngles.z);
            }

            return State.Success;
        }
    }
}