/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using Pathfinding;
using UnityEngine;

namespace RenownedGames.AITree.Integrations.AstarPathfindingProject
{
    [NodeContent("Move To", "Tasks/Integrations/A* Pathfinding Project/Move To", IconPath = "Images/Icons/Node/AStarIcon.png")]
    [RequireComponent(typeof(IAstarAI))]
    public class AstarMoveToNode : TaskNode
    {
        [Header("Blackboard")]
        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key key;

        [Header("Advanced")]
        [SerializeField]
        private bool allowPartialPath;

        [SerializeField]
        private bool trackMovingGoal = true;

        [SerializeField]
        private bool goLastPointOnLost = true;

        // Stored required components.
        private IAstarAI ai;

        // Stored required properties.
        private Vector3? destinationPosition;
        private Vector3? lastTargetPosition;

        protected override void OnInitialize()
        {
            ai = GetOwner().GetComponent<IAstarAI>();
        }

        protected override void OnEntry()
        {
            ai.canSearch = true;

            if (!trackMovingGoal)
            {
                destinationPosition = GetDestinationPosition();
                if (destinationPosition != null)
                {
                    ai.destination = destinationPosition.Value;
                }
            }
        }

        protected override State OnUpdate()
        {
            if (ai == null)
            {
                return State.Failure;
            }

            if (trackMovingGoal)
            {
                destinationPosition = GetDestinationPosition();
                if (destinationPosition != null)
                {
                    lastTargetPosition = destinationPosition.Value;
                    ai.destination = destinationPosition.Value;
                }
                else
                {
                    if (goLastPointOnLost)
                    {
                        if (lastTargetPosition != null)
                        {
                            ai.destination = lastTargetPosition.Value;
                        }
                        else
                        {
                            return State.Failure;
                        }
                    }
                    else
                    {
                        return State.Failure;
                    }
                }
            }

            if (!allowPartialPath && !ai.hasPath)
            {
                return State.Failure;
            }

            ai.isStopped = false;

            if (ai.reachedDestination)
            {
                return State.Success;
            }

            return State.Running;
        }

        /// <summary>
        /// Called when behaviour tree exit from node.
        /// </summary>
        protected override void OnExit()
        {
            base.OnExit();
            ai.isStopped = true;
        }

        /// <summary>
        /// Returns the destination position for movement.
        /// </summary>
        /// <returns>Destination position.</returns>
        private Vector3? GetDestinationPosition()
        {
            if (key.TryCastValueTo<Transform>(out Transform transform))
            {
                return transform.position;
            }
            else if (key.TryCastValueTo<Vector3>(out Vector3 position))
            {
                return position;
            }
            return null;
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = "A* Move To: ";
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

        /// <summary>
        /// Implement OnDrawGizmosSelected to draw a gizmo if the object is selected.
        /// </summary>
        public override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            if (destinationPosition.HasValue)
            {
                Gizmos.DrawSphere(destinationPosition.Value, .2f);
            }
        }
    }
}