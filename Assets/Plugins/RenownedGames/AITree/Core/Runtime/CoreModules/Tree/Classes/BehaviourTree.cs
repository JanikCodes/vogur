/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using RenownedGames.ExLib.Reflection;
using RenownedGames.Apex;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RenownedGames.AITree
{
    /// <summary>
    /// The base class defining the AI behavior tree. 
    /// </summary>
    [HideMonoScript]
    [CreateAssetMenu(fileName = "Behaviour Tree", menuName = "Renowned Games/AI Tree/Behaviour Tree")]
    public class BehaviourTree : ScriptableObject, ICloneableTree
    {
        [SerializeReference]
        private Blackboard blackboard;

        [SerializeField]
        private UpdateMode updateMode = UpdateMode.Update;

        [SerializeField]
        [ShowIf("updateMode", UpdateMode.Custom)]
        private int tickRate = 30;

        [SerializeField]
        [HideInInspector]
        private Node rootNode;

        [SerializeReference]
        [HideInInspector]
        private List<Node> nodes = new List<Node>();

        // Stored required properties.
        private bool running = false;
        private State state = State.Running;
        private Node currentNode;

#if UNITY_EDITOR
        [SerializeReference]
        [HideInInspector]
        private List<Group> groups = new List<Group>();

        [SerializeReference]
        [HideInInspector]
        private List<Note> notes = new List<Note>();

        [SerializeField]
        [HideInInspector]
        private Node selectedNode;
#endif

        /// <summary>
        /// Called when the behaviour tree object is loaded.
        /// </summary>
        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            InitializeNodes();
#endif
        }

        /// <summary>
        /// Called when the user hits the Reset button in the Inspector's context menu or when adding the component the first time.
        /// </summary>
        protected virtual void Reset()
        {
#if UNITY_EDITOR
            EditorApplication.update -= OnResetCallback;
            EditorApplication.update += OnResetCallback;
#endif
        }

        /// <summary>
        /// Implement this method if you want to draw gizmos that are also pickable and always drawn.
        /// </summary>
        protected internal void OnDrawGizmos()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].OnDrawGizmos();
            }

#if UNITY_EDITOR
            if (selectedNode != null)
            {
                selectedNode.OnDrawGizmosNodeSelected();
            }
