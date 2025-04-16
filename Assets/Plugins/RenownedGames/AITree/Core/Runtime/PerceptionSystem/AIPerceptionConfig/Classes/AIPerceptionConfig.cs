/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;

namespace RenownedGames.AITree.PerceptionSystem
{
    [Serializable]
    public abstract class AIPerceptionConfig : IAIPerceptionConfig
    {
        private AIPerception owner;

        /// <summary>
        /// Internal call to initialize config only in editor.
        /// <br><b>Use only in editor!</b></br>
        /// </summary>
        /// <param name="owner">Reference of AI perception owner.</param>
        internal void InitializeInEditor(AIPerception owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Called once when initializing config.
        /// </summary>
        /// <param name="owner">Reference of AI perception owner.</param>
        public virtual void Initialize(AIPerception owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Called on the frame when a script is enabled,
        /// just before any of the OnUpdate methods are called the first time.
        /// </summary>
        protected internal virtual void OnStart() { }

        /// <summary>
        /// Called when the config becomes enabled and active.
        /// </summary>
        protected internal virtual void OnEnable() { }

        /// <summary>
        /// Called every frame, if the config is enabled.
        /// </summary>
        protected internal virtual void OnUpdate() { }

        /// <summary>
        /// Called every fixed frame-rate frame, 
        /// 0.02 seconds (50 calls per second) is the default time between calls.
        /// </summary>
        protected internal virtual void OnFixedUpdate() { }

        /// <summary>
        /// Called when the config becomes disabled.
        /// </summary>
        protected internal virtual void OnDisable() { }

        /// <summary>
        /// Called when the config becomes destroyed.
        /// </summary>
        protected internal virtual void OnStop() { }

        /// <summary>
        /// Called while selecting object, use for debug.
        /// </summary>
        protected internal virtual void OnDrawGizmosSelected() { }

        #region [Event Callback Functions]
        /// <summary>
        /// Called when perception config has updated the target.
        /// <br><b>Param type of(AIPerceptionSource)</b>: New reference of AI perception source.</br>
        /// </summary>
        public abstract event Action<AIPerceptionSource> OnTargetUpdated;
        #endregion

        #region [Getter / Setter]
        public AIPerception GetOwner()
        {
            return owner;
        }
        #endregion
    }
}