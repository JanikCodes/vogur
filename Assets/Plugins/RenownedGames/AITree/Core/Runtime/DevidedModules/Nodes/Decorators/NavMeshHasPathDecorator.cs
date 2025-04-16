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
using UnityEngine.AI;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Has Path (NavMesh)", "Has Path (NavMesh)", IconPath = "Images/Icons/EQS/Tests/HasPathTestIcon.png")]
    public class NavMeshHasPathDecorator : ConditionDecorator
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key sourcePosition;

        [SerializeField]
        [NonLocal]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key targetPosition;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override bool CalculateResult()
        {
            if (sourcePosition == null || targetPosition == null) return false;

            Vector3 positionA = Vector3.zero;
            {
                if (sourcePosition is TransformKey transformKey)
                {
                    Transform transform = transformKey.GetValue();
                    if (transform == null) return false;

                    positionA = transform.position;
                }
                else if (sourcePosition is Vector3Key vector3Key)
                {
                    positionA = vector3Key.GetValue();
                }
            }

            Vector3 positionB = Vector3.zero;
            {
                if (targetPosition is TransformKey transformKey)
                {
                    Transform transform = transformKey.GetValue();
                    if (transform == null) return false;

                    positionA = transform.position;
                }
                else if (targetPosition is Vector3Key vector3Key)
                {
                    positionA = vector3Key.GetValue();
                }
            }

            bool hasPath = false;
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(positionA, positionB, NavMesh.AllAreas, path))
            {
                hasPath = path.status == NavMeshPathStatus.PathComplete;
            }

            return hasPath;
        }

        #region [IEntityDescription Implementation]
        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            return $"Has Path: {(sourcePosition != null ? sourcePosition.name : "None")} - {(targetPosition != null ? targetPosition.name : "None")}";
        }
        #endregion
    }
}