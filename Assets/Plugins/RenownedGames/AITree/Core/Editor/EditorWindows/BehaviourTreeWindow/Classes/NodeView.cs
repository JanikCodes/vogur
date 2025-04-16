/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using RenownedGames.AITreeEditor.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = RenownedGames.AITree.Node;
using NodeElement = UnityEditor.Experimental.GraphView.Node;

namespace RenownedGames.AITreeEditor
{
    internal abstract class NodeView : NodeElement, IDropTarget
    {
        private Node node;
        private int sequenceNumber;
        private string defaultName;
        private Label titleLabel;
        private Label descriptionLabel;
        private VisualElement iconElement;
        private SequenceNumberView sequenceNumberView;
        private NodeProgressView progressView;
        private BehaviourTreeGraph graph;
        private AITreeSettings settings;

        /// <summary>
        /// Node view constructor.
        /// </summary>
        /// <param name="node">Node reference.</param>
        /// <param name="uxmlFile">UXML file path.</param>
        public NodeView(BehaviourTreeGraph graph, Node node, string uxmlFile) : base(uxmlFile)
        {
            this.graph = graph;
            this.node = node;

            viewDataKey = node.GetInstanceID().ToString();
            styleSheets.Add(AITreeSettings.instance.GetNodeUSS());

            Vector2 nodePosition = node.GetNodePosition();
            style.left = nodePosition.x;
            style.top = nodePosition.y;

            iconElement = this.Q<VisualElement>("info-icon");
            titleLabel = this.Q<Label>(className: "info-title");
            descriptionLabel = this.Q<Label>("info-description");
            sequenceNumberView = this.Q<SequenceNumberView>("sequence-number");
            sequenceNumberView?.Init(graph, this);
            progressView = this.Q<NodeProgressView>();
            progressView.style.display = DisplayStyle.None;

            LoadNameAndIcon();
            InitializeStyles();
            UpdateCapabilities();
            OnInspectorUpdate();

            settings = AITreeSettings.instance;

            node.GetBehaviourTree().Updating += UpdateProgessView;
        }

        /// <summary>
        /// Called once when loading node view to initialize styles.
        /// </summary>
        protected abstract void InitializeStyles();

        /// <summary>
        /// Called when the GraphElement is selected.
        /// </summary>
        public override void OnSelected()
        {
            base.OnSelected();
            UpdateSelection?.Invoke(node);
            node.GetBehaviourTree().SetSelectedNode(node);
        }

        /// <summary>
        /// Called when the GraphElement is unselected.
        /// </summary>
        public override void OnUnselected()
        {
            base.OnUnselected();
            UpdateSelection?.Invoke(null);
            node.GetBehaviourTree().SetSelectedNode(null);
        }

        /// <summary>
        /// Set node position.
        /// </summary>
        /// <param name="newPos">New position.</param>
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
        }

        /// <summary>
        /// Called once when loading node view to update capabilities.
        /// </summary>
        protected virtual void UpdateCapabilities()
        {
            if (node is RootNode)
            {
                capabilities &= ~Capabilities.Deletable;
            }
        }

