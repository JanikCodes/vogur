/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = RenownedGames.AITree.Node;

namespace RenownedGames.AITreeEditor
{
    internal class WrapView : NodeView
    {
        private static readonly Color RestingEdgeColor = new Color32(200, 200, 200, 255);
        private static readonly Color RunningEdgeColor = new Color32(254, 234, 4, 255);
        private static readonly Color SuccessEdgeColor = new Color32(68, 169, 101, 255);
        private static readonly Color FailureEdgeColor = new Color32(255, 77, 77, 255);
        private static readonly Color SelectedEdgeColor = new Color32(68, 192, 255, 255);

        private State lastState;
        private Port inputPort;
        private Port outputPort;
        private GroupView groupView;
        private VisualElement decoratorContainer;
        private VisualElement serviceContainer;
        private VisualElement breakpointView;

        /// <summary>
        /// Wrap view constructor.
        /// </summary>
        /// <param name="node">Node reference.</param>
        public WrapView(BehaviourTreeGraph graph, Node node) : base(graph, node, AssetDatabase.GetAssetPath(AITreeSettings.instance.GetNodeUXML()))
        {
            decoratorContainer = this.Q<VisualElement>("decorators-container");
            serviceContainer = this.Q<VisualElement>("services-container");
            breakpointView = this.Q<VisualElement>("breakpoint");

            InitializeInputPorts();
            InitializeOutputPorts();

            GetNode().GetBehaviourTree().Updating += UpdateVisualization;

            UpdateBreakpointView();
        }

        /// <summary>
        /// Called once when loading node view to initialize styles.
        /// </summary>
        protected override void InitializeStyles()
        {
            if (GetNode() is CompositeNode)
            {
                AddToClassList("composite");
            }
            else if (GetNode() is RootNode)
            {
                AddToClassList("root");
            }
            else if(GetNode() is TempTask)
            {
                AddToClassList("temp-task");
            }
            else if (GetNode() is TaskNode)
            {
                AddToClassList("task");
            }
        }

        /// <summary>
        /// Called once when loading node view to update capabilities.
        /// </summary>
        protected override void UpdateCapabilities()
        {
            base.UpdateCapabilities();
            if (!AITreeSettings.instance.AllowNodeSnapping())
            {
                capabilities &= ~Capabilities.Snappable;
            }
        }

        /// <summary>
        /// Set node position.
        /// </summary>
        /// <param name="newPos">New position.</param>
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(GetNode(), "[BehaviourTree] Move node");
            GetNode().SetNodePosition(newPos.position);
            EditorUtility.SetDirty(GetNode());
        }

        /// <summary>
        /// Called once when loading node view to initialize input ports.
        /// </summary>
        protected virtual void InitializeInputPorts()
        {
            if (GetNode() is CompositeNode)
            {
                inputPort = new NodePort(GetGraph(), Direction.Input, Port.Capacity.Single);
            }
            else if (GetNode() is TaskNode)
            {
                inputPort = new NodePort(GetGraph(), Direction.Input, Port.Capacity.Single);
            }

            if (inputPort != null)
            {
                inputPort.portName = "";
                inputPort.style.flexDirection = FlexDirection.Column;
                inputContainer.Add(inputPort);
            }
        }

        /// <summary>
        /// Called once when loading node view to initialize output ports.
        /// </summary>
        protected virtual void InitializeOutputPorts()
        {
            if (GetNode() is CompositeNode)
            {
                outputPort = new NodePort(GetGraph(), Direction.Output, Port.Capacity.Multi);
            }
            else if (GetNode() is RootNode)
            {
                outputPort = new NodePort(GetGraph(), Direction.Output, Port.Capacity.Single);
            }

            if (outputPort != null)
            {
                outputPort.portName = "";
                outputPort.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(outputPort);
            }
        }

