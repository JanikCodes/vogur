/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using RenownedGames.ExLib.Reflection;
using RenownedGames.ExLibEditor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Group = RenownedGames.AITree.Group;
using Node = RenownedGames.AITree.Node;
using NodeElement = UnityEditor.Experimental.GraphView.Node;
using Object = UnityEngine.Object;

namespace RenownedGames.AITreeEditor
{
    public class BehaviourTreeGraph : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviourTreeGraph, UxmlTraits> { }

        private class CopyData
        {
            public enum Type
            {
                Default,
                Auxiliary
            }

            public Type type;
            public HashSet<Node> nodesToCopy = new HashSet<Node>();
            public HashSet<Group> groupsToCopy = new HashSet<Group>();
            public HashSet<Note> notesToCopy = new HashSet<Note>();

            public Rect bounds;
        }

        private class ArrangeBox
        {
            private BehaviourTreeGraph graph;

            private List<ArrangeBox> children;

            private NodeView view;
            private Vector2 selfSize;
            private Vector2 size;

            public ArrangeBox(BehaviourTreeGraph graph, NodeView view)
            {
                this.graph = graph;
                this.view = view;

                children = new List<ArrangeBox>();
                foreach (NodeView childView in GetChilderenViews())
                {
                    ArrangeBox childBox = new ArrangeBox(graph, childView);
                    children.Add(childBox);
                }
            }

            public void RecalculateSize(Vector2 margin)
            {
                if (view != null)
                {
                    children.ForEach(c => c.RecalculateSize(margin));
                    selfSize = view.GetPosition().size + margin;
                    size = Vector2.zero;

                    foreach (ArrangeBox box in children)
                    {
                        size.x += box.size.x;
                        size.y = Mathf.Max(size.y, box.size.y);
                    }

                    size.x = Mathf.Max(size.x, selfSize.x);
                    size.y += selfSize.y;
                }
            }

            public void RecalculatePosition(Vector2 origin)
            {
                if (view != null)
                {
                    Vector2 position = Vector2.zero;
                    position.x = origin.x + size.x / 2f - selfSize.x / 2f;
                    position.y = origin.y;
                    view.SetPosition(new Rect(position, selfSize));

                    float childWidth = 0;
                    foreach (ArrangeBox child in children)
                    {
                        childWidth += child.size.x;
                    }

                    if (size.x > childWidth)
                    {
                        origin.x += (size.x - childWidth) / 2f;
                    }

                    origin.y += selfSize.y;

                    foreach (ArrangeBox child in children)
                    {
                        child.RecalculatePosition(origin);
                        origin.x += child.size.x;
                    }
                }
            }

            private List<NodeView> GetChilderenViews()
            {
                List<NodeView> childrenViews = new List<NodeView>();

                if (view != null)
                {
                    if (view.GetNode() is CompositeNode composite)
                    {
                        foreach (Node node in composite.GetChildren())
                        {
                            NodeView childView = graph.FindNodeView(node);
                            childrenViews.Add(childView);
                        }
                    }
                    else if (view.GetNode() is RootNode root)
                    {
                        NodeView childView = graph.FindNodeView(root.GetChild());
                        childrenViews.Add(childView);
                    }
                }

                return childrenViews;
            }
        }

        private Vector2 mousePosition;
        private CopyData copyData;
        private List<GroupView> groups;
        private List<NoteView> notes;
        private BehaviourTree behaviourTree;
        private AITreeSettings settings;

        /// <summary>
        /// Behaviour tree graph constructor.
        /// </summary>
        public BehaviourTreeGraph() : base()
        {
            groups = new List<GroupView>();
            notes = new List<NoteView>();

            settings = AITreeSettings.instance;
            styleSheets.Add(settings.GetBehaviourTreeUSS());
            Insert(0, new GridBackground());

            foreach (Manipulator manipulator in Manipulators)
            {
                this.AddManipulator(manipulator);
            }

            if (settings.ShowMiniMap())
            {
                Add(new MiniMapView() { windowed = true, graphView = this });
            }

            EditorApplication.update -= DeleteListener;
            EditorApplication.update += DeleteListener;

            nodeCreationRequest += OnNodeCreation;
            serializeGraphElements = OnCopyOperation;
            unserializeAndPaste = OnPasteOperation;
            canPasteSerializedData = OnCanPasteOperation;

            Undo.undoRedoPerformed -= OnUndoRedo;
            Undo.undoRedoPerformed += OnUndoRedo;
        }

        /// <summary>
        /// Add element to selection.
        /// </summary>
        /// <param name="selectable">Element to add to selection.</param>
        public override void AddToSelection(ISelectable selectable)
        {
            if (selectable is AuxiliaryView)
            {
                if (selection.Any(s => s is WrapView))
                {
                    return;
                }
            }
            else if (selectable is WrapView wrapView)
            {
                if (selection.Any(s => s is AuxiliaryView))
                {
                    ClearSelection();
                }
            }

            base.AddToSelection(selectable);
        }

        /// <summary>
        /// Get all ports compatible with given port.
        /// </summary>
        /// <param name="startPort">Start port to validate against.</param>
        /// <param name="nodeAdapter">Node adapter.</param>
        /// <returns>List of compatible ports.</returns>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            void TracePorts(List<Port> acceptedPorts, Port startPort)
            {
                Port oppositePort = GetOppositePort(startPort);
                if (oppositePort == null) return;

                acceptedPorts.Remove(oppositePort);

                foreach (Edge edge in oppositePort.connections)
                {
                    Port port = GetEdgePort(edge, startPort.direction);
                    TracePorts(acceptedPorts, port);
                }
            }

            List<Port> acceptedPorts = ports.ToList();
            TracePorts(acceptedPorts, startPort);

            for (int i = 0; i < acceptedPorts.Count; i++)
            {
                Port endPort = acceptedPorts[i];
                if (endPort.direction == startPort.direction || endPort.node == startPort.node)
                {
                    i--;
                    acceptedPorts.Remove(endPort);
                }
            }

