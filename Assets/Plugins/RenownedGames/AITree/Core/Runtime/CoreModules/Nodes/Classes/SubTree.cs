/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.AITree
{
    [NodeContent("Sub Tree", "Tasks/Common/SubTree", IconPath = "Images/Icons/Node/SubTreeIcon.png")]
    public class SubTree : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        [Label("Behaviour Tree")]
        private BehaviourTree sharedBehaviourTree;

        // Stored required properties.
        private BehaviourTree behaviourTree;
        private Blackboard blackboard;

        /// <summary>
        /// Called on behaviour tree is awake.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            AssociateContent();
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (behaviourTree != null)
            {
                return behaviourTree.OnUpdate();
            }
            return State.Failure;
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            if (sharedBehaviourTree == null)
            {
                return $"Sub tree: None";
            }
            return $"Sub tree: {sharedBehaviourTree.name}";
        }

#if UNITY_EDITOR
        /// <summary>
        /// Called when the value in the inspector changes.
        /// </summary>
        protected override void OnInspectorChanged()
        {
            base.OnInspectorChanged();

            if (sharedBehaviourTree != null && sharedBehaviourTree == GetBehaviourTree())
            {
                EditorUtility.DisplayDialog("AI Tree: SubTree",
                    $"Recursion detected.\n\n" +
                    $"{sharedBehaviourTree.name} -> {sharedBehaviourTree.name} ->->->...",
                    "Fix");
                sharedBehaviourTree = null;
            }
        }
#endif

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
        /// Clone and initialize shared blackboard for this sub tree.
        /// </summary>
        private void AssociateBlackboard()
        {
            Blackboard sharedBlackboard = sharedBehaviourTree.GetBlackboard();
            Blackboard masterSharedBlackboard = GetOwner().GetSharedBlackboard();
            Blackboard masterBlackboard = GetOwner().GetBlackboard();

            if (sharedBlackboard == masterSharedBlackboard)
            {
                blackboard = masterBlackboard;
            }
            else
            {
                blackboard = sharedBlackboard.Clone();

                if(sharedBlackboard.GetParent() == masterSharedBlackboard)
                {
                    blackboard.SetParent(masterBlackboard);
                }
                else
                {
                    foreach (Key key in blackboard.Keys)
                    {
                        if (key is SelfKey selfKey)
                        {
                            selfKey.SetValue(GetOwner().transform);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clone and initialize shared behaviour tree for this sub tree.
        /// </summary>
        private void AssociateBehaviourTree()
        {
            if (sharedBehaviourTree != null)
            {
                behaviourTree = sharedBehaviourTree.Clone();
                behaviourTree.Initialize(GetOwner(), blackboard);
            }
        }

        #region [Getter / Setter]
        /// <summary>
        /// Shared reference of behaviour tree.
        /// </summary>
        public BehaviourTree GetSharedSubTree()
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
        public void SetSharedSubTree(BehaviourTree sharedBehaviourTree)
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
        /// Associated instance of sub behaviour tree.
        /// </summary>
        public BehaviourTree GetSubTree()
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