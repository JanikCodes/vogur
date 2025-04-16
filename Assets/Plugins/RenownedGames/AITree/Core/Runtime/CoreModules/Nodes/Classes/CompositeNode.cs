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
    public abstract class CompositeNode : WrapNode
    {
        [SerializeReference]
        [HideInInspector]
        private List<Node> children = new List<Node>();

        // Stored required properties.
        private int current;
        private Dictionary<Node, int> childIndexes;

        /// <summary>
        /// Sets a reference to the parent node.
        /// </summary>
        internal override void SetupParentReference()
        {
            base.SetupParentReference();

            childIndexes = new Dictionary<Node, int>();

            for (int i = 0; i < children.Count; i++)
            {
                Node child = children[i];
                if (child != null)
                {
                    child.SetParent(this);
                    childIndexes.Add(child, i);
                }
            }
        }

        /// <summary>
        /// Executes node logic.
        /// </summary>
        /// <returns>State</returns>
        public override State Update()
        {
            if (children.Count == 0)
            {
                return State.Failure;
            }

            State state = base.Update();

            if (state != State.Running)
            {
                for (int i = current + 1; i < children.Count; i++)
                {
                    Node child = children[i];
                    if (child != null)
                    {
                        child.Restore();
                    }
                }
            }

            // Visualization request of executed children in behaviour tree graph.
#if UNITY_EDITOR
            for (int i = 0; i < current; i++)
            {
                Node child = children[i];
                if (child != null)
                {
                    child.Visualize(true);
                }
            }
#endif

            return state;
        }

        /// <summary>
        /// Traverse through all next linked nodes with visitor callback.
        /// </summary>
        public override void Traverse(Action<Node> visiter)
        {
            base.Traverse(visiter);

            for (int i = 0; i < children.Count; i++)
            {
                Node child = children[i];
                if (child != null)
                {
                    child.Traverse(visiter);
                }
            }
        }

        /// <summary>
        /// Adds a child node.
        /// </summary>
        /// <param name="node">Node.</param>
        public void AddChild(Node node)
        {
            children.Add(node);
        }

        /// <summary>
        /// Deletes a child node.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>True if the node is deleted.</returns>
        public bool RemoveChild(Node node)
        {
            return children.Remove(node);
        }

        /// <summary>
        /// Deletes all child.
        /// </summary>
        public void RemoveChildren()
        {
            children.Clear();
        }

        /// <summary>
        /// Returns the next child node by the current node.
        /// </summary>
        public Node NextChild(Node currentChild)
        {
            if (childIndexes.ContainsKey(currentChild))
            {
                int index = childIndexes[currentChild];

                if (index + 1 < children.Count)
                {
                    return children[index + 1];
                }
            }

            return null;
        }

        /// <summary>
        /// Jumps to a specific node to execute it.
        /// </summary>
        /// <param name="jumpNode">Node to jump.</param>
        public void JumpToNode(WrapNode jumpNode)
        {
            if (children.Contains(jumpNode))
            {
                if (!IsStarted())
                {
                    OnEntry();
                    IsStarted(true);
                }

                current = children.IndexOf(jumpNode);
            }
        }

        /// <summary>
        /// Clone node with all its stuff.
        /// </summary>
        /// <returns>New cloned copy of node.</returns>
        public override Node Clone()
        {
            children.RemoveAll(t => t == null);
            children.TrimExcess();

#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
            }
#endif

            CompositeNode composite = (CompositeNode)base.Clone();
            if(children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    composite.children[i] = children[i].Clone();
                }
            }
            return composite;
        }

        internal override void OnClone(CloneData cloneData)
        {
            base.OnClone(cloneData);
            for (int i = 0; i < children.Count; i++)
            {
                children[i] = cloneData.cloneNodeMap[children[i]];
            }
        }

        /// <summary>
        /// Called when behaviour tree aborts node.
        /// </summary>
        protected override void OnAbort()
        {
            base.OnAbort();
            // Visualization request of children in behaviour tree graph.
#if UNITY_EDITOR
            for (int i = 0; i < children.Count; i++)
            {
                Node child = children[i];
                if(child != null)
                {
                    child.Visualize(true);
                }
            }
#endif
        }

        #region [Getter / Setter]
        public List<Node> GetChildren()
        {
            return children;
        }

        public void SetChildren(List<Node> children)
        {
            this.children = children;
        }

        public int GetChildCount()
        {
            return children.Count;
        }

        public int GetCurrent()
        {
            return current;
        }

        protected void SetCurrent(int value)
        {
            current = value;
        }

        public virtual Node GetChild(int index)
        {
            if (index >= 0 && index < children.Count)
            {
                return children[index];
            }
            return null;
        }
        #endregion
    }
}
