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
using System.Collections;
using UnityEngine;

namespace RenownedGames.AITree
{
    [AddComponentMenu("Renowned Games/AI Tree/Behaviour/Behaviour Runner")]
    [ObjectIcon("Images/Logotype/AITreeIcon.png")]
    [DisallowMultipleComponent]
    public class BehaviourRunner : MonoBehaviour
    {
        [SerializeField]
        private BehaviourTree sharedBehaviourTree;

        // Stored required components.
        private BehaviourTree behaviourTree;
        private Blackboard blackboard;

        /// <summary>
        /// Called when an enabled script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            if (sharedBehaviourTree == null)
            {
                Debug.LogWarning($"BehaviourRunner component of the (<i>{name}</i>) game object does not have a behaviour tree!");
                enabled = false;
                return;
            }

            AssociateContent();
        }

        /// <summary>
        /// Called on the frame when a script is enabled,
        /// just before any of the Update methods are called the first time.
        /// </summary>
        protected virtual void Start()
        {
            if (behaviourTree != null)
            {
                behaviourTree.OnStart();
            }
        }

        /// <summary>
        /// Called every frame, if the Behaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
            if (behaviourTree != null)
            {
                if (behaviourTree.GetUpdateMode() == UpdateMode.Update || (behaviourTree.GetUpdateMode() == UpdateMode.Custom && Time.frameCount % behaviourTree.GetTickRate() == 0))
                {
                    behaviourTree.OnUpdate();
                }
            }
        }

        /// <summary>
        /// Called every fixed frame-rate frame. 0.02 seconds (50 calls per second) is the default time between calls, if the Behaviour is enabled.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (behaviourTree != null && behaviourTree.GetUpdateMode() == UpdateMode.FixedUpdate)
            {
                behaviourTree.OnUpdate();
            }
        }

        /// <summary>
        /// Called every frame, if the Behaviour is enabled.
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (behaviourTree != null && behaviourTree.GetUpdateMode() == UpdateMode.LateUpdate)
            {
                behaviourTree.OnUpdate();
            }
        }

        /// <summary>
        /// Called when an enabled script instance is being destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (behaviourTree != null)
            {
                behaviourTree.OnStop();
            }
        }

        /// <summary>
        /// Implement if you want to draw gizmos that are also pickable and always drawn.
        /// <br>This is Editor-only</br>
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
            if (behaviourTree != null)
            {
                behaviourTree.OnDrawGizmos();
            }
        }

        /// <summary>
        /// Implement to draw a gizmos if the object is selected.
        /// </summary>
        protected virtual void OnDrawGizmosSelected()
        {
            if (behaviourTree != null)
            {
                behaviourTree.OnDrawGizmosSelected();
            }
        }

        /// <summary>
        /// Clone and initialize shared behaviour tree for this runner.
        /// </summary>
        private void AssociateBehaviourTree()
        {
            if (sharedBehaviourTree != null)
            {
                behaviourTree = sharedBehaviourTree.Clone();
                behaviourTree.Initialize(this);
                OnCloneTreeCompleted?.Invoke(behaviourTree);
            }
        }

        /// <summary>
        /// Clone and initialize shared blackboard for this runner.
        /// </summary>
        private void AssociateBlackboard()
        {
            Blackboard sharedBlackboard = GetSharedBlackboard();
            if (sharedBlackboard != null)
            {
                blackboard = sharedBlackboard.Clone();
                InitializeSelfKey(blackboard);
                NotifySyncReceivers(blackboard);
            }
        }

        /// <summary>
        /// Clone and initialize shared behaviour tree and its blackboard.
        /// </summary>
        private void AssociateContent()
        {
            if (blackboard == null)
            {
                AssociateBlackboard();
            }

            if (behaviourTree == null)
            {
                AssociateBehaviourTree();
            }
        }

        /// <summary>
        /// Find and associated the system Self key with current runner.
        /// </summary>
        /// <param name="blackboard">Associated instance of blackboard.</param>
        private void InitializeSelfKey(Blackboard blackboard)
        {
            foreach (Key key in blackboard.Keys)
            {
                if (key is SelfKey selfKey)
                {
                    selfKey.SetValue(transform);
                }
            }
        }

        /// <summary>
        /// Notify all self and children components which implement ISyncKeyReceiver interface.
        /// </summary>
        /// <param name="blackboard">Associated instance of blackboard.</param>
        private void NotifySyncReceivers(Blackboard blackboard)
        {
            ISyncKeyReceiver[] receivers = GetComponentsInChildren<ISyncKeyReceiver>();
            for (int i = 0; i < receivers.Length; i++)
            {
                receivers[i].OnSynchronization(blackboard.GetAllKeys());
            }
        }

        #region [Events]
        /// <summary>
        /// Called when behaviour runner complete cloning of shared beahviour tree.
        /// </summary>
        public event Action<BehaviourTree> OnCloneTreeCompleted;
        #endregion

        #region [Getter / Setter]
        /// <summary>
        /// Shared reference of behaviour tree.
        /// </summary>
        public BehaviourTree GetSharedBehaviourTree()
        {
            return sharedBehaviourTree;
        }

        /// <summary>
        /// Set new shared behaviour tree reference.
        /// <br>Note: This action will stop the execution of the current behavior tree and terminate it.
        /// The work of the tree, its state and the values of the keys in the blackboard will be destroyed and overwritten by the new tree.
        /// This action is irreversible!</br>
        /// </summary>
        /// <param name="behaviourTree">Reference of behaviour tree.</param>
        public void SetSharedBehaviourTree(BehaviourTree sharedBehaviourTree)
        {
            if (this.sharedBehaviourTree != sharedBehaviourTree)
            {
                behaviourTree = null;
                blackboard = null;
                this.sharedBehaviourTree = sharedBehaviourTree;
                AssociateContent();
            }
        }

        /// <summary>
        /// Associated instance of behaviour tree.
        /// </summary>
        public BehaviourTree GetBehaviourTree()
        {
            if (behaviourTree == null && Application.isPlaying)
            {
                AssociateBehaviourTree();
            }

            return behaviourTree;
        }

        /// <summary>
        /// Shared reference of blackboard.
        /// </summary>
        public Blackboard GetSharedBlackboard()
        {
            if (sharedBehaviourTree == null)
            {
                return null;
            }

            return sharedBehaviourTree.GetBlackboard();
        }

        /// <summary>
        /// Associated instance of blackboard.
        /// </summary>
        public Blackboard GetBlackboard()
        {
            if (blackboard == null && Application.isPlaying)
            {
                AssociateBlackboard();
            }

            return blackboard;
        }
        #endregion
    }
}
