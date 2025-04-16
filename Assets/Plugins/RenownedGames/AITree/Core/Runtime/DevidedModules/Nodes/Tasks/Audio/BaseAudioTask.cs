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
    public abstract class BaseAudioTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private TransformKey transform;

        // Stored required components.
        private AudioSource audioSource;

        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        protected override void OnEntry()
        {
            base.OnEntry();
            if (transform != null)
            {
                Transform _transform = transform.GetValue();
                if (transform != null)
                {
                    audioSource = _transform.GetComponent<AudioSource>();
                }
            }
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected sealed override State OnUpdate()
        {
            if (audioSource == null)
            {
                return State.Failure;
            }

            Action(audioSource);

            return State.Success;
        }

        protected abstract void Action(AudioSource audioSource);

        #region [Getter / Setter]
        public Vector3 GetGameObjectPosition()
        {
            if (transform != null)
            {
                Transform _transform = transform.GetValue();
                if (_transform != null)
                {
                    return _transform.position;
                }
            }

            return Vector3.zero;
        }
        #endregion
    }
}