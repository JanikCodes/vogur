/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;
using UnityEngine.AI;
using RenownedGames.Apex;

namespace RenownedGames.AITree.Demo
{
    [HideMonoScript]
    [AddComponentMenu("Renowned Games/AI Tree/Demo/Animation/AI Animator")]
    [ObjectIcon("Images/Logotype/AITreeIcon.png")]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIAnimator : MonoBehaviour
    {
        [SerializeField]
        private bool rootMotion = false;

        [SerializeField]
        private float minVelocityToMove = 0.5f;

        [SerializeField]
        private float velocitySmoothSpeed = 10;

        [SerializeField]
        [Foldout("Parameters", Style = "Group")]
        private string speedParameter = "Speed";

        [SerializeField]
        [Foldout("Parameters", Style = "Group")]
        private string directionParameter = "Direction";

        [SerializeField]
        [Foldout("Parameters", Style = "Group")]
        private string isMovingParameter = "IsMoving";

        // Stored required components.
        private Animator animator;
        private NavMeshAgent navMeshAgent;

        // Stored required properties.
        private int speedHash;
        private int directionHash;
        private int isMovingHash;

        private Vector2 smoothVelocity = Vector2.zero;
        private Vector3 lastPosition;

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            if (rootMotion)
            {
                animator.applyRootMotion = true;
                navMeshAgent.updatePosition = false;
                navMeshAgent.updateRotation = true;
            }

            speedHash = 0;
            directionHash = 0;
            isMovingHash = 0;

            for (int i = 0; i < animator.parameters.Length; i++)
            {
                AnimatorControllerParameter parameter = animator.parameters[i];

                if (parameter.name == speedParameter)
                {
                    speedHash = Animator.StringToHash(speedParameter);
                }

                if (parameter.name == directionParameter)
                {
                    directionHash = Animator.StringToHash(directionParameter);
                }

                if (parameter.name == isMovingParameter)
                {
                    isMovingHash = Animator.StringToHash(isMovingParameter);
                }
            }

            lastPosition = transform.position;
        }

        /// <summary>
        /// This callback will be invoked at each frame after the state machines and the animations have been evaluated, but before OnAnimatorIK.
        /// </summary>
        private void OnAnimatorMove()
        {
            if (rootMotion)
            {
                Vector3 rootPosition = animator.rootPosition;
                rootPosition.y = navMeshAgent.nextPosition.y;
                transform.position = rootPosition;
                transform.rotation = animator.rootRotation;
                navMeshAgent.nextPosition = rootPosition;
            }
        }

        /// <summary>
        /// Called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (navMeshAgent.isActiveAndEnabled)
            {
                SyncAnimatorAndAgent();
            }
        }

        private void SyncAnimatorAndAgent()
        {
            if (Time.deltaTime == 0) return;

            Vector3 worldDeltaPosition;

            if (rootMotion)
            {
                worldDeltaPosition = navMeshAgent.nextPosition - transform.position;
            }
            else
            {
                worldDeltaPosition = transform.position - lastPosition;
            }
            worldDeltaPosition.y = 0f;


            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);

            Vector2 velocity = deltaPosition / Time.deltaTime;
            smoothVelocity = Vector2.Lerp(smoothVelocity, velocity, Mathf.Min(1, Time.deltaTime * velocitySmoothSpeed));

            bool shouldMove = velocity.magnitude > minVelocityToMove && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;

            if (speedHash != 0)
            {
                animator.SetFloat(speedHash, smoothVelocity.y);
            }

            if (directionHash != 0)
            {
                animator.SetFloat(directionHash, smoothVelocity.x);
            }

            if (isMovingHash != 0)
            {
                animator.SetBool(isMovingHash, shouldMove);
            }

            lastPosition = transform.position;
        }

        #region [Getter / Setter]
        public bool RootMotion()
        {
            return rootMotion;
        }

        public void RootMotion(bool value)
        {
            rootMotion = value;
        }

        public float GetMinVelocityToMove()
        {
            return minVelocityToMove;
        }

        public void SetMinVelocityToMove(float value)
        {
            minVelocityToMove = value;
        }

        public float GetVelocitySmoothSpeed()
        {
            return velocitySmoothSpeed;
        }

        public void SetVelocitySmoothSpeed(float value)
        {
            velocitySmoothSpeed = value;
        }

        public string GetSpeedParameter()
        {
            return speedParameter;
        }

        public void SetSpeedParameter(string value)
        {
            speedParameter = value;
            speedHash = Animator.StringToHash(speedParameter);
        }

        public string GetDirectionParameter()
        {
            return directionParameter;
        }

        public void SetDirectionParameter(string value)
        {
            directionParameter = value;
            directionHash = Animator.StringToHash(directionParameter);
        }

        public string GetIsMovingParameter()
        {
            return isMovingParameter;
        }

        public void SetIsMovingParameter(string value)
        {
            isMovingParameter = value;
            isMovingHash = Animator.StringToHash(isMovingParameter);
        }

        public Animator GetAnimator()
        {
            return animator;
        }

        public NavMeshAgent GetNavMeshAgent()
        {
            return navMeshAgent;
        }
        #endregion
    }
}