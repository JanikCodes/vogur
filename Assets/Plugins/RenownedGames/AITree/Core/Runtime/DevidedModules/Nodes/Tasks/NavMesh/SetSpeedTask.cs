/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEngine.AI;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Set Speed", "Tasks/Nav Mesh/Set Speed", IconPath = "Images/Icons/Node/SetSpeedIcon.png")]
    [RequireComponent(typeof(NavMeshAgent))]
    public class SetSpeedTask : TaskNode
    {
        [SerializeField]
        private float speed = 3;

        // Stored required components.
        private NavMeshAgent agent;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            agent = GetOwner().GetComponent<NavMeshAgent>();
        }

        protected override State OnUpdate()
        {
            if (agent == null)
            {
                return State.Failure;
            }

            agent.speed = speed;
            return State.Success;
        }

        public override string GetDescription()
        {
            return $"Set Speed: {speed}";
        }
    }
}