#endif
        }

        /// <summary>
        /// Implement this method to draw a gizmos if the object is selected. Gizmos are drawn only when the object is selected.
        /// </summary>
        protected internal void OnDrawGizmosSelected()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].OnDrawGizmosSelected();
            }
        }

        /// <summary>
        /// Jump to specified node of behaviour tree.
        /// </summary>
        /// <param name="jumpNode">Node to jump.</param>
        internal void JumpToNode(Node jumpNode)
        {
            if (currentNode is TaskNode task)
            {
                if (task.IgnoreAbortSelf())
                {
                    return;
                }
            }

            rootNode.AbortByOrder(jumpNode.GetOrder());

            Node[] path = TraversePath(rootNode, jumpNode).ToArray();
            for (int i = 0; i < path.Length - 1; i++)
            {
                if (path[i] is CompositeNode composite)
                {
                    composite.JumpToNode(path[i + 1] as WrapNode);
                }
            }
        }

        /// <summary>
        /// Initialize all not synchronized keys declared in the node.
        /// </summary>
        /// <param name="node">Specified node.</param>
        /// <param name="keys">Reference of stored dictionary of not synchronized keys.</param>
        private void InitializeKeys(Node node, in Dictionary<string, Key> keys)
        {
            Type type = node.GetType();
            foreach (FieldInfo fieldInfo in type.AllFields(typeof(ScriptableObject)))
            {
                object value = fieldInfo.GetValue(node);
                if (value is Key key)
                {
                    if (key.IsLocal())
                    {
                        fieldInfo.SetValue(node, Instantiate(key));
                    }
                    else if (!key.IsSync())
                    {
                        SetLocalKey(node, fieldInfo, key.name, in keys);
                    }
                }
            }
        }

        /// <summary>
        /// Collect all not synchronized keys in dictionary.
        /// </summary>
        /// <param name="blackboard">Reference of associated blackboard.</param>
        private Dictionary<string, Key> GetAllLocalKeys(Blackboard blackboard)
        {
            Dictionary<string, Key> keys = new Dictionary<string, Key>();
            if (blackboard != null)
            {
                foreach (Key key in blackboard.Keys)
                {
                    keys[key.name] = key;
                }
            }
            return keys;
        }

        /// <summary>
        /// Set local key to key field info.
        /// </summary>
        /// <param name="targetObject">Declaring target object.</param>
        /// <param name="fieldInfo">Key field info.</param>
        /// <param name="keyName">Key name.</param>
        /// <param name="keys">Reference not synchronized keys dictionary.</param>
        private void SetLocalKey(object targetObject, FieldInfo fieldInfo, string keyName, in Dictionary<string, Key> keys)
        {
            if (keys.TryGetValue(keyName, out Key localKey))
            {
                fieldInfo.SetValue(targetObject, localKey);
            }
        }

        #region [IClonableTree Implementation]
        /// <summary>
        /// Clone behaviour tree with all its stuff.
        /// </summary>
        /// <returns>New cloned copy of behaviour tree.</returns>
        public BehaviourTree Clone()
        {
            BehaviourTree tree = Instantiate(this);
            tree.name = name;

//#if UNITY_EDITOR
//            // Clone nodes
//            List<Node> cloneNodes = new List<Node>();
//            Dictionary<Node, Node> cloneNodeMap = new Dictionary<Node, Node>();
//            for (int i = 0; i < nodes.Count; i++)
//            {
//                Node node = nodes[i];
//                Node cloneNode = Instantiate(node);
//                cloneNodes.Add(cloneNode);
//                cloneNodeMap.Add(node, cloneNode);
//            }
//            tree.nodes = cloneNodes;

//            // Clone groups
//            List<Group> cloneGroups = new List<Group>();
//            Dictionary<Group, Group> cloneGroupMap = new Dictionary<Group, Group>();
//            for (int i = 0; i < groups.Count; i++)
//            {
//                Group group = groups[i];
//                Group cloneGroup = Instantiate(group);
//                cloneGroups.Add(cloneGroup);
//                cloneGroupMap.Add(group, cloneGroup);
//            }
//            tree.groups = cloneGroups;

//            // Clone notes
//            List<Note> cloneNotes = new List<Note>();
//            Dictionary<Note, Note> cloneNoteMap = new Dictionary<Note, Note>();
//            for (int i = 0; i < notes.Count; i++)
//            {
//                Note note = notes[i];
//                Note cloneNote = Instantiate(note);
//                cloneNotes.Add(cloneNote);
//                cloneNoteMap.Add(note, cloneNote);
//            }
//            tree.notes = cloneNotes;

//            CloneData cloneData = new CloneData(cloneNodeMap, cloneGroupMap, cloneNoteMap);

//            // Revert node connections
//            tree.rootNode = cloneNodeMap[rootNode];
//            for (int i = 0; i < cloneNodes.Count; i++)
//            {
//                Node cloneNode = cloneNodes[i];
//                cloneNode.OnClone(cloneData);
//            }

//            for (int i = 0; i < cloneGroups.Count; i++)
//            {
//                Group cloneGroup = cloneGroups[i];
//                cloneGroup.OnClone(cloneData);
//            }
//#else
            // Clone only important nodes,
            // cutting off everything that is used only in the editor.
            if (rootNode != null)
            {
                tree.rootNode = rootNode.Clone();
                tree.nodes = new List<Node>();
                tree.rootNode.Traverse(tree.nodes.Add);
            }
//#endif

            return tree;
        }
        #endregion

        #region [Internal Callback Executions]
        /// <summary>
        /// Internal initialization of behaviour tree.
        /// </summary>
        /// <param name="owner">Reference of behaviour runner owner.</param>
        internal void Initialize(BehaviourRunner owner)
        {
            running = true;
            Dictionary<string, Key> keys = GetAllLocalKeys(owner.GetBlackboard());

            int order = -1;
            rootNode.Traverse(n =>
            {
                InitializeKeys(n, in keys);
                n.Initialize(owner, this, order++);
                n.Started += () => UpdateCurrentNode(n);
            });
        }

        /// <summary>
        /// Internal initialization of behaviour tree.
        /// </summary>
        /// <param name="owner">Reference of behaviour runner owner.</param>
        /// <param name="blackboard">Reference of custom blackboard.</param>
        internal void Initialize(BehaviourRunner owner, Blackboard blackboard)
        {
            running = true;
            Dictionary<string, Key> keys = GetAllLocalKeys(blackboard);

            int order = -1;
            rootNode.Traverse(n =>
            {
                InitializeKeys(n, in keys);
                n.Initialize(owner, this, order++);
                n.Started += () => UpdateCurrentNode(n);
            });
        }

        private void UpdateCurrentNode(Node node)
        {
            if(node is AuxiliaryNode auxiliary)
            {
                currentNode = auxiliary.GetContainerNode();
            }
            else
            {
                currentNode = node;
            }
        }

        /// <summary>
        /// Called on the frame when a BehaviourRunner is enabled,
        /// just before any of the Update methods are called the first time.
        /// </summary>
        protected internal void OnStart()
        {
            rootNode.Traverse(n => n.OnStart());
        }

        /// <summary>
        /// Called when updating behaviour tree state.
        /// </summary>
        protected internal State OnUpdate()
        {
            if (rootNode == null)
            {
                return State.Failure;
            }

#if UNITY_EDITOR
            ResetVisualization();
#endif

            state = rootNode.Update();

            Updating?.Invoke();

            return state;
        }

        /// <summary>
        /// Called when an enabled BehaviourRunner instance is being destroyed.
        /// </summary>
        protected internal void OnStop()
        {
            running = false;
        }
        #endregion

        #region [Editor-Only]
