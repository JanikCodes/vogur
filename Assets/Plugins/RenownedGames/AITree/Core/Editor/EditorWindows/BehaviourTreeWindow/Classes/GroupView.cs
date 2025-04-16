/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Group = RenownedGames.AITree.Group;
using GroupElement = UnityEditor.Experimental.GraphView.Group;
using Node = RenownedGames.AITree.Node;

namespace RenownedGames.AITreeEditor
{
    internal class GroupView : GroupElement
    {
        private bool deletionLocked;
        private Group group;
        private BehaviourTreeGraph graph;

        /// <summary>
        /// Group view constructor.
        /// </summary>
        /// <param name="group">Group reference.</param>
        public GroupView(BehaviourTreeGraph graph, Group group)
        {
            this.graph = graph;
            this.group = group;

            title = group.title;
            viewDataKey = group.GetInstanceID().ToString();

            SetPosition(new Rect(group.position, Vector2.zero));
        }

        /// <summary>
        /// Sets the geometry of the Scope.
        /// </summary>
        /// <param name="newPos">The new geometry.</param>
        public override void SetPosition(Rect newPos)
        {
            const string MOVE_GROUP_RECORD_KEY = "[BehaviourTree] Move group";

            Undo.RecordObject(group, MOVE_GROUP_RECORD_KEY);
            base.SetPosition(newPos);
            group.position.x = newPos.x;
            group.position.y = newPos.y;
            EditorUtility.SetDirty(group);
        }

        /// <summary>
        /// Called when added new elements.
        /// </summary>
        /// <param name="elements">Graph elements.</param>
        protected override void OnElementsAdded(IEnumerable<GraphElement> elements)
        {
            base.OnElementsAdded(elements);

            if (group != null)
            {
                List<Node> nodesToAdd = new List<Node>();

                foreach (GraphElement element in elements)
                {
                    if (element is WrapView wrapView)
                    {
                        wrapView.SetGroup(this);
                        nodesToAdd.Add(wrapView.GetNode());
                    }
                }

                AddNodes(nodesToAdd);
            }
        }

        /// <summary>
        /// Called when elements removed.
        /// </summary>
        /// <param name="elements">Graph elements.</param>
        protected override void OnElementsRemoved(IEnumerable<GraphElement> elements)
        {
            base.OnElementsRemoved(elements);

            if (!deletionLocked && group != null)
            {
                List<Node> nodesToRemove = new List<Node>();

                foreach (GraphElement element in elements)
                {
                    if (element is WrapView wrapView)
                    {
                        wrapView.SetGroup(null);
                        nodesToRemove.Add(wrapView.GetNode());
                    }
                }

                RemoveNodes(nodesToRemove);
            }
        }

        /// <summary>
        /// Called when this group is renamed. 
        /// </summary>
        /// <param name="oldName">Old group name.</param>
        /// <param name="newName">New group name.</param>
        protected override void OnGroupRenamed(string oldName, string newName)
        {
            Undo.RecordObject(group, "[BehaviourTree] Rename group.");
            base.OnGroupRenamed(oldName, newName);
            group.title = newName;
        }

        private void AddNode(Node node)
        {
            AddNodes(new List<Node> { node });
        }

        private void AddNodes(List<Node> nodes)
        {
            const string ADD_NODES_G_RECORD_KEY = "[BehaviourTree] Group elements added";

            Undo.RecordObject(group, ADD_NODES_G_RECORD_KEY);
            foreach (Node node in nodes)
            {
                if (!group.nodes.Contains(node))
                {
                    group.nodes.Add(node);
                }
            }
        }

        private void RemoveNode(Node node)
        {
            RemoveNodes(new List<Node> { node });
        }

        private void RemoveNodes(List<Node> nodes)
        {
            const string DELETE_NODES_G_RECORD_KEY = "[BehaviourTree] Group elements removed";

            Undo.RecordObject(group, DELETE_NODES_G_RECORD_KEY);
            foreach (Node node in nodes)
            {
                if (group.nodes.Contains(node))
                {
                    group.nodes.Remove(node);
                }
            }
        }

        #region [Getter / Setter]
        public BehaviourTreeGraph GetGraph()
        {
            return graph;
        }

        public Group GetGroup()
        {
            return group;
        }

        public bool DeletionLocked()
        {
            return deletionLocked;
        }

        public void DeletionLocked(bool value)
        {
            deletionLocked = value;
        }
        #endregion
    }
}

