/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RenownedGames.AITreeEditor.UIElements
{
    internal class SequenceNumberView : Label
    {
        public new class UxmlFactory : UxmlFactory<SequenceNumberView, UxmlTraits> { }

        private static HashSet<SequenceNumberView> lastHighlighted = new HashSet<SequenceNumberView>();
        private static readonly StyleColor hoverColor = new StyleColor(new Color(.84f, .16f, .16f));

        private NodeView target;
        private BehaviourTreeGraph graph;
        private StyleColor defaultColor;

        public void Init(BehaviourTreeGraph graph, NodeView target)
        {
            this.graph = graph;
            this.target = target;
            defaultColor = style.backgroundColor;

            RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            RegisterCallback<MouseLeaveEvent>(OnLeaveEnter);
        }

        /// <summary>
        /// Event sent when the mouse pointer enters an element or one of its descendent elements. 
        /// The event is cancellable, it does not trickle down, and it does not bubble up.
        /// </summary>
        /// <param name="evt">The event instance.</param>
        private void OnMouseEnter(MouseEnterEvent evt)
        {
            Node node = null;

            if (target.GetNode() is AuxiliaryNode auxiliary)
            {
                node = auxiliary.GetContainerNode();
            }

            if (node == null)
            {
                if (target is WrapView)
                {
                    node = target.GetNode();
                }
            }

            if (node != null)
            {
                Highlight(node);

                Node parent = node.GetParent();
                if (parent is CompositeNode composite)
                {
                    foreach (Node child in composite.GetChildren())
                    {
                        Highlight(child);
                    }
                }
            }
        }

        /// <summary>
        /// Event sent when the mouse pointer exits an element and all its descendent elements.
        /// The event is cancellable, it does not trickle down, and it does not bubble up.
        /// </summary>
        /// <param name="evt">The event instance.</param>
        private void OnLeaveEnter(MouseLeaveEvent evt)
        {
            foreach (SequenceNumberView numberView in lastHighlighted)
            {
                numberView.style.backgroundColor = defaultColor;
            }

            lastHighlighted.Clear();
        }

        private void Highlight(Node node)
        {
            NodeView nodeView = graph.FindNodeView(node);
            if (nodeView != null)
            {
                SequenceNumberView numberView = nodeView.GetSequenceNumberView();
                numberView.style.backgroundColor = hoverColor;
                lastHighlighted.Add(numberView);

                if (node is WrapNode wrap)
                {
                    foreach (AuxiliaryNode auxiliary in wrap.AllAuxiliaryNodes())
                    {
                        Highlight(auxiliary);
                    }
                }
            }
        }

        #region [Getter / Setter]
        public NodeView GetTarget()
        {
            return target;
        }

        public StyleColor GetDefaultColor()
        {
            return defaultColor;
        }
        #endregion
    }
}