#if UNITY_EDITOR
        /// <summary>
        /// <b>This method is only called in editor mode!</b>
        /// <br></br>
        /// Internal nodes initialization of behaviour tree.
        /// </summary>
        internal void InitializeNodes()
        {
            if(rootNode == null)
            {
                return;
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                Node node = nodes[i];
                if (node != null)
                {
                    node.Initialize(this, -1);
                }
            }

            int order = -1;
            rootNode.Traverse(n => n.Initialize(this, order++));
        }

        /// <summary>
        /// Called in editor application update when triggered Reset() method.
        /// </summary>
        private void OnResetCallback()
        {
            if (this == null)
            {
                EditorApplication.update -= OnResetCallback;
                return;
            }

            if (AssetDatabase.IsNativeAsset(this))
            {
                EditorApplication.update -= OnResetCallback;
                CreateRootNode();
            }
        }

        /// <summary>
        /// Create default behaviour tree root node.
        /// </summary>
        private void CreateRootNode()
        {
            RootNode rootNode = CreateInstance<RootNode>();
            rootNode.name = "RootNode";
            this.rootNode = rootNode;

            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(rootNode, this);
            }

            nodes.Add(rootNode);

            AssetDatabase.SaveAssets();

            BecameNativeAsset?.Invoke(this);
        }

        /// <summary>
        /// Reset visualization state of nodes on false.
        /// </summary>
        private void ResetVisualization()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Visualize(false);
            }
        }
#endif
        #endregion

        #region [Static]