            return acceptedPorts;
        }

        /// <summary>
        /// Add menu items to the contextual menu.
        /// </summary>
        /// <param name="evt">The event holding the menu to populate.</param>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Vector2 localMousePosition = ChangeCoordinatesToView(evt.localMousePosition);
            Vector2 windowPosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);

            if (behaviourTree == null)
            {
                evt.menu.AppendAction("Create new BehaviourTree", a =>
                {
                    BehaviourTree behaviourTree = BehaviourTree.Create("Behaviour Tree");

                    Selection.activeObject = behaviourTree;
                    EditorGUIUtility.PingObject(behaviourTree);
                });
            }
            else if (evt.target is BehaviourTreeGraph)
            {
                evt.menu.AppendAction("Create Node", a => OpenNodeCreationWindow(windowPosition, localMousePosition));
                evt.menu.AppendAction("Create Group", a => CreateGroup(localMousePosition));
                evt.menu.AppendAction("Create Note", a => CreateNote(localMousePosition));

#if !UNITY_2023_1_OR_NEWER
                evt.menu.AppendSeparator();
                evt.menu.AppendAction("Auto Arrange", a => AutoArrange());
#endif
            }
            else if (evt.target is NodeView nodeView)
            {
                WrapNode wrapNode = nodeView.GetNode() as WrapNode;
                if (wrapNode != null)
                {
                    WrapView wrapView = nodeView as WrapView;
                    if(wrapView != null)
                    {
                        evt.menu.AppendAction("Add Decorator", a => OpenDecoratorCreationWindow(windowPosition, wrapNode));

                        evt.menu.AppendAction("Add Service", a => OpenSeviceCreationWindow(windowPosition, wrapNode));

                        if (wrapView.GetNode() is TaskNode taskNode)
                        {
                            evt.menu.AppendSeparator();

                            evt.menu.AppendAction("Breakpoint", a =>
                            {
                                taskNode.Breakpoint(!taskNode.Breakpoint());
                                wrapView.UpdateBreakpointView();
                            }, s =>
                            {
                                return taskNode.Breakpoint() ? DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal;
                            });
                        }
                    }
                }

                evt.menu.AppendSeparator();
                evt.menu.AppendAction("Edit Script", a => OpenNodeScript(nodeView.GetNode()));
            }
        }

        /// <summary>
        /// Called at 10 frames per second to give the inspector a chance to update.
        /// </summary>
        protected internal virtual void OnInspectorUpdate()
        {
            foreach (VisualElement element in nodes)
            {
                if (element is NodeView nodeView)
                {
                    nodeView.OnInspectorUpdate();
                }
            }
        }

        /// <summary>
        /// This function is called when the window is closed.
        /// </summary>
        protected internal virtual void OnClose()
        {
            Undo.undoRedoPerformed -= OnUndoRedo;
        }

        /// <summary>
        /// Called when the graph changes.
        /// </summary>
        protected virtual GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            bool changed = false;

            if (graphViewChange.elementsToRemove != null)
            {
                int group = Undo.GetCurrentGroup();

                changed = true;

                graphViewChange.elementsToRemove = graphViewChange.elementsToRemove
                    .OrderBy(WeightGraphElementByImportanceOfDeletion)
                    .ThenBy(WeightGraphElementBySequenceNumber)
                    .ToList();

                graphViewChange.elementsToRemove.ForEach(e =>
                {
                    if (e is Edge edge)
                    {
                        WrapView parent = edge.output.node as WrapView;
                        Node parentNode = parent.GetNode();

                        WrapView child = edge.input.node as WrapView;
                        Node childNode = child.GetNode();

                        if (parentNode != null && childNode != null)
                        {
                            RemoveChild(parentNode, childNode);
                        }
                    }

                    if (e is WrapView wrapView)
                    {
                        GroupView groupView = wrapView.GetGroup();
                        if (groupView != null)
                        {
                            groupView.RemoveElement(wrapView);
                        }
                        DeleteNode(wrapView.GetNode());
                    }

                    if (e is DecoratorView decoratorView)
                    {
                        if (decoratorView.GetNode() != null)
                        {
                            DeleteDecorator(decoratorView.GetNode() as DecoratorNode);
                        }
                    }

                    if (e is ServiceView serviceView)
                    {
                        if (serviceView.GetNode() != null)
                        {
                            DeleteService(serviceView.GetNode() as ServiceNode);
                        }
                    }

                    {
                        if (e is GroupView groupView)
                        {
                            DeleteGroup(groupView.GetGroup());
                        }
                    }

                    if (e is NoteView noteView)
                    {
                        DeleteNote(noteView.GetNote());
                    }
                });

                Undo.CollapseUndoOperations(group);
            }

            List<Edge> egdeToDelete = new List<Edge>();
            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(e =>
                {
                    WrapView parent = e.output.node as WrapView;
                    Node parentNode = parent.GetNode();

                    WrapView child = e.input.node as WrapView;
                    Node childNode = child.GetNode();

                    if (parentNode != null && childNode != null)
                    {
                        AddChild(parentNode, childNode);
                        changed = true;
                    }
                    else
                    {
                        egdeToDelete.Add(e);
                    }
                });
            }
            if (egdeToDelete.Count > 0)
            {
                DeleteElements(egdeToDelete);
            }

            SortNodes();

            if (changed)
            {
                AssetChanged?.Invoke();
                behaviourTree.InitializeNodes();
            }

            UpdateNodesSequenceNumber();

            return graphViewChange;
        }

        /// <summary>
        /// Check if graph element has view with null target reference.
        /// </summary>
        public bool IsCorrectView()
        {
            foreach (GraphElement graphElement in graphElements)
            {
                if (graphElement is NodeView nodeView)
                {
                    if (nodeView.GetNode() == null)
                    {
                        return false;
                    }
                }

                if (graphElement is GroupView groupView)
                {
                    if (groupView.GetGroup() == null)
                    {
                        return false;
                    }
                }

                if (graphElement is NoteView noteView)
                {
                    if (noteView.GetNote() == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Automatic arrange of tree graph elements.
        /// </summary>
        public void AutoArrange()
        {
            Node node = behaviourTree.GetRootNode();
            NodeView nodeView = FindNodeView(node);
            AutoArrange(nodeView, Vector2.zero, Vector2.one * 39);
        }

        /// <summary>
        /// Updates the order of nodes based on their position.
        /// </summary>
        public void SortNodes()
        {
            foreach (NodeElement element in nodes)
            {
                if (element is WrapView wrapView)
                {
                    wrapView.SordChildren();
                }
            }
        }

        /// <summary>
        /// Changes the views for the selected behavior tree.
        /// </summary>
        public void PopulateView(BehaviourTree behaviourTree)
        {
            this.behaviourTree = behaviourTree;

            if (behaviourTree == null)
            {
                return;
            }

            // Lock node deletion from groups.
            if (graphElements != null)
            {
                foreach (GroupView groupView in graphElements.OfType<GroupView>())
                {
                    groupView.DeletionLocked(true);
                }
            }

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements.ToList());
            graphViewChanged += OnGraphViewChanged;

            behaviourTree.InitializeNodes();

            FixErrors(behaviourTree);

            CreateNodeViews();

            // Creating groups
            groups.Clear();
            List<Group> treeGroups = behaviourTree.GetGroups();
            for (int i = 0; i < treeGroups.Count; i++)
            {
                Group group = treeGroups[i];
                if (group != null)
                {
                    GroupView groupView = CreateGroupView(group);
                    for (int j = 0; j < group.nodes.Count; j++)
                    {
                        if (FindNodeView(group.nodes[j]) is WrapView wrapView && wrapView != null)
                        {
                            wrapView.SetGroup(groupView);
                            groupView.AddElement(wrapView);
                        }
                    }
                    AddElement(groupView);
                }
            }

            // Creating notes
            notes.Clear();
            List<Note> treeNotes = behaviourTree.GetNotes();
            for (int i = 0; i < notes.Count; i++)
            {
                Note note = treeNotes[i];
                if (note != null)
                {
                    NoteView noteView = CreateNoteView(note);
                    AddElement(noteView);
                }
            }

            UpdateNodesSequenceNumber();

            if (behaviourTree.IsRunning())
            {
                foreach (NodeElement nodeElement in nodes)
                {
                    if (nodeElement is WrapView wrapView)
                    {
                        wrapView.UpdateVisualization();
                    }
                }
            }
        }

        /// <summary>
        /// Clears the graph.
        /// </summary>
        public void ClearGraph()
        {
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements.ToList());

            Undo.ClearAll();
        }

        /// <summary>
        /// Automatic arrange of tree graph elements.
        /// </summary>
        internal void AutoArrange(NodeView rootView, Vector2 origin, Vector2 margin)
        {
            ArrangeBox box = new ArrangeBox(this, rootView);
            box.RecalculateSize(margin);
            box.RecalculatePosition(origin);
        }

        /// <summary>
        /// Opens a window for creating a node.
        /// </summary>
        internal void OpenNodeCreationWindow(Vector2 windowPosition, Vector2 nodePosition)
        {
            OpenNodeCreationWindow(windowPosition, nodePosition, null);
        }

        /// <summary>
        /// Opens a window for creating a node.
        /// </summary>
        internal void OpenNodeCreationWindow(Vector2 windowPosition, Vector2 nodePosition, Action<NodeView> onCreate)
        {
            ExSearchWindow nodeSearchWindow = ExSearchWindow.Create("Nodes");

            NodeTypeCache.NodeCollection nodes = NodeTypeCache.GetNodesInfo();
            for (int i = 0; i < nodes.Count; i++)
            {
                NodeTypeCache.NodeInfo nodeInfo = nodes[i];

                Type type = nodeInfo.type;
                if (!type.IsAbstract && !type.IsGenericType && type.IsSubclassOf(typeof(WrapNode)))
                {
                    NodeContentAttribute attribute = nodeInfo.contentAttribute;
                    if (attribute != null && !attribute.Hide)
                    {
                        GUIContent content = new GUIContent(nodeInfo.contentAttribute.path, nodeInfo.icon);
                        nodeSearchWindow.AddEntry(content, () => CreateNode(type, nodePosition, onCreate));
                    }
                }
            }
            nodeSearchWindow.Open(windowPosition);
        }

        /// <summary>
        /// Opens a window for creating a decorator.
        /// </summary>
        internal void OpenDecoratorCreationWindow(Vector2 windowPosition, WrapNode wrapNode)
        {
            OpenDecoratorCreationWindow(windowPosition, wrapNode, null);
        }

        /// <summary>
        /// Opens a window for creating a decorator.
        /// </summary>
        internal void OpenDecoratorCreationWindow(Vector2 windowPosition, WrapNode wrapNode, Action<DecoratorView> onCreate)
        {
            ExSearchWindow nodeSearchWindow = ExSearchWindow.Create("Decorators");

            NodeTypeCache.NodeCollection nodes = NodeTypeCache.GetNodesInfo();
            for (int i = 0; i < nodes.Count; i++)
            {
                NodeTypeCache.NodeInfo nodeInfo = nodes[i];

                Type type = nodeInfo.type;
                if (!type.IsAbstract && !type.IsGenericType && type.IsSubclassOf(typeof(DecoratorNode)))
                {
                    NodeContentAttribute attribute = nodeInfo.contentAttribute;
                    if (attribute != null && !attribute.Hide)
                    {
                        GUIContent content = new GUIContent(nodeInfo.contentAttribute.path, nodeInfo.icon);
                        nodeSearchWindow.AddEntry(content, () => CreateDecorator(wrapNode, type));
                    }
                }
            }
            nodeSearchWindow.Open(windowPosition);
        }

        /// <summary>
        /// Opens a window for creating a service.
        /// </summary>
        internal void OpenSeviceCreationWindow(Vector2 windowPosition, WrapNode wrapNode)
        {
            OpenSeviceCreationWindow(windowPosition, wrapNode, null);
        }

        /// <summary>
        /// Opens a window for creating a service.
        /// </summary>
        internal void OpenSeviceCreationWindow(Vector2 windowPosition, WrapNode wrapNode, Action<ServiceView> onCreate)
        {
            ExSearchWindow nodeSearchWindow = ExSearchWindow.Create("Services");

            NodeTypeCache.NodeCollection nodes = NodeTypeCache.GetNodesInfo();
            for (int i = 0; i < nodes.Count; i++)
            {
                NodeTypeCache.NodeInfo nodeInfo = nodes[i];

                Type type = nodeInfo.type;
                if (!type.IsAbstract && !type.IsGenericType && type.IsSubclassOf(typeof(ServiceNode)))
                {
                    NodeContentAttribute attribute = nodeInfo.contentAttribute;
                    if (attribute != null && !attribute.Hide)
                    {
                        GUIContent content = new GUIContent(nodeInfo.contentAttribute.path, nodeInfo.icon);
                        nodeSearchWindow.AddEntry(content, () => CreateService(wrapNode, type));
                    }
                }
            }
            nodeSearchWindow.Open(windowPosition);
        }

        /// <summary>
        /// Opens the node script with associated application.
        /// </summary>
        /// <param name="node">Node reference.</param>
        internal void OpenNodeScript(Node node)
        {
            MonoScript monoScript = MonoScript.FromScriptableObject(node);
            if(monoScript != null)
            {
                AssetDatabase.OpenAsset(monoScript);
            }
            else
            {
                Debug.LogError($"The script pointing to {node.GetType()} node not found.");
            }
        }

        /// <summary>
        /// Transforms a point from the local space of an element to the local space of another element.
        /// </summary>
        internal Vector2 ChangeCoordinatesToView(Vector2 mousePosition)
        {
            return this.ChangeCoordinatesTo(contentViewContainer, mousePosition);
        }

        /// <summary>
        /// Find node view by node reference.
        /// </summary>
        /// <param name="node">Node reference.</param>
        internal NodeView FindNodeView(Node node)
        {
            if (node != null)
            {
                return GetNodeByGuid(node.GetInstanceID().ToString()) as NodeView;
            }
            return null;
        }

        /// <summary>
        /// Find node views by node references.
        /// </summary>
        /// <param name="nodes">IEnumerable of node references.</param>
        internal List<TView> FindNodeViews<TView>(IEnumerable<Node> nodes) where TView : NodeView
        {
            List<TView> result = new List<TView>();
            foreach (Node node in nodes)
            {
                if (FindNodeView(node) is TView tView)
                {
                    result.Add(tView);
                }
            }
            return result;
        }

        /// <summary>
        /// Find group view by Group references.
        /// </summary>
        /// <param name="group">Group reference.</param>
        internal GroupView FindGroupView(Group group)
        {

            if (group != null)
            {
                return GetElementByGuid(group.GetInstanceID().ToString()) as GroupView;
            }
            return null;
        }

        /// <summary>
        /// Find note view by Note references.
        /// </summary>
        /// <param name="note">Note reference.</param>
        internal NoteView FindNoteView(Note note)
        {
            if (note != null)
            {
                return GetElementByGuid(note.GetInstanceID().ToString()) as NoteView;
            }
            return null;
        }

        /// <summary>
        /// Create group at given position.
        /// </summary>
        /// <param name="position">Position of group.</param>
        /// <returns>Created group view.</returns>
        internal GroupView CreateGroup(Vector2 position)
        {
            Group group = CreateGroupInstance();
            group.position = position;
            group.title = "Group";
            GroupView groupView = CreateGroupView(group);
            AddElement(groupView);
            return groupView;
        }

        /// <summary>
        /// Creates a view of all nodes with their connections.
        /// </summary>
        private void CreateNodeViews()
        {
            WrapView TraverseView(Node parent, List<Node> unconnectedNodes)
            {
                if (parent == null)
                {
                    return null;
                }

                unconnectedNodes.Remove(parent);
                WrapView parentView = CreateWrapView(parent);
                AddElement(parentView);

                if (parent is WrapNode wrap)
                {
                    List<DecoratorNode> decorators = wrap.GetDecorators();
                    for (int i = 0; i < decorators.Count; i++)
                    {
                        DecoratorNode decorator = decorators[i];
                        unconnectedNodes.Remove(decorator);
                        parentView.GetDecoratorContainer().Add(CreateDecoratorView(decorator));
                    }

                    List<ServiceNode> services = wrap.GetServices();
                    for (int i = 0; i < services.Count; i++)
                    {
                        ServiceNode service = services[i];
                        unconnectedNodes.Remove(service);
                        parentView.GetServiceContainer().Add(CreateServiceView(service));
                    }
                }

                List<Node> children = BehaviourTree.GetChildren(parent);
                for (int i = 0; i < children.Count; i++)
                {
                    Node node = children[i];
                    WrapView childView = TraverseView(node, unconnectedNodes);
                    if (childView != null && childView.GetInputPort() != null)
                    {
                        Edge edge = parentView.GetOutputPort().ConnectTo<Edge>(childView.GetInputPort());
                        AddElement(edge);
                    }
                }

                return parentView;
            }

            List<Node> unconnectedNodes = new List<Node>(behaviourTree.GetNodes());
            TraverseView(behaviourTree.GetRootNode(), unconnectedNodes);

            for (int i = 0; i < unconnectedNodes.Count; i++)
            {
                Node node = unconnectedNodes[i];
                if (node == null || node is AuxiliaryNode)
                {
                    continue;
                }

                WrapView parentView = CreateWrapView(node);
                AddElement(parentView);

                if (node is WrapNode wrap)
                {
                    List<DecoratorNode> decorators = wrap.GetDecorators();
                    for (int j = 0; j < decorators.Count; j++)
                    {
                        parentView.GetDecoratorContainer().Add(CreateDecoratorView(decorators[j]));
                    }

                    List<ServiceNode> services = wrap.GetServices();
                    for (int j = 0; j < services.Count; j++)
                    {
                        parentView.GetServiceContainer().Add(CreateServiceView(services[j]));
                    }
                }
            }

            for (int i = 0; i < unconnectedNodes.Count; i++)
            {
                Node node = unconnectedNodes[i];
                if (node is AuxiliaryNode)
                {
                    continue;
                }

                List<Node> children = BehaviourTree.GetChildren(node);
                for (int j = 0; j < children.Count; j++)
                {
                    Node child = children[j];

                    WrapView parentView = FindNodeView(node) as WrapView;
                    WrapView childView = FindNodeView(child) as WrapView;

                    Edge edge = parentView.GetOutputPort().ConnectTo<Edge>(childView.GetInputPort());
                    AddElement(edge);
                }
            }
        }

        /// <summary>
        /// Clone reference of view graph element.
        /// </summary>
        private T CloneElement<T>(T original) where T : ScriptableObject
        {
            T clone = ScriptableObject.CreateInstance(original.GetType()) as T;
            EditorUtility.CopySerialized(original, clone);

            if (clone is Node cloneNode)
            {
                foreach (FieldInfo fieldInfo in clone.GetType().AllFields(typeof(ScriptableObject)))
                {
                    if (fieldInfo.FieldType == typeof(Key) || fieldInfo.FieldType.IsSubclassOf(typeof(Key)))
                    {
                        Key key = (Key)fieldInfo.GetValue(cloneNode);
                        if (key != null && key.IsLocal())
                        {
                            Key instanceKey = ScriptableObject.CreateInstance(key.GetType()) as Key;
                            EditorUtility.CopySerialized(key, instanceKey);

                            if (!EditorApplication.isPlaying)
                            {
                                AssetDatabase.AddObjectToAsset(instanceKey, behaviourTree);
                            }

                            Undo.RegisterCreatedObjectUndo(instanceKey, "[BehaviourTree] Create clone key");

                            fieldInfo.SetValue(cloneNode, instanceKey);
                        }
                    }
                }
            }

            return clone;
        }

        /// <summary>
        /// Delete listener in editor update loop.
        /// </summary>
        private void DeleteListener()
        {
            Event evt = BTEventTracker.Current;
            if (evt != null)
            {
                if(evt.type == EventType.KeyDown 
                    && evt.keyCode == KeyCode.Backspace 
                    && evt.control
                    || (settings != null 
                    && settings.GetGraphHotKeyAPI() == AITreeSettings.HotKeyAPI.KeyDownListener 
                    && evt.type == EventType.Used 
                    && evt.keyCode == KeyCode.Delete))
                {
                    DeleteSelectionOperation("Delete", AskUser.AskUser);
                }
            }
        }

        private void CreateNode(Type type, Vector2 position, Action<NodeView> onCreate)
        {
            Node node = CreateNodeInstance(type);
            node.SetNodePosition(position);

            behaviourTree.InitializeNodes();

            WrapView wrapView = CreateWrapView(node, true);
            AddElement(wrapView);

            UpdateNodesSequenceNumber();

            onCreate?.Invoke(wrapView);
        }

        private Node CreateNodeInstance(Type type)
        {
            const string CREATE_NODE_RECORD_KEY = "[BehaviourTree] Create node";
            const string ADD_NODE_RECORD_KEY = "[BehaviourTree] Add node";

            Undo.IncrementCurrentGroup();

            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.SetName(ObjectNames.NicifyVariableName(type.Name));

            NodeContentAttribute nodeContentAttribute = node.GetType().GetCustomAttribute<NodeContentAttribute>();
            if (nodeContentAttribute != null)
            {
                if (!string.IsNullOrWhiteSpace(nodeContentAttribute.name))
                {
                    node.SetName(nodeContentAttribute.name);
                }
                else if (!string.IsNullOrWhiteSpace(nodeContentAttribute.path))
                {
                    node.SetName(System.IO.Path.GetFileName(nodeContentAttribute.path));
                }
            }

            if (!EditorApplication.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node, behaviourTree);
            }

            Undo.RegisterCreatedObjectUndo(node, CREATE_NODE_RECORD_KEY);

            Undo.RecordObject(behaviourTree, ADD_NODE_RECORD_KEY);
            behaviourTree.GetNodes().Add(node);

            Undo.SetCurrentGroupName(CREATE_NODE_RECORD_KEY);

            AssetChanged?.Invoke();
            return node;
        }

        private void DeleteNode(Node node)
        {
            const string DELETE_NODE_BT_RECORD_KEY = "[BehaviourTree] Delete node from BehaviourTree";
            const string DELETE_DECORATORS_BT_RECORD_KEY = "[BehaviourTree] Delete decorators from BehaviourTree";
            const string DELETE_SERVICES_BT_RECORD_KEY = "[BehaviourTree] Delete services from BehaviourTree";
            const string DELETE_NODE_RECORD_KEY = "[BehaviourTree] Delete node";

            Undo.IncrementCurrentGroup();

            List<DecoratorNode> decorators = null;
            List<ServiceNode> services = null;
            if (node is WrapNode wrapNode)
            {
                if (wrapNode.GetDecorators().Count > 0)
                {
                    decorators = new List<DecoratorNode>(wrapNode.GetDecorators());
                }

                if (wrapNode.GetServices().Count > 0)
                {
                    services = new List<ServiceNode>(wrapNode.GetServices());
                }
            }

            List<Key> instacedKeys = new List<Key>();
            foreach (FieldInfo fieldInfo in node.GetType().AllFields())
            {
                if (fieldInfo.FieldType == typeof(Key) || fieldInfo.FieldType.IsSubclassOf(typeof(Key)))
                {
                    Key key = fieldInfo.GetValue(node) as Key;
                    if (key != null && key.IsLocal())
                    {
                        instacedKeys.Add(key);
                    }
                }
            }

            Undo.RecordObject(behaviourTree, DELETE_NODE_BT_RECORD_KEY);
            behaviourTree.GetNodes().Remove(node);

            if (decorators != null)
            {
                Undo.RecordObject(behaviourTree, DELETE_DECORATORS_BT_RECORD_KEY);

                for (int i = 0; i < decorators.Count; i++)
                {
                    DeleteDecorator(decorators[i]);
                }
            }

            if (services != null)
            {
                Undo.RecordObject(behaviourTree, DELETE_SERVICES_BT_RECORD_KEY);

                for (int i = 0; i < services.Count; i++)
                {
                    DeleteService(services[i]);
                }
            }

            Undo.DestroyObjectImmediate(node);

            if (instacedKeys.Count > 0)
            {
                foreach (Key key in instacedKeys)
                {
                    Undo.DestroyObjectImmediate(key);
                }
            }

            Undo.SetCurrentGroupName(DELETE_NODE_RECORD_KEY);
        }

        private void AddChild(Node parent, Node child)
        {
            const string ADD_CHILD_RECORD_KEY = "[BehaviourTree] Add child";

            if (parent is CompositeNode composite)
            {
                Undo.RecordObject(composite, ADD_CHILD_RECORD_KEY);
                composite.AddChild(child);
                EditorUtility.SetDirty(composite);
            }

            if (parent is RootNode root)
            {
                Undo.RecordObject(root, ADD_CHILD_RECORD_KEY);
                root.SetChild(child);
                EditorUtility.SetDirty(root);
            }
        }

        private void RemoveChild(Node parent, Node child)
        {
            const string REMOVE_CHILD_RECORD_KEY = "[BehaviourTree] Remove child";

            if (parent is CompositeNode composite)
            {
                Undo.RecordObject(composite, REMOVE_CHILD_RECORD_KEY);
                composite.RemoveChild(child);
                EditorUtility.SetDirty(composite);
            }

            if (parent is RootNode root)
            {
                Undo.RecordObject(root, REMOVE_CHILD_RECORD_KEY);
                root.SetChild(null);
                EditorUtility.SetDirty(root);
            }
        }

        private void CreateDecorator(WrapNode wrapNode, Type decoratorType, Action<DecoratorView> onCreate)
        {
            DecoratorNode decorator = CreateDecoratorInstance(wrapNode, decoratorType);
            behaviourTree.InitializeNodes();

            DecoratorView decoratorView = CreateDecoratorView(decorator);
            (FindNodeView(wrapNode) as WrapView).GetDecoratorContainer().Add(decoratorView);

            UpdateNodesSequenceNumber();

            onCreate?.Invoke(decoratorView);
        }

        private void CreateDecorator(WrapNode wrapNode, Type decoratorType)
        {
            CreateDecorator(wrapNode, decoratorType, null);
        }

        private DecoratorNode CreateDecoratorInstance(WrapNode wrapNode, Type decoratorType)
        {
            const string CREATE_DECORATOR_RECORD_KEY = "[BehaviourTree] Create decorator";
            const string ADD_DECORATOR_BT_RECORD_KEY = "[BehaviourTree] Add decorator to BehaviourTree";
            const string ADD_DECORATOR_WN_RECORD_KEY = "[BehaviourTree] Add decorator to WrapNode";
            const string GR_CREATE_DECORATOR_RECORD_KEY = "[BehaviourTree] [Group] Create decorator";

            Undo.IncrementCurrentGroup();

            DecoratorNode decorator = ScriptableObject.CreateInstance(decoratorType) as DecoratorNode;
            decorator.name = decoratorType.Name;
            decorator.SetName(ObjectNames.NicifyVariableName(decoratorType.Name));

            NodeContentAttribute nodeContentAttribute = decorator.GetType().GetCustomAttribute<NodeContentAttribute>();
            if (nodeContentAttribute != null)
            {
                if (!string.IsNullOrWhiteSpace(nodeContentAttribute.name))
                {
                    decorator.SetName(nodeContentAttribute.name);
                }
                else if (!string.IsNullOrWhiteSpace(nodeContentAttribute.path))
                {
                    decorator.SetName(System.IO.Path.GetFileName(nodeContentAttribute.path));
                }
            }

            if (!EditorApplication.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(decorator, behaviourTree);
            }

            Undo.RegisterCreatedObjectUndo(decorator, CREATE_DECORATOR_RECORD_KEY);

            Undo.RecordObject(behaviourTree, ADD_DECORATOR_BT_RECORD_KEY);
            behaviourTree.GetNodes().Add(decorator);

            Undo.RecordObject(wrapNode, ADD_DECORATOR_WN_RECORD_KEY);
            wrapNode.AddDecorator(decorator);

            Undo.SetCurrentGroupName(GR_CREATE_DECORATOR_RECORD_KEY);

            AssetChanged?.Invoke();
            return decorator;
        }

        private void DeleteDecorator(DecoratorNode decorator)
        {
            const string DELETE_NODE_WN_RECORD_KEY = "[BehaviourTree] Delete node from WrapNode";
            const string DELETE_NODE_BT_RECORD_KEY = "[BehaviourTree] Delete node from BehaviourTree";
            const string DELETE_DECORATOR_RECORD_KEY = "[BehaviourTree] Delete decorator";

            Undo.IncrementCurrentGroup();

            DecoratorView decoratorView = FindNodeView(decorator) as DecoratorView;

            WrapNode wrapNode = PQ<WrapView>(decoratorView).GetNode() as WrapNode;

            List<Key> instacedKeys = new List<Key>();
            foreach (FieldInfo fieldInfo in decorator.GetType().AllFields())
            {
                if (fieldInfo.FieldType == typeof(Key) || fieldInfo.FieldType.IsSubclassOf(typeof(Key)))
                {
                    Key key = fieldInfo.GetValue(decorator) as Key;
                    if (key != null && key.IsLocal())
                    {
                        instacedKeys.Add(key);
                    }
                }
            }

            Undo.RecordObject(wrapNode, DELETE_NODE_WN_RECORD_KEY);
            wrapNode.RemoveDecorator(decoratorView.GetNode() as DecoratorNode);

            Undo.RecordObject(behaviourTree, DELETE_NODE_BT_RECORD_KEY);
            behaviourTree.GetNodes().Remove(decorator);

            Undo.DestroyObjectImmediate(decorator);

            if (instacedKeys.Count > 0)
            {
                foreach (Key key in instacedKeys)
                {
                    Undo.DestroyObjectImmediate(key);
                }
            }

            Undo.SetCurrentGroupName(DELETE_DECORATOR_RECORD_KEY);
        }

        private void CreateService(WrapNode wrapNode, Type serviceType)
        {
            CreateService(wrapNode, serviceType, null);
        }

        private void CreateService(WrapNode wrapNode, Type serviceType, Action<ServiceView> onCreate)
        {
            ServiceNode service = CreateServiceInstance(wrapNode, serviceType);
            behaviourTree.InitializeNodes();

            ServiceView serviceView = CreateServiceView(service);
            (FindNodeView(wrapNode) as WrapView).GetServiceContainer().Add(serviceView);

            UpdateNodesSequenceNumber();

            onCreate?.Invoke(serviceView);
        }

        private ServiceNode CreateServiceInstance(WrapNode wrapNode, Type serviceType)
        {
            const string CREATE_SERVICE_RECORD_KEY = "[BehaviourTree] Create service";
            const string ADD_SERVICE_BT_RECORD_KEY = "[BehaviourTree] Add service to BehaviourTree";
            const string ADD_SERVICE_WN_RECORD_KEY = "[BehaviourTree] Add service to WrapNode";
            const string GR_CREATE_SERVICE_RECORD_KEY = "[BehaviourTree] [Group] Create service";

            Undo.IncrementCurrentGroup();

            ServiceNode service = ScriptableObject.CreateInstance(serviceType) as ServiceNode;
            service.name = serviceType.Name;
            service.SetName(ObjectNames.NicifyVariableName(serviceType.Name));

            NodeContentAttribute nodeContentAttribute = service.GetType().GetCustomAttribute<NodeContentAttribute>();
            if (nodeContentAttribute != null)
            {
                if (!string.IsNullOrWhiteSpace(nodeContentAttribute.name))
                {
                    service.SetName(nodeContentAttribute.name);
                }
                else if (!string.IsNullOrWhiteSpace(nodeContentAttribute.path))
                {
                    service.SetName(System.IO.Path.GetFileName(nodeContentAttribute.path));
                }
            }

            if (!EditorApplication.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(service, behaviourTree);
            }

            Undo.RegisterCreatedObjectUndo(service, CREATE_SERVICE_RECORD_KEY);

            Undo.RecordObject(behaviourTree, ADD_SERVICE_BT_RECORD_KEY);
            behaviourTree.GetNodes().Add(service);

            Undo.RecordObject(wrapNode, ADD_SERVICE_WN_RECORD_KEY);
            wrapNode.AddService(service);

            Undo.SetCurrentGroupName(GR_CREATE_SERVICE_RECORD_KEY);

            AssetChanged?.Invoke();
            return service;
        }

        private void DeleteService(ServiceNode service)
        {
            const string DELETE_NODE_WN_RECORD_KEY = "[BehaviourTree] Delete node from WrapNode";
            const string DELETE_NODE_BT_RECORD_KEY = "[BehaviourTree] Delete node from BehaviourTree";
            const string DELETE_SERVICE_RECORD_KEY = "[BehaviourTree] Delete service";

            Undo.IncrementCurrentGroup();

            ServiceView serviceView = FindNodeView(service) as ServiceView;

            WrapNode wrapNode = PQ<WrapView>(serviceView).GetNode() as WrapNode;

            List<Key> instacedKeys = new List<Key>();
            foreach (FieldInfo fieldInfo in service.GetType().AllFields())
            {
                if (fieldInfo.FieldType == typeof(Key) || fieldInfo.FieldType.IsSubclassOf(typeof(Key)))
                {
                    Key key = fieldInfo.GetValue(service) as Key;
                    if (key != null && key.IsLocal())
                    {
                        instacedKeys.Add(key);
                    }
                }
            }

            Undo.RecordObject(wrapNode, DELETE_NODE_WN_RECORD_KEY);
            wrapNode.RemoveService(serviceView.GetNode() as ServiceNode);

            Undo.RecordObject(behaviourTree, DELETE_NODE_BT_RECORD_KEY);
            behaviourTree.GetNodes().Remove(service);

            Undo.DestroyObjectImmediate(service);

            if (instacedKeys.Count > 0)
            {
                foreach (Key key in instacedKeys)
                {
                    Undo.DestroyObjectImmediate(key);
                }
            }

            Undo.SetCurrentGroupName(DELETE_SERVICE_RECORD_KEY);
        }

        private Group CreateGroupInstance()
        {
            const string CREATE_GROUP_RECORD_KEY = "[BehaviourTree] Create group";
            const string ADD_GROUPE_RECORD_KEY = "[BehaviourTree] Add group";

            Undo.IncrementCurrentGroup();

            Group group = ScriptableObject.CreateInstance<Group>();
            group.name = "Group";

            if (!EditorApplication.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(group, behaviourTree);
            }

            Undo.RegisterCreatedObjectUndo(group, CREATE_GROUP_RECORD_KEY);

            Undo.RecordObject(behaviourTree, ADD_GROUPE_RECORD_KEY);
            behaviourTree.GetGroups().Add(group);

            Undo.SetCurrentGroupName(CREATE_GROUP_RECORD_KEY);

            AssetChanged?.Invoke();
            return group;
        }

        private void DeleteGroup(Group group)
        {
            const string DELETE_GROUP_BT_RECORD_KEY = "[BehaviourTree] Delete group from BehaviourTree";
            const string DELETE_GROUP_RECORD_KEY = "[BehaviourTree] Delete group";

            Undo.IncrementCurrentGroup();

            List<Node> nodes = null;
            if (group.nodes.Count > 0)
            {
                nodes = new List<Node>(group.nodes);
            }

            Undo.RecordObject(behaviourTree, DELETE_GROUP_BT_RECORD_KEY);
            behaviourTree.GetGroups().Remove(group);

            if (nodes != null && nodes.Count > 0)
            {
                GroupView groupView = FindGroupView(group);
                List<WrapView> wrapViews = FindNodeViews<WrapView>(nodes);
                groupView.RemoveElements(wrapViews);
            }

            Undo.DestroyObjectImmediate(group);

            Undo.SetCurrentGroupName(DELETE_GROUP_RECORD_KEY);
        }

        private void CreateNote(Vector2 position)
        {
            Note note = CreateNoteInstance();
            note.position = position;
            note.size = new Vector2(250, 150);
            note.title = "Note";
            note.contents = "Description";
            note.theme = StickyNoteTheme.Classic;
            note.fontSize = StickyNoteFontSize.Medium;
            AddElement(CreateNoteView(note));
        }

        private Note CreateNoteInstance()
        {
            const string CREATE_NOTE_RECORD_KEY = "[BehaviourTree] Create note";
            const string ADD_NOTE_RECORD_KEY = "[BehaviourTree] Add note";

            Undo.IncrementCurrentGroup();

            Note note = ScriptableObject.CreateInstance<Note>();
            note.name = "Note";

            if (!EditorApplication.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(note, behaviourTree);
            }

            Undo.RegisterCreatedObjectUndo(note, CREATE_NOTE_RECORD_KEY);

            Undo.RecordObject(behaviourTree, ADD_NOTE_RECORD_KEY);
            behaviourTree.GetNotes().Add(note);

            Undo.SetCurrentGroupName(CREATE_NOTE_RECORD_KEY);

            AssetChanged?.Invoke();
            return note;
        }

        private void DeleteNote(Note note)
        {
            const string DELETE_NOTE_BT_RECORD_KEY = "[BehaviourTree] Delete note from BehaviourTree";
            const string DELETE_NOTE_RECORD_KEY = "[BehaviourTree] Delete note";

            Undo.IncrementCurrentGroup();

            Undo.RecordObject(behaviourTree, DELETE_NOTE_BT_RECORD_KEY);
            behaviourTree.GetNotes().Remove(note);

            Undo.DestroyObjectImmediate(note);

            Undo.SetCurrentGroupName(DELETE_NOTE_RECORD_KEY);
        }

        private WrapView CreateWrapView(Node node, bool selectAfterCreate = false)
        {
            WrapView wrapView = new WrapView(this, node);
            wrapView.UpdateSelection += UpdateSelection;
            wrapView.ViewChanged += OnNodeViewChanged;

            ApplyTooltip(wrapView, node.GetType());

            if (selectAfterCreate)
            {
                wrapView.OnSelected();
            }

            return wrapView;
        }

        private DecoratorView CreateDecoratorView(DecoratorNode decorator)
        {
            DecoratorView decoratorView = new DecoratorView(this, decorator);
            decoratorView.UpdateSelection += UpdateSelection;
            decoratorView.ViewChanged += OnNodeViewChanged;
            ApplyTooltip(decoratorView, decorator.GetType());

            return decoratorView;
        }

        private ServiceView CreateServiceView(ServiceNode service)
        {
            ServiceView serviceView = new ServiceView(this, service);
            serviceView.UpdateSelection += UpdateSelection;
            serviceView.ViewChanged += OnNodeViewChanged;
            ApplyTooltip(serviceView, service.GetType());

            return serviceView;
        }

        private GroupView CreateGroupView(Group group)
        {
            GroupView groupView = new GroupView(this, group);
            groups.Add(groupView);
            return groupView;
        }

        private NoteView CreateNoteView(Note note)
        {
            NoteView noteView = new NoteView(this, note);
            notes.Add(noteView);
            return noteView;
        }

        private void ApplyTooltip(VisualElement visualElement, Type nodeType)
        {
            if((AITreeSettings.instance.GetNodeTooltipMode() & AITreeSettings.NodeTooltipMode.MouseOverlay) != 0)
            {
                if (NodeTypeCache.TryGetNodeInfo(nodeType, out NodeTypeCache.NodeInfo nodeInfo))
                {
                    if (nodeInfo.tooltipAttribute != null)
                    {
                        visualElement.tooltip = nodeInfo.tooltipAttribute.text;
                    }
                }
            }
        }

        private void UpdateNodesSequenceNumber()
        {
            nodes.ForEach(n =>
            {
                if (n is NodeView nodeView)
                {
                    if (!(nodeView.GetNode() is RootNode))
                    {
                        nodeView.AddToClassList("unconnected");
                    }

                    nodeView.SetSequenceNumber(-1, true);
                }
            });

            int sequenceNumber = 0;
            behaviourTree.GetRootNode().Traverse(n =>
            {
                if (n is RootNode)
                {
                    return;
                }

                NodeView nodeView = FindNodeView(n);
                if (nodeView != null)
                {
                    nodeView.RemoveFromClassList("unconnected");
                    nodeView.SetSequenceNumber(sequenceNumber++, true);
                }
            });

            nodes.ForEach(n =>
            {
                if (n is WrapView wrapView)
                {
                    if (wrapView.GetNode() is RootNode) return;

                    Node node = wrapView.GetNode();

                    sequenceNumber = 0;
                    if (wrapView.GetSequenceNumber() == -1)
                    {
                        node.Traverse(n =>
                        {
                            WrapView wrapView = FindNodeView(n) as WrapView;
                            if (wrapView != null)
                            {
                                if (wrapView.GetSequenceNumber() < sequenceNumber)
                                {
                                    wrapView.SetSequenceNumber(sequenceNumber++, false);
                                }
                            }
                        });
                    }
                }

            });
        }

        private Port GetOppositePort(Port port)
        {
            WrapView wrapView = port.node as WrapView;
            if (port.direction == Direction.Input)
            {
                return wrapView.GetOutputPort();
            }
            else
            {
                return wrapView.GetInputPort();
            }
        }

        private Port GetEdgePort(Edge edge, Direction direction)
        {
            if (direction == Direction.Input)
            {
                return edge.input;
            }
            else
            {
                return edge.output;
            }
        }

        /// <summary>
        /// Override this iteration to add new manipulators.
        /// </summary>
        protected virtual IEnumerable<Manipulator> Manipulators
        {
            get
            {
                yield return new ContentZoomer() { minScale = 0.25f, maxScale = 5.0f };
                yield return new ContentDragger();
                yield return new DoubleClickSelection(OnDoubleClickSelection);
                yield return new SelectionDragger();
                yield return new RectangleSelector();
                yield return new MultipleGrouping();
                yield return new MousePositionUpdater(this, (pos) => mousePosition = pos);
                yield return new AllSelector();
            }
        }

        #region [Callbacks]
        /// <summary>
        /// Called when undo/redo.
        /// </summary>
        private void OnUndoRedo()
        {
            SortNodes();
            PopulateView(behaviourTree);
            Focus();
            AssetChanged?.Invoke();
        }

        /// <summary>
        /// Callback for when the user requests to display the node creation window.
        /// </summary>
        private void OnNodeCreation(NodeCreationContext context)
        {
            if (behaviourTree != null)
            {
                Vector2 windowPosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                Vector2 localMousePosition = ChangeCoordinatesToView(GUIUtility.ScreenToGUIPoint(context.screenMousePosition));
                OpenNodeCreationWindow(windowPosition, localMousePosition);
            }
        }

        /// <summary>
        /// Called when the node view changes.
        /// </summary>
        private void OnNodeViewChanged()
        {
            PopulateView(behaviourTree);
        }

        /// <summary>
        /// Called by double-clicking.
        /// </summary>
        /// <param name="target">VisualElement clicked on.</param>
        private void OnDoubleClickSelection(VisualElement target)
        {
            NodeView nodeView = target as NodeView;
            if (nodeView == null)
            {
                nodeView = target.GetFirstAncestorOfType<NodeView>();
            }

            if (nodeView != null)
            {
                Node node = nodeView.GetNode();

                if (node is CompositeNode || node is RootNode)
                {
                    ClearSelection();
                    node.Traverse(n => AddToSelection(FindNodeView(n) as WrapView));
                }
                else if (node is RunBehaviourTask behaviourTask)
                {
                    BehaviourTree behaviourTree = behaviourTask.GetRunBehaviourTree();
                    if (behaviourTree != null)
                    {
                        ClearSelection();
                        Selection.activeObject = behaviourTree;
                    }
                }
                else if (node is SubTree subTree)
                {
                    if (EditorApplication.isPlaying)
                    {
                        BehaviourTreeWindow window = BehaviourTreeWindow.CreateWindow();
                        window.TrackEditor(subTree.GetSubTree());
                    }
                    else
                    {
                        BehaviourTree behaviourTree = subTree.GetSharedSubTree();
                        if (behaviourTree != null)
                        {
                            ClearSelection();
                            Selection.activeObject = behaviourTree;
                        }
                    }
                }
                else if (node is TaskNode)
                {
                    ClearSelection();
                    AddToSelection(FindNodeView(node));
                }
            }
        }

        /// <summary>
        /// Callback for serializing graph elements for copy/paste and other actions.
        /// </summary>
        private string OnCopyOperation(IEnumerable<GraphElement> elements)
        {
            CopyData data = new CopyData();

            HashSet<Node> nodes = new HashSet<Node>();

            bool hasWrapNode = false;

            foreach (GraphElement graphElement in elements)
            {
                if (graphElement is NodeView nodeView)
                {
                    Node node = nodeView.GetNode();
                    nodes.Add(node);

                    if (!hasWrapNode && node is WrapNode)
                    {
                        hasWrapNode = true;
                    }
                }

                if (graphElement is GroupView groupView)
                {
                    Group group = groupView.GetGroup();
                    data.groupsToCopy.Add(group);
                }

                if (graphElement is NoteView noteView)
                {
                    Note note = noteView.GetNote();
                    data.notesToCopy.Add(note);
                }
            }

            data.bounds = new Rect();
            foreach (Node node in nodes)
            {
                if (node is RootNode) continue;

                if (node is WrapNode wrap)
                {
                    data.nodesToCopy.Add(wrap);
                    WrapView wrapView = FindNodeView(wrap) as WrapView;
                    if (wrapView != null)
                    {
                        data.bounds = ExpandRect(data.bounds, wrapView.localBound);
                    }
                }

                if (node is AuxiliaryNode auxiliary)
                {
                    if (!hasWrapNode)
                    {
                        data.nodesToCopy.Add(auxiliary);
                    }
                }
            }

            foreach (Group group in data.groupsToCopy)
            {
                GroupView groupView = FindGroupView(group);
                if (groupView != null)
                {
                    data.bounds = ExpandRect(data.bounds, groupView.localBound);
                }
            }

            foreach (Note note in data.notesToCopy)
            {
                NoteView noteView = FindNoteView(note);
                if (noteView != null)
                {
                    data.bounds = ExpandRect(data.bounds, noteView.localBound);
                }
            }

            data.type = CopyData.Type.Default;
            if (data.nodesToCopy.Count != 0 && !hasWrapNode)
            {
                data.type = CopyData.Type.Auxiliary;
            }

            copyData = data;

            return string.Empty;
        }

        /// <summary>
        /// Callback for unserializing graph elements and adding them to the graph.
        /// </summary>
        private void OnPasteOperation(string operationName, string data)
        {
            if (copyData == null || copyData.nodesToCopy == null) return;

            Dictionary<Node, Node> createdNodes = new Dictionary<Node, Node>();
            Dictionary<Group, Group> createdGroups = new Dictionary<Group, Group>();
            Dictionary<Note, Note> createdNotes = new Dictionary<Note, Note>();

            if (copyData.type == CopyData.Type.Default)
            {
                Undo.IncrementCurrentGroup();

                // Create groups
                foreach (Group group in copyData.groupsToCopy)
                {
                    if (group == null)
                    {
                        Debug.LogWarning("The original group for cloning is missing");
                        continue;
                    }

                    Group cloneGroup = CloneElement<Group>(group);
                    createdGroups.Add(group, cloneGroup);

                    // Positioning
                    if (operationName == "Duplicate")
                    {
                        cloneGroup.position += new Vector2(10, 10);
                    }
                    else
                    {
                        Vector2 pivot = new Vector2(copyData.bounds.center.x, copyData.bounds.y);
                        cloneGroup.position += mousePosition - pivot;
                    }

                    // Remove nodes.
                    cloneGroup.nodes.Clear();

                    if (!EditorApplication.isPlaying)
                    {
                        AssetDatabase.AddObjectToAsset(cloneGroup, behaviourTree);
                    }

                    Undo.RegisterCreatedObjectUndo(cloneGroup, "[BehaviourTree] Create group");
                }

                // Add created groups to behaviour tree
                Undo.RecordObject(behaviourTree, "[BehaviourTree] Add groups to BehaviourTree");
                foreach (Group group in copyData.groupsToCopy)
                {
                    if (group == null) continue;
                    Group cloneGroup = createdGroups[group];
                    behaviourTree.GetGroups().Add(cloneGroup);
                }

                // Create notes
                foreach (Note note in copyData.notesToCopy)
                {
                    if (note == null)
                    {
                        Debug.LogWarning("The original note for cloning is missing");
                        continue;
                    }

                    Note cloneNote = CloneElement<Note>(note);
                    createdNotes.Add(note, cloneNote);

                    // Positioning
                    if (operationName == "Duplicate")
                    {
                        cloneNote.position += new Vector2(10, 10);
                    }
                    else
                    {
                        Vector2 pivot = new Vector2(copyData.bounds.center.x, copyData.bounds.y);
                        cloneNote.position += mousePosition - pivot;
                    }

                    if (!EditorApplication.isPlaying)
                    {
                        AssetDatabase.AddObjectToAsset(cloneNote, behaviourTree);
                    }

                    Undo.RegisterCreatedObjectUndo(cloneNote, "[BehaviourTree] Create note");
                }

                // Add created notes to behaviour tree
                Undo.RecordObject(behaviourTree, "[BehaviourTree] Add notes to BehaviourTree");
                foreach (Note note in copyData.notesToCopy)
                {
                    if (note == null) continue;
                    Note cloneNote = createdNotes[note];
                    behaviourTree.GetNotes().Add(cloneNote);
                }

                // Create node
                foreach (Node node in copyData.nodesToCopy)
                {
                    if (node == null)
                    {
                        Debug.LogWarning("The original node for cloning is missing");
                        continue;
                    }

                    WrapNode wrapNode = node as WrapNode;
                    WrapNode cloneNode = CloneElement<WrapNode>(wrapNode);
                    createdNodes.Add(node, cloneNode);

                    // Positioning
                    if (operationName == "Duplicate")
                    {
                        Vector2 clonePosition = cloneNode.GetNodePosition();
                        clonePosition += new Vector2(10, 10);
                        cloneNode.SetNodePosition(clonePosition);
                    }
                    else
                    {
                        Vector2 clonePosition = cloneNode.GetNodePosition();
                        Vector2 pivot = new Vector2(copyData.bounds.center.x, copyData.bounds.y);
                        clonePosition += mousePosition - pivot;
                        cloneNode.SetNodePosition(clonePosition);
                    }

                    // Remove connections.
                    if (cloneNode is CompositeNode composite)
                    {
                        composite.RemoveChildren();
                    }

                    if (!EditorApplication.isPlaying)
                    {
                        AssetDatabase.AddObjectToAsset(cloneNode, behaviourTree);
                    }

                    Undo.RegisterCreatedObjectUndo(cloneNode, "[BehaviourTree] Create node");

                    // Clone decorators
                    List<DecoratorNode> clonedDecorators = new List<DecoratorNode>();
                    foreach (DecoratorNode decorator in wrapNode.GetDecorators())
                    {
                        if (decorator == null)
                        {
                            Debug.LogWarning("The original decorator for cloning is missing");
                            continue;
                        }

                        DecoratorNode cloneDecorator = CloneElement<DecoratorNode>(decorator);
                        clonedDecorators.Add(cloneDecorator);

                        if (!EditorApplication.isPlaying)
                        {
                            AssetDatabase.AddObjectToAsset(cloneDecorator, behaviourTree);
                        }

                        Undo.RegisterCreatedObjectUndo(cloneDecorator, "[BehaviourTree] Create decorator");

                        Undo.RecordObject(behaviourTree, "[BehaviourTree] Add decorator to BehaviourTree");
                        behaviourTree.GetNodes().Add(cloneDecorator);
                    }
                    Undo.RecordObject(cloneNode, "[BehaviourTree] Set decorators for node");
                    cloneNode.SetDecorators(clonedDecorators);

                    // Clone services
                    List<ServiceNode> clonedServices = new List<ServiceNode>();
                    foreach (ServiceNode service in wrapNode.GetServices())
                    {
                        if (service == null)
                        {
                            Debug.LogWarning("The original service for cloning is missing");
                            continue;
                        }

                        ServiceNode cloneService = CloneElement<ServiceNode>(service);
                        clonedServices.Add(cloneService);

                        if (!EditorApplication.isPlaying)
                        {
                            AssetDatabase.AddObjectToAsset(cloneService, behaviourTree);
                        }

                        Undo.RegisterCreatedObjectUndo(cloneService, "[BehaviourTree] Create service");

                        Undo.RecordObject(behaviourTree, "[BehaviourTree] Add service to BehaviourTree");
                        behaviourTree.GetNodes().Add(cloneService);

                    }
                    Undo.RecordObject(cloneNode, "[BehaviourTree] Set services for node");
                    cloneNode.SetServices(clonedServices);
                }

                // Add created nodes to behaviour tree
                Undo.RecordObject(behaviourTree, "[BehaviourTree] Add nodes to BehaviourTree");
                foreach (Node node in copyData.nodesToCopy)
                {
                    if (node == null) continue;
                    Node cloneNode = createdNodes[node];
                    behaviourTree.GetNodes().Add(cloneNode);
                }

                // Revert nodes to group
                foreach (Group group in copyData.groupsToCopy)
                {
                    if (group == null) continue;

                    Group cloneGroup = createdGroups[group];

                    List<Node> nodes = new List<Node>();
                    foreach (Node node in group.nodes)
                    {
                        if (node == null) continue;
                        if (createdNodes.TryGetValue(node, out Node cloneNode))
                        {
                            nodes.Add(cloneNode);
                        }
                    }

                    Undo.RecordObject(cloneGroup, "[BehaviourTree] Add nodes to group");
                    cloneGroup.nodes = nodes;
                }

                // Revert node connections.
                foreach (Node node in copyData.nodesToCopy)
                {
                    if (node == null) continue;

                    if (node is CompositeNode composite)
                    {
                        CompositeNode cloneComposite = createdNodes[node] as CompositeNode;

                        List<Node> clonedChildren = new List<Node>();
                        foreach (Node child in composite.GetChildren())
                        {
                            if (createdNodes.TryGetValue(child, out Node cloneChild))
                            {
                                clonedChildren.Add(cloneChild);
                            }
                        }

                        Undo.RecordObject(cloneComposite, "[BehaviourTree] Set children for composite");
                        cloneComposite.SetChildren(clonedChildren);
                    }
                }

                Undo.SetCurrentGroupName("[BehaviourTree] Paste nodes");
            }
            else if (copyData.type == CopyData.Type.Auxiliary)
            {
                if (selection.Count != 1) return;

                WrapView wrapView = selection[0] as WrapView;
                if (wrapView == null) return;

                if (wrapView.GetNode() is RootNode) return;

                Undo.IncrementCurrentGroup();

                WrapNode selectedNode = wrapView.GetNode() as WrapNode;

                // Clone decorators
                List<DecoratorNode> clonedDecorators = new List<DecoratorNode>();
                foreach (Node node in copyData.nodesToCopy)
                {
                    if (node is DecoratorNode decorator)
                    {
                        if (decorator == null)
                        {
                            Debug.LogWarning("The original decorator for cloning is missing");
                            continue;
                        }

                        DecoratorNode cloneDecorator = CloneElement<DecoratorNode>(decorator);
                        createdNodes.Add(node, cloneDecorator);

                        if (!EditorApplication.isPlaying)
                        {
                            AssetDatabase.AddObjectToAsset(cloneDecorator, behaviourTree);
                        }

                        Undo.RegisterCreatedObjectUndo(cloneDecorator, "[BehaviourTree] Create decorator");
                        clonedDecorators.Add(cloneDecorator);
                    }
                }

                // Add cloned decorators to BehaviourTree
                Undo.RecordObject(behaviourTree, "[BehaviourTree] Add decorators to BehaviourTree");
                foreach (DecoratorNode cloneDecorator in clonedDecorators)
                {
                    behaviourTree.GetNodes().Add(cloneDecorator);
                }

                Undo.RecordObject(selectedNode, "[BehaviourTree] Add decorators to node");
                if (clonedDecorators.Count > 0)
                {
                    selectedNode.AddDecorators(clonedDecorators);
                }

                // Clone services
                List<ServiceNode> clonedServices = new List<ServiceNode>();
                foreach (Node node in copyData.nodesToCopy)
                {
                    if (node is ServiceNode service)
                    {
                        if (node == null)
                        {
                            Debug.LogWarning("The original service for cloning is missing");
                            continue;
                        }

                        ServiceNode cloneService = CloneElement<ServiceNode>(service);
                        createdNodes.Add(node, cloneService);

                        if (!EditorApplication.isPlaying)
                        {
                            AssetDatabase.AddObjectToAsset(cloneService, behaviourTree);
                        }

                        Undo.RegisterCreatedObjectUndo(cloneService, "[BehaviourTree] Create service");
                        clonedServices.Add(cloneService);
                    }
                }

                // Add cloned services to BehaviourTree
                Undo.RecordObject(behaviourTree, "[BehaviourTree] Add services to BehaviourTree");
                foreach (ServiceNode cloneService in clonedServices)
                {
                    behaviourTree.GetNodes().Add(cloneService);
                }

                Undo.RecordObject(selectedNode, "[BehaviourTree] Add services to node");
                if (clonedServices.Count > 0)
                {
                    selectedNode.AddServices(clonedServices);
                }

                Undo.SetCurrentGroupName("[BehaviourTree] Paste nodes");
            }

            if (operationName == "Duplicate")
            {
                copyData = null;
            }

            AssetChanged?.Invoke();

            PopulateView(behaviourTree);

            foreach (GraphElement graphElement in graphElements)
            {
                if (graphElement is NodeView nodeView)
                {
                    Node node = nodeView.GetNode();
                    if (createdNodes.ContainsValue(node))
                    {
                        AddToSelection(graphElement);
                    }
                }
            }
        }

        /// <summary>
        /// Ask whether or not the serialized data can be pasted.
        /// </summary>
        private bool OnCanPasteOperation(string data)
        {
            return true;
        }
        #endregion

        #region [Events]
        /// <summary>
        /// Called when selected or unselected node in graph.
        /// </summary>
        public event Action<Node> UpdateSelection;

        /// <summary>
        /// Called when asset has been changed.
        /// </summary>
        public event Action AssetChanged;
        #endregion

        #region [Static]
        internal static void FixErrors(BehaviourTree behaviourTree)
        {
            bool hasError = false;

            using (SerializedObject serializedObject = new SerializedObject(behaviourTree))
            {
                SerializedProperty nodes = serializedObject.FindProperty("nodes");
                for (int i = 0; i < nodes.arraySize; i++)
                {
                    SerializedProperty node = nodes.GetArrayElementAtIndex(i);
                    Object nodeObject = node.objectReferenceValue;
                    if (nodeObject == null)
                    {
                        nodes.DeleteArrayElementAtIndex(i);
                        i--;
                        hasError = true;
                    }
                    else if (nodeObject is AuxiliaryNode auxiliary && auxiliary.GetContainerNode() == null)
                    {
                        Object mainAsset = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(nodeObject));
                        AssetDatabase.RemoveObjectFromAsset(nodeObject);
                        Object.DestroyImmediate(nodeObject);
                        EditorUtility.SetDirty(mainAsset);
                        AssetDatabase.SaveAssetIfDirty(mainAsset);

                        nodes.DeleteArrayElementAtIndex(i);
                        i--;
                        hasError = true;
                    }
                    else if (nodeObject is WrapNode wrap)
                    {
                        using (SerializedObject wrapSerializedObject = new SerializedObject(wrap))
                        {
                            SerializedProperty decorators = wrapSerializedObject.FindProperty("decorators");
                            for (int j = 0; j < decorators.arraySize; j++)
                            {
                                SerializedProperty decorator = decorators.GetArrayElementAtIndex(j);
                                if (decorator.objectReferenceValue == null)
                                {
                                    decorators.DeleteArrayElementAtIndex(j);
                                    j--;
                                    hasError = true;
                                }
                            }

                            SerializedProperty services = wrapSerializedObject.FindProperty("services");
                            for (int j = 0; j < services.arraySize; j++)
                            {
                                SerializedProperty service = services.GetArrayElementAtIndex(j);
                                if (service.objectReferenceValue == null)
                                {
                                    services.DeleteArrayElementAtIndex(j);
                                    j--;
                                    hasError = true;
                                }
                            }

                            if (wrapSerializedObject.hasModifiedProperties)
                            {
                                wrapSerializedObject.ApplyModifiedProperties();
                            }
                        }
                    }
                }

                SerializedProperty notes = serializedObject.FindProperty("notes");
                for (int i = 0; i < notes.arraySize; i++)
                {
                    SerializedProperty note = notes.GetArrayElementAtIndex(i);
                    if (note.objectReferenceValue == null)
                    {
                        notes.DeleteArrayElementAtIndex(i);
                        i--;
                        hasError = true;
                    }
                }

                SerializedProperty groups = serializedObject.FindProperty("groups");
                for (int i = 0; i < groups.arraySize; i++)
                {
                    SerializedProperty group = groups.GetArrayElementAtIndex(i);
                    if (group.objectReferenceValue == null)
                    {
                        groups.DeleteArrayElementAtIndex(i);
                        i--;
                        hasError = true;
                    }
                }

                if (serializedObject.hasModifiedProperties)
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }

            if (hasError)
            {
                EditorApplication.delayCall += () =>
                {
                    if (EditorUtility.DisplayDialog("AI Tree - Behaviour Tree",
                        $"The tree had errors, the automatic control system correct them.\n\n" +

                        $"Possible reasons:\n" +
                        $"  Deleting a task, decorator or service class file that was in the tree.\n" +
                        $"  The task type has been changed to the type of decorator or service.\n" +
                        $"  The decorator or service type has been changed to the of type task.\n\n" +

                        $"Undefined types were automatically deleted, but references to their objects remained in the behaviour tree asset file.\n\n" +
                        "Expand the behaviour tree asset (the arrow to the right of the asset file) and manually delete the extra types assets.",
                        "Continue"))
                    {
                        EditorApplication.delayCall += () => EditorGUIUtility.PingObject(behaviourTree);
                    }
                };
            }
        }

        internal static T PQ<T>(VisualElement element) where T : class
        {
            if (element == null)
            {
                return null;
            }

            if (element is T t)
            {
                return t;
            }

            if (element.parent != null)
            {
                return PQ<T>(element.parent);
            }

            return null;
        }

        internal static int WeightGraphElementByImportanceOfDeletion(GraphElement element)
        {
            int c = 0;

            if (element is Edge)
            {
                c = 1;
            }
            else if (element is WrapView)
            {
                c = 2;
            }
            else if (element is DecoratorView)
            {
                c = 3;
            }

            return c;
        }

        internal static int WeightGraphElementBySequenceNumber(GraphElement element)
        {
            if (element is WrapView nodeView)
            {
                return -nodeView.GetSequenceNumber();
            }
            return 0;
        }

        internal static Rect ExpandRect(Rect a, Rect b)
        {
            if (a.width == 0 && a.height == 0)
            {
                return b;
            }

            float x = Mathf.Min(a.x, b.x);
            float y = Mathf.Min(a.y, b.y);
            float xMax = Mathf.Max(a.xMax, b.xMax);
            float yMax = Mathf.Max(a.yMax, b.yMax);
            return new Rect(x, y, xMax - x, yMax - y);
        }
        #endregion

        #region [Getter / Setter]
        public BehaviourTree GetBehaviourTree()
        {
            return behaviourTree;
        }
        #endregion
    }
}