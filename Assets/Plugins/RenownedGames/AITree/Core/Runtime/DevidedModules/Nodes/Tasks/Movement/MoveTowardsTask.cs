/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Move Towards", "Tasks/Movement/Move Towards", IconPath = "Images/Icons/Node/MoveObjectIcon.png")]
    public class MoveTowardsTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key key;

        [Title("Node")]
        [SerializeField]
        private FloatKey movementSpeed;

        [SerializeField]
        private float acceptableRadius = 0.1f;

        [SerializeField]
        private bool lookAtTarget = false;

        [SerializeField]
        [EnableIf("lookAtTarget")]
        private float rotationSpeed = 90f;

        [Title("Advanced")]
        [SerializeField]
        private bool trackMovingGoal = true;

        // Stored required components.
        private Transform transform;

        // Stored required properties.
        private Vector3? destinationPosition;

        /// <summary>
        /// Called on behaviour tree is awake.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            transform = GetOwner().transform;
        }

        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        protected override void OnEntry()
        {
            base.OnEntry();

            if (!trackMovingGoal && key.TryGetPosition(Space.World, out Vector3 value))
            {
                destinationPosition = value;
            }
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            if (movementSpeed == null)
            {
                return State.Failure;
            }

            if (trackMovingGoal && key.TryGetPosition(Space.World, out Vector3 value))
            {
                destinationPosition = value;
            }

            if (destinationPosition == null)
            {
                return State.Failure;
            }

            Vector3 targetPosition = destinationPosition.Value;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed.GetValue() * GetDeltaTime());

            Quaternion targetRotation = Quaternion.identity;
            if (lookAtTarget)
            {
                targetRotation = GetDesiredRotation();
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * GetDeltaTime());
            }

            if (Vector3.Distance(transform.position, targetPosition) < acceptableRadius &&
                (lookAtTarget && Quaternion.Dot(transform.rotation, targetRotation) >= 0.999f || !lookAtTarget))
            {
                return State.Success;
            }

            return State.Running;
        }

        /// <summary>
        /// Returns the desired rotation.
        /// </summary>
        /// <returns>Quaternion.</returns>
        private Quaternion GetDesiredRotation()
        {
            Vector3 direction = destinationPosition.Value - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                return Quaternion.LookRotation(direction);
            }

            return transform.rotation;
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = $"Move Towards: ";
            if (key != null)
            {
                description += key.ToString();
            }
            else
            {
                description += "None";
            }
            return description;
        }
    }
}