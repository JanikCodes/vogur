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
using UnityEngine;

namespace RenownedGames.AITree
{
    [NodeContent("Run Behaviour", "Tasks/Common/Run Behaviour", IconPath = "Images/Icons/Node/RunBehaviourIcon.png")]
    public class RunBehaviourTask : TaskNode
    {
        [Title("Node")]
        [SerializeField]
        private BehaviourTree runBehaviourTree;

        /// <summary>
        /// Called on behaviour tree is awake.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            runBehaviourTree.Initialize(GetOwner());
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns></returns>
        protected override State OnUpdate()
        {
            return runBehaviourTree.OnUpdate();
        }

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            if(runBehaviourTree == null)
            {
                return $"Run Behaviour: None";
            }
            return $"Run Behaviour: {runBehaviourTree.name}";
        }

        /// <summary>
        /// Returns a clone of a specific T object.
        /// </summary>
        /// <returns>Type of object to clone.</returns>
        public override Node Clone()
        {
            RunBehaviourTask node = base.Clone() as RunBehaviourTask;
            node.runBehaviourTree = runBehaviourTree.Clone();
            return node;
        }

        internal override void OnClone(CloneData cloneData)
        {
            base.OnClone(cloneData);
            runBehaviourTree = runBehaviourTree.Clone();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Called when the value in the inspector changes.
        /// </summary>
        protected override void OnInspectorChanged()
        {
            base.OnInspectorChanged();
            if (runBehaviourTree == GetBehaviourTree())
            {
                runBehaviourTree = null;
            }
        }
#endif

        #region [Getter / Setter]
        public BehaviourTree GetRunBehaviourTree()
        {
            return runBehaviourTree;
        }

        public void GetRunBehaviourTree(BehaviourTree behaviourTree)
        {
            runBehaviourTree = behaviourTree;
        }
        #endregion
    }
}