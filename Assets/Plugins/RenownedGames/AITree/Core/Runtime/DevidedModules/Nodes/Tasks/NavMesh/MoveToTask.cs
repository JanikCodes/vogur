/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;
using UnityEngine.AI;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Move To", "Tasks/Nav Mesh/Move To", IconPath = "Images/Icons/Node/AIMove.png")]
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoveToTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key key;

        [Title("Node")]
        [SerializeField]
        private float acceptableRadius = 0.1f;

        [SerializeField]
        private bool includeAgentRadius = true;

        [SerializeField]
        private bool includeGoalRadius = true;

        [Title("Advanced")]
        [SerializeField]
        private bool allowPartialPath;

        [SerializeField]
        private bool trackMovingGoal = true;

        [SerializeField]
        private bool goLastPointOnLost = true;

        // Stored required components.
        private NavMeshAgent agent;
        private NavMeshAgent goalAgent;
        private CapsuleCollider goalCollider;

        // Stored required properties.
        private Vector3? lastTargetPosition;

        /// <summary>
        /// Called on behaviour tree is awake.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            agent = GetOwner().GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        protected override void OnEntry()
        {
            base.OnEntry();

            agent.stoppingDistance = 0f;

            if (includeGoalRadius)
            {
                if (key.TryGetTransform(out Transform transform))
                {
                    if(goalAgent == null || (goalAgent != null && goalAgent.transform != transform))
                    {
                        goalAgent = transform.GetComponent<NavMeshAgent>();
                    }

                    if (goalAgent == null || goalCollider == null || (goalCollider != null && goalCollider.transform != transform))
                    {
                        goalCollider = transform.GetComponent<CapsuleCollider>();
                    }
                }
            }

            if (!trackMovingGoal)
            {
                if (key.TryGetPosition(Space.World, out Vector3 value))
                {
                    agent.destination = value;
                }
            }
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        protected override State OnUpdate()
        {
            if (agent == null)
            {
                Debug.LogWarning($"The controller [{GetOwner().name}] should be based on NavMeshControll.");
                return State.Failure;
            }

            agent.isStopped = false;

            if (trackMovingGoal)
            {
                if (key.TryGetPosition(Space.World, out Vector3 value))
                {
                    lastTargetPosition = value;
                    agent.destination = value;
                }
                else
                {
                    if (goLastPointOnLost)
                    {
                        if (lastTargetPosition != null)
                        {
                            agent.destination = lastTargetPosition.Value;
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

            if (!allowPartialPath && agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                return State.Failure;
            }


            if (!agent.pathPending || agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                float tolerance = acceptableRadius;

                if (includeAgentRadius)
                {
                    tolerance += agent.radius;
                }

                if (includeGoalRadius)
                {
                    if(goalAgent != null)
                    {
                        tolerance += goalAgent.radius;
                    }
                    else if(goalCollider != null)
                    {
                        tolerance += goalCollider.radius;
                    }
                }

                if (agent.remainingDistance <= tolerance)
                {
                    return State.Success;
                }
            }

            return State.Running;
        }

        /// <summary>
        /// Called when behaviour tree exit from node.
        /// </summary>
        protected override void OnExit()
        {
            base.OnExit();

            if (agent.isOnNavMesh)
            {
                agent.isStopped = true;
            }
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = "Move To: ";
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

            if (IsStarted() && agent.hasPath)
            {
                Gizmos.DrawSphere(agent.destination, .2f);
            }
        }
    }
}
