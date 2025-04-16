/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("AITreeEditor")]

namespace RenownedGames.AITree
{
    [HideMonoScript]
    public abstract class Node : ScriptableObject, INode, ICloneableNode, IEntityName, IEntityDescription, IAbortable, IRestorable
    {
        [Title("Description", VerticalSpace = 2)]
        [SerializeField]
        private string nodeName = string.Empty;

#if UNITY_EDITOR
        [SerializeField]
        [HideInInspector]
        private Vector2 nodePosition;

        [SerializeField]
        [HideInInspector]
        private bool breakpoint;
#endif

        // Stored required properties.
#if UNITY_EDITOR
        private bool visualize;
#endif
        private bool started;
        private int order = -1;
        private State state = State.Running;
        private State lastState = State.Running;
        private Node parent;
        private BehaviourRunner owner;
        private BehaviourTree tree;

#if UNITY_EDITOR
        /// <summary>
        /// Editor only method to initialize node in behaviour tree.
        /// </summary>
        /// <param name="tree">Behaviour tree reference. (Owner of this node.)</param>
        /// <param name="order">Node order in behaviour tree.</param>
        internal void Initialize(BehaviourTree tree, int order)
        {
            this.tree = tree;
            this.order = order;

            SetupParentReference();
            OnInspectorChanged();
        }
#endif

        /// <summary>
        /// Initialize node in behaviour tree.
        /// </summary>
        /// <param name="owner">BehaviourRunner owner of node.</param>
        /// <param name="tree">Behaviour tree reference. (Owner of this node.)</param>
        /// <param name="order">Node order in behaviour tree.</param>
        internal void Initialize(BehaviourRunner owner, BehaviourTree tree, int order)
        {
            this.owner = owner;
            this.tree = tree;
            this.order = order;
            SetupParentReference();
            OnInitialize();
        }

        /// <summary>
        /// Sets a reference to the parent node.
        /// </summary>
        internal virtual void SetupParentReference() { }

        /// <summary>
        /// Called on behaviour tree is awake.
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        protected internal virtual void OnStart() { }

        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        protected virtual void OnEntry() { }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected abstract State OnUpdate();

        /// <summary>
        /// Called when behaviour tree exit from node.
        /// </summary>
        protected virtual void OnExit() { }

        /// <summary>
        /// Called when behaviour tree aborts node.
        /// </summary>
        protected virtual void OnAbort() { }