        /// <summary>
        /// Load default name and icon for node view.
        /// </summary>
        protected void LoadNameAndIcon()
        {
            NodeTypeCache.NodeCollection nodeInfos = NodeTypeCache.GetNodesInfo();
            for (int i = 0; i < nodeInfos.Count; i++)
            {
                NodeTypeCache.NodeInfo nodeInfo = nodeInfos[i];
                if(nodeInfo.type == node.GetType())
                {
                    if (nodeInfo.contentAttribute != null)
                    {
                        if (!string.IsNullOrWhiteSpace(nodeInfo.contentAttribute.name))
                        {
                            defaultName = nodeInfo.contentAttribute.name;
                        }
                        else if (!string.IsNullOrWhiteSpace(nodeInfo.contentAttribute.path))
                        {
                            defaultName = System.IO.Path.GetFileName(nodeInfo.contentAttribute.path);
                        }
                    }
                    else
                    {
                        defaultName = ObjectNames.NicifyVariableName(node.name);
                    }

                    if (nodeInfo.icon != null)
                    {
                        iconElement.style.backgroundImage = new StyleBackground(nodeInfo.icon);
                        iconElement.style.display = DisplayStyle.Flex;
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Set sequence number of node view.
        /// </summary>
        public void SetSequenceNumber(int value, bool global)
        {
            sequenceNumber = value;
            if (global)
            {
                sequenceNumberView.text = value.ToString();
            }
        }

        /// <summary>
        /// Called at 10 frames per second to give the inspector a chance to update.
        /// </summary>
        protected internal void OnInspectorUpdate()
        {
            if (!string.IsNullOrEmpty(node.GetName().Trim()))
            {
                titleLabel.text = node.GetName().Trim();
            }
            else
            {
                titleLabel.text = defaultName;
            }

            string description = node.GetDescription();
            if (string.IsNullOrEmpty(description))
            {
                description = defaultName;
            }
            descriptionLabel.text = description;
        }

        /// <summary>
        /// Called in behaviour tree update.
        /// </summary>
        private void UpdateProgessView()
        {
            if (settings.ShowNodeProgress() && node.IsStarted())
            {
                float? progress = node.GetProgress();
                if (progress.HasValue)
                {
                    progressView.style.display = DisplayStyle.Flex;
                    progressView.SetValue(progress.Value);
                }
            }
            else
            {
                progressView.style.display = DisplayStyle.None;
            }
        }

        #region [IDropTarget Implementation]
        public bool CanAcceptDrop(List<ISelectable> selection)
        {
            if (selection.Count == 1)
            {
                if (selection[0] is AuxiliaryView)
                {
                    return true;
                }
            }

            return false;
        }

        public bool DragUpdated(DragUpdatedEvent evt, IEnumerable<ISelectable> selection, IDropTarget dropTarget, ISelection dragSource)
        {
            return false;
        }

        public bool DragPerform(DragPerformEvent evt, IEnumerable<ISelectable> selection, IDropTarget dropTarget, ISelection dragSource)
        {
            AuxiliaryView dropableView = selection.First() as AuxiliaryView;
            AuxiliaryNode dropable = dropableView.GetNode() as AuxiliaryNode;

            WrapNode fromWrap = dropable.GetContainerNode();

            if (dropable is DecoratorNode dropableDecorator)
            {
                if (dropTarget is DecoratorView decoratorView)
                {
                    DecoratorNode decorator = decoratorView.node as DecoratorNode;
                    WrapNode toWrap = decorator.GetContainerNode();
                    int index = toWrap.GetDecorators().IndexOf(decorator);

                    Undo.RecordObject(fromWrap, "[BehaviourTree] Remove decorator");
                    fromWrap.RemoveDecorator(dropableDecorator);

                    Undo.RecordObject(toWrap, "[BehaviourTree] Add decorator");
                    toWrap.AddDecorator(dropableDecorator, index);

                    ViewChanged?.Invoke();
                    return true;
                }
                else if (dropTarget is WrapView wrapView)
                {
                    WrapNode toWrap = wrapView.node as WrapNode;

                    if (toWrap != null)
                    {
                        Undo.RecordObject(fromWrap, "[BehaviourTree] Remove decorator");
                        fromWrap.RemoveDecorator(dropableDecorator);

                        Undo.RecordObject(toWrap, "[BehaviourTree] Add decorator");
                        toWrap.AddDecorator(dropableDecorator);

                        ViewChanged?.Invoke();
                        return true;
                    }
                }
            }
            else if (dropable is ServiceNode dropableService)
            {
                if (dropTarget is ServiceView serviceView)
                {
                    ServiceNode service = serviceView.GetNode() as ServiceNode;
                    WrapNode toWrap = service.GetContainerNode();
                    int index = toWrap.GetServices().IndexOf(service);

                    Undo.RecordObject(fromWrap, "[BehaviourTree] Remove service");
                    fromWrap.RemoveService(dropableService);

                    Undo.RecordObject(toWrap, "[BehaviourTree] Add service");
                    toWrap.AddService(dropableService, index);

                    ViewChanged?.Invoke();
                    return true;
                }
                else if (dropTarget is WrapView wrapView)
                {
                    WrapNode toWrap = wrapView.node as WrapNode;

                    if (toWrap != null)
                    {
                        Undo.RecordObject(fromWrap, "[BehaviourTree] Remove service");
                        fromWrap.RemoveService(dropableService);

                        Undo.RecordObject(toWrap, "[BehaviourTree] Add service");
                        toWrap.AddService(dropableService);

                        ViewChanged?.Invoke();
                        return true;
                    }
                }
            }

            return false;
        }

        public bool DragEnter(DragEnterEvent evt, IEnumerable<ISelectable> selection, IDropTarget enteredTarget, ISelection dragSource)
        {
            return false;
        }

        public bool DragLeave(DragLeaveEvent evt, IEnumerable<ISelectable> selection, IDropTarget leftTarget, ISelection dragSource)
        {
            return false;
        }

        public bool DragExited()
        {
            return false;
        }
        #endregion

        #region [Events]
        /// <summary>
        /// Called when node view has been selected or unselected.
        /// </summary>
        public event Action<Node> UpdateSelection;

        /// <summary>
        /// Called when node view has been changed.
        /// </summary>
        public event Action ViewChanged;
        #endregion

        #region [Getter / Setter]
        public BehaviourTreeGraph GetGraph()
        {
            return graph;
        }

        public Node GetNode()
        {
            return node;
        }

        public int GetSequenceNumber()
        {
            return sequenceNumber;
        }

        public string GetDefaultName()
        {
            return defaultName;
        }

        public Label GetTitleLabel()
        {
            return titleLabel;
        }

        public Label GetDescriptionLabel()
        {
            return descriptionLabel;
        }

        public VisualElement GetIconElement()
        {
            return iconElement;
        }

        public SequenceNumberView GetSequenceNumberView()
        {
            return sequenceNumberView;
        }
        #endregion
    }
}