        /// <summary>
        /// Call to update edge state.
        /// </summary>
        protected void UpdateEdgeState(bool visualize)
        {
            for (int i = 0; i < inputContainer.childCount; i++)
            {
                VisualElement child = inputContainer[i];
                if (child is Port port)
                {
                    foreach (Edge edge in port.connections)
                    {
                        if (edge == null)
                        {
                            continue;
                        }

                        if (edge.selected)
                        {
                            edge.edgeControl.inputColor = SelectedEdgeColor;
                            edge.edgeControl.outputColor = SelectedEdgeColor;
                            edge.edgeControl.edgeWidth = 3;
                            continue;
                        }

                        edge.edgeControl.inputColor = RestingEdgeColor;
                        edge.edgeControl.outputColor = RestingEdgeColor;
                        edge.edgeControl.edgeWidth = 2;

                        if (visualize)
                        {
                            switch (GetNode().GetState())
                            {
                                case State.Success:
                                    edge.edgeControl.inputColor = SuccessEdgeColor;
                                    edge.edgeControl.outputColor = SuccessEdgeColor;
                                    break;
                                case State.Failure:
                                    edge.edgeControl.inputColor = FailureEdgeColor;
                                    edge.edgeControl.outputColor = FailureEdgeColor;
                                    break;
                                case State.Running:
                                    if (GetNode().IsStarted())
                                    {
                                        edge.edgeControl.inputColor = RunningEdgeColor;
                                        edge.edgeControl.outputColor = RunningEdgeColor;
                                        edge.edgeControl.edgeWidth = 3;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Call to update breakpoint view.
        /// </summary>
        public void UpdateBreakpointView()
        {
            breakpointView.style.display = GetNode().Breakpoint() ? DisplayStyle.Flex : DisplayStyle.None;
        }

        /// <summary>
        /// Call to update visualization.
        /// </summary>
        public void UpdateVisualization()
        {
            switch (lastState)
            {
                case State.Success:
                    RemoveFromClassList("success");
                    break;
                case State.Failure:
                    RemoveFromClassList("failure");
                    break;
                case State.Aborted:
                    RemoveFromClassList("aborted");
                    break;
                case State.Running:
                    RemoveFromClassList("running");
                    break;
            }

            if (GetNode().GetOrder() == -1 && GetNode().GetType() != typeof(RootNode))
            {
                UpdateEdgeState(false);
                return;
            }

            UpdateEdgeState(GetNode().Visualize());
            if (!GetNode().Visualize())
            {
                return;
            }

            switch (GetNode().GetState())
            {
                case State.Success:
                    AddToClassList("success");
                    break;
                case State.Failure:
                    AddToClassList("failure");
                    break;
                case State.Aborted:
                    AddToClassList("aborted");
                    break;
                case State.Running:
                    AddToClassList("running");
                    break;
            }

            lastState = GetNode().GetState();
        }

        public void SordChildren()
        {
            if (GetNode() is CompositeNode composite)
            {
                composite.GetChildren().Sort(Comparison);
            }

            int Comparison(Node lhs, Node rhs)
            {
                Vector2 lhsPos = lhs.GetNodePosition();
                Vector2 rhsPos = rhs.GetNodePosition();
                return lhsPos.x < rhsPos.x ? -1 : 1;
            }
        }

        /// <summary>
        /// All the input port's current connections.
        /// </summary>
        public IEnumerable<Edge> InConnections
        {
            get
            {
                return inputPort.connections;
            }
        }

        /// <summary>
        /// All the output port's current connections.
        /// </summary>
        public IEnumerable<Edge> OutConnections
        {
            get
            {
                return outputPort.connections;
            }
        }

        #region [Getter / Setter]
        public State GetLastState()
        {
            return lastState;
        }

        public Port GetInputPort()
        {
            return inputPort;
        }

        public Port GetOutputPort()
        {
            return outputPort;
        }

        public GroupView GetGroup()
        {
            return groupView;
        }

        internal void SetGroup(GroupView groupView)
        {
            this.groupView = groupView;
        }

        public VisualElement GetDecoratorContainer()
        {
            return decoratorContainer;
        }

        public VisualElement GetServiceContainer()
        {
            return serviceContainer;
        }

        public VisualElement GetBreakpointView()
        {
            return breakpointView;
        }
        #endregion
    }
}