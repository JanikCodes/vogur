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
    [NodeContent("Transform Set Scale", "Tasks/Transform/Set Scale", IconPath = "Images/Icons/Node/TransformIcon.png")]
    public class TrfmSetScaleTask : TaskNode
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
        private FloatKey xScale;

        [SerializeField]
        [ShowIf("targetKeyType", TargetKeyType.XYZ)]
        [NonLocal]
        private FloatKey yScale;

        [SerializeField]
        [ShowIf("targetKeyType", TargetKeyType.XYZ)]
        [NonLocal]
        private FloatKey zScale;

        

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

            Vector3 scale = _transform.lossyScale;
            if (space == Space.Self)
            {
                scale = _transform.localScale;
            }

            if (targetKeyType == TargetKeyType.Vector)
            {
                if (vector == null)
                {
                    return State.Failure;
                }

                vector.SetValue(scale);
            }
            else
            {
                if (xScale == null || yScale == null || zScale == null)
                {
                    return State.Failure;
                }

                xScale.SetValue(scale.x);
                yScale.SetValue(scale.y);
                zScale.SetValue(scale.z);
            }

            return State.Success;
        }
    }
}