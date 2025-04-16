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
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RenownedGames.AITreeEditor
{
    internal class DecoratorView : AuxiliaryView
    {
        private static readonly HashSet<WrapView> lastHighlighted = new HashSet<WrapView>();
        private static readonly StyleColor selfColor = new StyleColor(new Color(.02f, .57f, .44f));
        private static readonly StyleColor lowPriorityColor = new StyleColor(new Color(.03f, .45f, .58f));
        private static StyleColor? defaultColor;

        /// <summary>
        /// Decorator view constructor.
        /// </summary>
        /// <param name="graph">The graph where is contained view.</param>
        /// <param name="decorator">Decorator node reference.</param>
        public DecoratorView(BehaviourTreeGraph graph, DecoratorNode decorator) : base(graph, decorator, AssetDatabase.GetAssetPath(AITreeSettings.instance.GetDecoratorUXML()))
        {
            GetNode().InspectorChanged += UpdateHightlight;
        }

        /// <summary>
        /// Called once when loading node view to initialize styles.
        /// </summary>
        protected override void InitializeStyles()
        {
            if(GetNode() is TempDecoratorNode)
            {
                AddToClassList("temp-decorator");
            }
            else
            {
                AddToClassList("decorator");
            }
        }

        /// <summary>
        /// Called when the GraphElement is selected.
        /// </summary>
        public override void OnSelected()
        {
            base.OnSelected();
            UpdateHightlight();
        }

        /// <summary>
        /// Called when the GraphElement is unselected.
        /// </summary>
        public override void OnUnselected()
        {
            base.OnUnselected();
            UpdateHightlight();
        }

        private void UpdateHightlight()
        {
            HideHightlight();

            if (selected && !EditorApplication.isPlaying)
            {
                if (GetNode() is ObserverDecorator observerDecorator)
                {
                    switch (observerDecorator.GetObserverAbort())
                    {
                        case ObserverAbort.None:
                            break;
                        case ObserverAbort.Self:
                            HightlightSelf();
                            break;
                        case ObserverAbort.LowPriority:
                            HightlightLowPriority();
                            break;
                        case ObserverAbort.Both:
                            HightlightSelf();
                            HightlightLowPriority();
                            break;
                    }
                }
            }
        }

        private void HideHightlight()
        {
            foreach (WrapView wrapView in lastHighlighted)
            {
                if (defaultColor != null && defaultColor.HasValue)
                {
                    wrapView.Q<VisualElement>(name = "node").style.backgroundColor = defaultColor.Value;
                }
            }
            lastHighlighted.Clear();
            lastHighlighted.TrimExcess();
        }

        private void HightlightSelf()
        {
            DecoratorNode decorator = GetNode() as DecoratorNode;
            WrapNode wrap = decorator.GetContainerNode();
            wrap.Traverse(n =>
            {
                if (wrap != n || wrap is TaskNode)
                {
                    Hightlight(n, selfColor);
                }
            });
        }

        private void HightlightLowPriority()
        {
            DecoratorNode decorator = GetNode() as DecoratorNode;
            WrapNode parentNode = decorator.GetContainerNode();

            int parentIndex = parentNode.GetOrder() - parentNode.GetDecorators().Count;
            parentNode.Traverse(n => parentIndex++);

            parentNode.GetBehaviourTree().GetRootNode().Traverse(n =>
            {
                if (n.GetOrder() < parentIndex)
                {
                    return;
                }

                if (n is WrapNode wrap)
                {
                    Hightlight(wrap, lowPriorityColor);
                }
            });
        }

        private void Hightlight(Node node, StyleColor color)
        {
            NodeView nodeView = GetGraph().FindNodeView(node);
            if (nodeView != null && nodeView is WrapView wrapView)
            {
                if (defaultColor == null || !defaultColor.HasValue)
                {
                    defaultColor = wrapView.Q<VisualElement>(name = "node").style.backgroundColor;
                }

                wrapView.Q<VisualElement>(name = "node").style.backgroundColor = color;
                lastHighlighted.Add(wrapView);
            }
        }
    }
}