#if UNITY_EDITOR
        /// <summary>
        /// <b>This method is only called in editor mode!</b>
        /// <br></br>
        /// Create new behaviour tree asset in project.
        /// </summary>
        public static BehaviourTree Create(string name)
        {
            BehaviourTree behaviourTree = CreateInstance<BehaviourTree>();
            behaviourTree.name = name;

            string path = AssetDatabase.GenerateUniqueAssetPath($"Assets/{name}.asset");
            AssetDatabase.CreateAsset(behaviourTree, path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return behaviourTree;
        }
#endif
        /// <summary>
        /// Traverse path from target node to searching node.
        /// </summary>
        public static LinkedList<Node> TraversePath(Node node, Node searchingNode)
        {
            bool TraversePathLocal(LinkedList<Node> path, Node rootNode, Node searchingNode)
            {
                if (rootNode == searchingNode)
                {
                    path.AddFirst(rootNode);
                    return true;
                }
                else
                {
                    if (rootNode is WrapNode wrap && searchingNode is DecoratorNode decorator)
                    {
                        if (wrap.GetDecorators().Contains(decorator))
                        {
                            path.AddFirst(wrap);
                            return true;
                        }
                    }
                }

                foreach (Node child in IterateChildren(rootNode))
                {
                    if (TraversePathLocal(path, child, searchingNode))
                    {
                        path.AddFirst(rootNode);
                        return true;
                    }
                }

                return false;
            }

            LinkedList<Node> path = new LinkedList<Node>();
            if (TraversePathLocal(path, node, searchingNode))
            {
                return path;
            }

            return null;
        }

        /// <summary>
        /// Traverse all nodes from target node with visiter callback.
        /// </summary>
        [Obsolete("Use Node.Traverse() implementation.")]
        public static void Traverse(Node node, Action<Node> visiter)
        {
            if (node != null)
            {
                node.Traverse(visiter);
                //WrapNode wrapNode = node as WrapNode;
                //if (wrapNode != null)
                //{
                //    List<DecoratorNode> decorators = wrapNode.GetDecorators();
                //    for (int i = 0; i < decorators.Count; i++)
                //    {
                //        Traverse(decorators[i], visiter);
                //    }
                //}

                //visiter?.Invoke(node);

                //if (wrapNode != null)
                //{
                //    List<ServiceNode> services = wrapNode.GetServices();
                //    for (int i = 0; i < services.Count; i++)
                //    {
                //        Traverse(services[i], visiter);
                //    }
                //}

                //foreach (Node child in IterateChildren(node))
                //{
                //    Traverse(child, visiter);
                //}
            }
        }

        /// <summary>
        /// Get all children of node in List representation.
        /// </summary>
        public static List<Node> GetChildren(Node node)
        {
            if (node is CompositeNode compositeNode)
            {
                return compositeNode.GetChildren();
            }

            List<Node> children = new List<Node>();
            if (node is RootNode rootNode && rootNode.GetChild() != null)
            {
                children.Add(rootNode.GetChild());
            }

            return children;
        }

        /// <summary>
        /// Iterate through all children of node.
        /// </summary>
        public static IEnumerable<Node> IterateChildren(Node node)
        {
            if (node is CompositeNode composite)
            {
                List<Node> children = composite.GetChildren();
                for (int i = 0; i < children.Count; i++)
                {
                    yield return children[i];
                }
            }
            else if (node is RootNode root && root.GetChild() != null)
            {
                yield return root.GetChild();
            }
        }

        /// <summary>
        /// Check that node is sub node of other node.
        /// </summary>
        public static bool IsSubNode(Node node, Node checkNode)
        {
            int index = checkNode.GetOrder();
            int left = node.GetOrder();
            int right = left;

            if (node.GetParent() is CompositeNode composite)
            {
                Node next = composite.NextChild(node);
                if (next != null)
                {
                    right = next.GetOrder();
                }
            }

            if (left == right)
            {
                return true;
            }
            else
            {
                return left <= index && index < right;
            }
        }
        #endregion

        #region [Event Callback Function]
        /// <summary>
        /// Called every behaviour tree tick.
        /// </summary>
        public event Action Updating;

        /// <summary>
        /// <b>This action is only called in editor mode!</b>
        /// <br></br>
        /// Called when behaviour tree became native asset.
        /// </summary>
        internal event Action<BehaviourTree> BecameNativeAsset;
        #endregion

        #region [Getter / Setter]
        public Blackboard GetBlackboard()
        {
            return blackboard;
        }

        public UpdateMode GetUpdateMode()
        {
            return updateMode;
        }

        public void SetUpdateMode(UpdateMode updateMode)
        {
            this.updateMode = updateMode;
        }

        public int GetTickRate()
        {
            return tickRate;
        }

        public void SetTickRate(int count)
        {
            tickRate = count;
        }

        public bool IsRunning()
        {
            return running;
        }

        public Node GetRootNode()
        {
            return rootNode;
        }

        public List<Node> GetNodes()
        {
            return nodes;
        }

        public void SetNodes(List<Node> value)
        {
            nodes = value;
        }

        public Node GetCurrentNode()
        {
            return currentNode;
        }

#if UNITY_EDITOR
        /// <summary>
        /// <b>Editor only.</b> Return list of groups.
        /// </summary>
        public List<Group> GetGroups()
        {
            return groups;
        }

        /// <summary>
        /// <b>Editor only.</b> Set list of groups.
        /// </summary>
        public void SetGroups(List<Group> value)
        {
            groups = value;
        }

        /// <summary>
        /// <b>Editor only.</b> Return list of notes.
        /// </summary>
        public List<Note> GetNotes()
        {
            return notes;
        }

        /// <summary>
        /// <b>Editor only.</b> Set list of notes.
        /// </summary>
        public void SetNotes(List<Note> value)
        {
            notes = value;
        }

        /// <summary>
        /// <b>Editor only.</b> Return current selected node of behaviour tree in editor window graph.
        /// </summary>
        public Node GetSelectedNode()
        {
            return selectedNode;
        }

        /// <summary>
        /// <b>Editor only.</b> Set current selected node of behaviour tree in editor window graph.
        /// </summary>
        internal void SetSelectedNode(Node value)
        {
            selectedNode = value;
        }
#endif
        #endregion
    }
}
