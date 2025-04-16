/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System;
using UnityEngine;

namespace RenownedGames.AITree.PerceptionSystem
{
    [RequireComponent(typeof(BehaviourRunner))]
    public abstract class AIPerception : MonoBehaviour, IAIPerception
    {
        [SerializeField]
        private bool replaceable = false;

        [SerializeReference]
        [ReferenceArray]
        private AIPerceptionConfig[] configs;

        // Stored required properties.
        private AIPerceptionSource lastSource;

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            for (int i = 0; i < configs.Length; i++)
            {
                AIPerceptionConfig config = configs[i];
                if (config != null)
                {
                    config.OnTargetUpdated += OnTargetUpdated;
                    config.Initialize(this);
                }
            }
        }

        /// <summary>
        /// Calls when the script is loaded or a value changes in the Inspector.
        /// </summary>
        protected virtual void OnValidate()
        {
            if (configs == null) return;

            for (int i = 0; i < configs.Length; i++)
            {
                AIPerceptionConfig config = configs[i];
                if (config != null)
                {
                    config.InitializeInEditor(this);
                }
            }
        }

        /// <summary>
        /// Called on the frame when a script is enabled,
        /// just before any of the Update methods are called the first time.
        /// </summary>
        protected virtual void Start()
        {
            for (int i = 0; i < configs.Length; i++)
            {
                AIPerceptionConfig config = configs[i];
                if (config != null)
                {
                    config.OnStart();
                }
            }
        }

        /// <summary>
        /// Called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            for (int i = 0; i < configs.Length; i++)
            {
                AIPerceptionConfig config = configs[i];
                if (config != null)
                {
                    config.OnEnable();
                }
            }
        }

        /// <summary>
        /// Called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            for (int i = 0; i < configs.Length; i++)
            {
                AIPerceptionConfig config = configs[i];
                if (config != null)
                {
                    config.OnUpdate();
                }
            }
        }

        /// <summary>
        /// Called every fixed frame-rate frame, 
        /// 0.02 seconds (50 calls per second) is the default time between calls.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            for (int i = 0; i < configs.Length; i++)
            {
                AIPerceptionConfig config = configs[i];
                if (config != null)
                {
                    config.OnFixedUpdate();
                }
            }
        }

        /// <summary>
        /// Called when the behaviour becomes disabled.
        /// </summary>
        private void OnDisable()
        {
            for (int i = 0; i < configs.Length; i++)
            {
                AIPerceptionConfig config = configs[i];
                if (config != null)
                {
                    config.OnDisable();
                }
            }
        }

        /// <summary>
        /// Called when object is destroying.
        /// </summary>
        protected virtual void OnDestroy()
        {
            for (int i = 0; i < configs.Length; i++)
            {
                AIPerceptionConfig config = configs[i];
                if (config != null)
                {
                    config.OnStop();
                }
            }
        }

        /// <summary>
        /// Implement OnDrawGizmosSelected to draw a gizmo if the object is selected.
        /// </summary>
        protected virtual void OnDrawGizmosSelected()
        {
            if (configs == null) return;

            for (int i = 0; i < configs.Length; i++)
            {
                AIPerceptionConfig config = configs[i];
                if (config != null)
                {
                    config.OnDrawGizmosSelected();
                }
            }
        }

        /// <summary>
        /// Called when AI perception source has been updated.
        /// </summary>
        /// <param name="target">Updated AI perception source instance.</param>
        protected virtual void OnTargetUpdated(AIPerceptionSource source)
        {
            if (source != lastSource && (replaceable || lastSource == null || source == null))
            {
                OnTargetChanged(source);
                lastSource = source;
            }
        }

        /// <summary>
        /// Called when AI perception source has been changed.
        /// </summary>
        /// <param name="target">Changed AI perception source instance.</param>
        protected virtual void OnTargetChanged(AIPerceptionSource source)
        {
            if (source != null && lastSource == null)
            {
                OnSourceDetect?.Invoke(source);
            }

            if (source == null && lastSource != null)
            {
                OnSourceLoss?.Invoke(lastSource);
            }
        }

        #region [Event Callback Functions]
        /// <summary>
        /// Called when source detected.
        /// </summary>
        public event Action<AIPerceptionSource> OnSourceDetect;

        /// <summary>
        /// Called when source losses.
        /// </summary>
        public event Action<AIPerceptionSource> OnSourceLoss;
        #endregion

        #region [Getter / Setter]
        public AIPerceptionConfig[] GetConfigs()
        {
            return configs;
        }

        public void SetConfigs(AIPerceptionConfig[] value)
        {
            configs = value;
        }
        #endregion
    }
}