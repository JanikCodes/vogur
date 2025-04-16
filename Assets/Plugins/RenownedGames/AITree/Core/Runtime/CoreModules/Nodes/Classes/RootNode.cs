/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RenownedGames.AITree
{
    [NodeContent("Root", "", Hide = true, IconPath = "Images/Icons/Node/RootIcon.png")]
    public sealed class RootNode : Node
    {
        [SerializeField]
        [HideInInspector]
        private Node child;

        /// <summary>
        /// Sets a reference to the parent node.
        /// </summary>
        internal override void SetupParentReference()
        {
            if (child != null)
            {
                child.SetParent(this);
            }
        }

        /// <summary>
        /// Executes node logic.
        /// </summary>
        /// <returns>State</returns>
        public override State Update()
        {
            return child == null ? State.Failure : base.Update();
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        protected override State OnUpdate()
        {
            return child.Update();
        }

        /// <summary>
        /// Traverse through all next linked nodes with visitor callback.
        /// </summary>
        public override void Traverse(Action<Node> visiter)
        {
            base.Traverse(visiter);
            if (child != null)
            {
                child.Traverse(visiter);
            }
        }

        /// <summary>
        /// Returns a clone of a specific T object.
        /// </summary>
        /// <returns>Type of object to clone.</returns>
        public override Node Clone()
        {
            RootNode root = base.Clone() as RootNode;
            if (child != null)
            {
                root.child = child.Clone();
            }
            return root;
        }

        internal override void OnClone(CloneData cloneData)
        {
            base.OnClone(cloneData);
            child = cloneData.cloneNodeMap[child];
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            if (GetBehaviourTree() != null)
            {
                Blackboard blackboard = GetBehaviourTree().GetBlackboard();
                if (blackboard != null)
                {
                    return blackboard.name;
                }
            }
            return "None";
        }

        #region [Getter / Setter]
        /// <summary>
        /// Returns a child node.
        /// </summary>
        /// <returns></returns>
        public Node GetChild()
        {
            return child;
        }

        /// <summary>
        /// Sets the child node.
        /// </summary>
        /// <param name="child">Node</param>
        public void SetChild(Node child)
        {
            this.child = child;
        }
        #endregion
    }
}
