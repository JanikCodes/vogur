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
    [NodeContent("Random Position", "Tasks/Nav Mesh/Random Position", IconPath = "Images/Icons/Node/RandomPositionIcon.png")]
    public class RandomPositionTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        private Vector3Key key;

        [Title("Node")]
        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key origin;

        [SerializeField]
        private FloatKey radius;

        // Stored required components.
        private Transform ownerTransform;

        /// <summary>
        /// Called on behaviour tree is awake.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            ownerTransform = GetOwner().transform;
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (radius == null || key == null)
            {
                return State.Failure;
            }

            if(origin == null || !origin.TryGetPosition(out Vector3 originValue))
            {
                originValue = ownerTransform.position;
            }

            Vector3 randomOffset = Random.insideUnitSphere * radius.GetValue();
            Vector3 randomPosition = originValue + new Vector3(randomOffset.x, 0, randomOffset.z);

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hitInfo, radius.GetValue(), NavMesh.AllAreas))
            {
                key.SetValue(hitInfo.position);
            }
            else
            {
                key.SetValue(randomPosition);
            }

            return State.Success;
        }

        public override string GetDescription()
        {
            string description = $"RandomPosition:";

            if (origin != null)
            {
                description += $"\nOrigin: {origin.ToString()}";
            }

            if (radius != null)
            {
                description += $"\nRadius: {radius.ToString()}";
            }

            return description;
        }
    }
}