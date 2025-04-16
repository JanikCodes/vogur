/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace RenownedGames.AITreeEditor
{
    internal class NodePort : Port
    {
        private class DefaultEdgeConnectorListener : IEdgeConnectorListener
        {
            private BehaviourTreeGraph graph;
            private GraphViewChange graphViewChange;
            private List<Edge> edgesToCreate;
            private List<GraphElement> edgesToDelete;

            public DefaultEdgeConnectorListener(BehaviourTreeGraph graph)
            {
                this.graph = graph;
                edgesToCreate = new List<Edge>();
                edgesToDelete = new List<GraphElement>();

                graphViewChange.edgesToCreate = edgesToCreate;
            }

            public void OnDropOutsidePort(Edge edge, Vector2 position)
            {
                Port outputPort = edge.output;
                if (outputPort == null)
                {
                    return;
                }

                Vector2 windowPosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                Vector2 localMousePosition = graph.ChangeCoordinatesToView(position);
                graph.OpenNodeCreationWindow(windowPosition, localMousePosition, nodeView =>
                {
                    if (nodeView is WrapView wrapView)
                    {
                        Edge edge = new Edge();
                        edge.input = wrapView.GetInputPort();
                        edge.output = outputPort;

                        OnDrop(graph, edge);
                    }
                });
            }

            public void OnDrop(GraphView graphView, Edge edge)
            {
                this.edgesToCreate.Clear();
                this.edgesToCreate.Add(edge);
                edgesToDelete.Clear();

                if (edge.input.capacity == Capacity.Single)
                {
                    foreach (Edge edgeToDelete in edge.input.connections)
                    {
                        if (edgeToDelete != edge)
                        {
                            edgesToDelete.Add(edgeToDelete);
                        }
                    }
                }
                    

                if (edge.output.capacity == Capacity.Single)
                {
                    foreach (Edge edgeToDelete in edge.output.connections)
                    {
                        if (edgeToDelete != edge)
                        {
                            edgesToDelete.Add(edgeToDelete);
                        }
                    }
                }
                    

                if (edgesToDelete.Count > 0)
                {
                    graphView.DeleteElements(edgesToDelete);
                }

                List<Edge> edgesToCreate = this.edgesToCreate;
                if (graphView.graphViewChanged != null)
                {
                    edgesToCreate = graphView.graphViewChanged(graphViewChange).edgesToCreate;
                }

                for (int i = 0; i < edgesToCreate.Count; i++)
                {
                    Edge e = edgesToCreate[i];
                    graphView.AddElement(e);

                    if (edge.input != null && edge.output != null)
                    {
                        edge.input.Connect(e);
                        edge.output.Connect(e);
                    }
                }
            }
        }

        /// <summary>
        /// Node port constructor.
        /// </summary>
        /// <param name="graph">Behaviour tree graph.</param>
        /// <param name="direction">Port direction.</param>
        /// <param name="capacity">Port capacity.</param>
        public NodePort(BehaviourTreeGraph graph, Direction direction, Capacity capacity) : base(Orientation.Vertical, direction, capacity, typeof(bool))
        {
            DefaultEdgeConnectorListener connectorListener = new DefaultEdgeConnectorListener(graph);
            m_EdgeConnector = new EdgeConnector<Edge>(connectorListener);

            this.AddManipulator(m_EdgeConnector);
        }

        /// <summary>
        /// Check if point is on top of port. Used for selection and hover.
        /// </summary>
        /// <param name="localPoint">The point.</param>
        /// <returns>True if the point is over the port.</returns>
        public override bool ContainsPoint(Vector2 localPoint)
        {
            Rect rect = new Rect(0, 0, layout.width, layout.height);
            return rect.Contains(localPoint);
        }
    }
}