        /// <summary>
        /// Set parent of the node.
        /// </summary>
        /// <param name="parent">New node parent reference.</param>
        protected internal virtual void SetParent(Node parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Set state of the node.
        /// </summary>
        /// <param name="state"></param>
        protected void SetState(State state)
        {
            if (lastState != state || this.state != state)
            {
                this.state = state;
                StateChanged?.Invoke(state);
                lastState = state;
            }
        }

        /// <summary>
        /// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
        /// </summary>
        public virtual void OnDrawGizmos() { }

        /// <summary>
        /// Implement OnDrawGizmosSelected to draw a gizmo if the object is selected.
        /// </summary>
        public virtual void OnDrawGizmosSelected() { }

        /// <summary>
        /// Implement OnDrawGizmosNodeSelected to draw the gizmo if the node is selected in the editor
        /// </summary>
        public virtual void OnDrawGizmosNodeSelected() { }

        /// <summary>
        /// Traverse through all next linked nodes with visitor callback.
        /// </summary>
        public virtual void Traverse(Action<Node> visiter)
        {
            visiter(this);
        }

        /// <summary>
        /// Progress of node execution.
        /// </summary>
        /// <returns>If node support progress line, return value of normalized progress. Otherwise null.</returns>
        protected internal virtual float? GetProgress()
        {
            return null;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Editor only method called when the value in the inspector or changes.
        /// </summary>
        [OnObjectChanged(DelayCall = true)]
        protected virtual void OnInspectorChanged()
        {
            InspectorChanged?.Invoke();
        }
#endif

        #region [WiP]
        internal virtual void OnClone(CloneData cloneData) { }
        #endregion

        #region [INode Implementation]
        /// <summary>
        /// Updating state of the node in behaviour tree execution order.
        /// </summary>
        /// <returns>State of node after updating.</returns>
        public virtual State Update()
        {
            if (!started)
            {
#if UNITY_EDITOR
                if (breakpoint)
                {
                    UnityEditor.EditorApplication.isPaused = true;
                }
#endif
                OnEntry();
                Started?.Invoke();
                started = true;
            }

#if UNITY_EDITOR
            visualize = true;
#endif

            state = State.Running;
            State nextState = OnUpdate();
            if (state == State.Aborted)
            {
                SetState(state);
            }
            else
            {
                SetState(nextState);
            }

            Updating?.Invoke(state);

            if (started && (state == State.Success || state == State.Failure))
            {
                OnExit();
                Completed?.Invoke(state);
                started = false;
            }

            return state;
        }
        #endregion

        #region [IClonableNode Implementation]
        /// <summary>
        /// Clone node with all its stuff.
        /// </summary>
        /// <returns>New cloned copy of node.</returns>
        public virtual Node Clone()
        {
            return Instantiate(this);
        }
        #endregion

        #region [IEntityName Implementation]
        /// <summary>
        /// Name of entity.
        /// </summary>
        public string GetName()
        {
            return nodeName;
        }
        #endregion

        #region [IEntityDescription Implementation]
        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public virtual string GetDescription()
        {
            return string.Empty;
        }
        #endregion

        #region [IAbortable Implementation]
        /// <summary>
        /// Aborts execution.
        /// </summary>
        public void Abort()
        {
            Traverse(n =>
            {
                n.OnAbort();
                n.state = State.Running;

                if (n.started)
                {
                    n.SetState(State.Aborted);
                    n.IsStarted(false);
                    n.OnExit();
                }
            });
        }

        /// <summary>
        /// Aborts execution at given order.
        /// </summary>
        public void AbortByOrder(int order)
        {
            Traverse(n =>
            {
                n.OnAbort();
                if (n.GetOrder() >= order)
                {
                    n.SetState(State.Running);

                    if (n.IsStarted())
                    {
                        n.SetState(State.Aborted);
                        n.IsStarted(false);
                        n.OnExit();
                    }
                }
            });
        }
        #endregion

        #region [IRestorable Implementation]
        /// <summary>
        /// Traverse resets parameters to their original values.
        /// </summary>
        public void Restore()
        {
            Restore(State.Running);
        }

        /// <summary>
        /// Traverse resets parameters to specific state.
        /// </summary>
        public void Restore(State state)
        {
            Traverse(n =>
            {
                if (n.started)
                {
                    n.SetState(state);
                    n.IsStarted(false);
                    n.OnExit();
                }
            });
        }
        #endregion

        #region [Event Callback Functions]
        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        public event Action Started;

        /// <summary>
        /// Called when node state has been changed.
        /// </summary>
        public event Action<State> StateChanged;

        /// <summary>
        /// Called every update time while the behavior tree is being executed by the node.
        /// </summary>
        public event Action<State> Updating;

        /// <summary>
        /// Called when the behavior tree exits the node with the specified state.
        /// </summary>
        public event Action<State> Completed;

#if UNITY_EDITOR
        /// <summary>
        /// Editor only callback called when the value in the inspector changes.
        /// </summary>
        public event Action InspectorChanged;
#endif
        #endregion

        #region [Getter / Setter]
        public void SetName(string name)
        {
            nodeName = name;
        }

        public BehaviourRunner GetOwner()
        {
            return owner;
        }

        public BehaviourTree GetBehaviourTree()
        {
            return tree;
        }

        public State GetState()
        {
            return state;
        }

        public bool IsStarted()
        {
            return started;
        }

        protected void IsStarted(bool started)
        {
            this.started = started;
        }

#if UNITY_EDITOR
        /// <summary>
        /// <b>Editor only.</b> Return node position in the behaviour tree.
        /// </summary>
        public Vector2 GetNodePosition()
        {
            return nodePosition;
        }

        /// <summary>
        /// <b>Editor only.</b> Set node position in the behaviour tree.
        /// </summary>
        public void SetNodePosition(Vector2 value)
        {
            nodePosition = value;
        }

        /// <summary>
        /// <b>Editor only.</b> Breakpoint mark of this node.
        /// </summary>
        public bool Breakpoint()
        {
            return breakpoint;
        }

        /// <summary>
        /// <b>Editor only.</b> Breakpoint mark of this node.
        /// </summary>
        public void Breakpoint(bool value)
        {
            breakpoint = value;
        }

        /// <summary>
        /// <b>Editor only.</b> Visualize node state in behaviour tree graph in the next callback.
        /// </summary>
        public bool Visualize()
        {
            return visualize;
        }

        /// <summary>
        /// <b><b>Editor only.</b></b> Visualize node state in behaviour tree graph in the next callback.
        /// </summary>
        public void Visualize(bool value)
        {
            visualize = value;
        }
        #endif

        public int GetOrder()
        {
            return order;
        }

        protected void SetOrder(int order)
        {
            this.order = order;
        }

        public float GetDeltaTime()
        {
            switch (tree.GetUpdateMode())
            {
                default:
                case UpdateMode.Update:
                case UpdateMode.LateUpdate:
                    return Time.deltaTime;
                case UpdateMode.FixedUpdate:
                    return Time.fixedDeltaTime;
                case UpdateMode.Custom:
                    return Time.deltaTime * tree.GetTickRate();
            }
        }

        public Node GetParent()
        {
            return parent;
        }
        #endregion
    }
}
