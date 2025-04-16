/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Parallel", "Composites/Parallels/Parallel", IconPath = "Images/Icons/Node/ParallelIcon.png")]
    public class ParallelNode : CompositeNode
    {
        [SerializeField]
        private bool skipLeftNodes = true;

        protected State[] childrenLeftToExecute;

        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        protected override void OnEntry()
        {
            base.OnEntry();

            if (childrenLeftToExecute == null || childrenLeftToExecute.Length != GetChildCount())
            {
                childrenLeftToExecute = new State[GetChildCount()];
            }

            for (int i = 0; i < GetChildCount(); i++)
            {
                childrenLeftToExecute[i] = State.Running;
            }
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        protected override State OnUpdate()
        {
            bool skipLeftNodes = GetSkipLeftNodes();

            bool stillRunning = false;
            for (int i = 0; i < childrenLeftToExecute.Length; i++)
            {
                Node child = GetChild(i);
                if (!skipLeftNodes || skipLeftNodes && childrenLeftToExecute[i] == State.Running)
                {
                    if (child == null)
                    {
                        AbortRunningChildren();
                        return State.Failure;
                    }

                    State state = child.Update();
                    switch (state)
                    {
                        case State.Success:
                            break;
                        case State.Failure:
                            AbortRunningChildren();
                            return State.Failure;
                        case State.Aborted:
                            AbortRunningChildren();
                            return State.Aborted;
                        case State.Running:
                            stillRunning = true;
                            break;
                    }

                    childrenLeftToExecute[i] = state;
                }
            }

            return stillRunning ? State.Running : State.Success;
        }

        /// <summary>
        /// Calls Abort for all child nodes that are running.
        /// </summary>
        protected void AbortRunningChildren()
        {
            for (int i = 0; i < childrenLeftToExecute.Length; i++)
            {
                if (childrenLeftToExecute[i] == State.Running)
                {
                    Node child = GetChild(i);
                    if (child != null)
                    {
                        child.Abort();
                    }
                }
            }
        }

        #region [Getter / Setter]
        public bool GetSkipLeftNodes()
        {
            return skipLeftNodes;
        }

        public void SetSkipLeftNodes(bool value)
        {
            skipLeftNodes = value;
        }
        #endregion
    }
}