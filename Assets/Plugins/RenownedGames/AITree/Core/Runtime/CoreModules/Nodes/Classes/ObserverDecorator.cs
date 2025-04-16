/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022 - 2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RenownedGames.AITree
{
    public abstract class ObserverDecorator : DecoratorNode
    {
        [Title("Flow Control")]
        [SerializeField]
        private NotifyObserver notifyObserver = NotifyObserver.OnResultChange;

        [SerializeField]
        [Enum(HideValues = nameof(GetHiddenObserverAborts))]
        [DisableInPlayMode]
        protected ObserverAbort observerAbort = ObserverAbort.None;

        // Stored required properties.
        private bool running;
        private bool lastResult;

        /// <summary>
        /// Called on behaviour tree is awake.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            OnValueChange += OnValueChangeAction;
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected sealed override State OnUpdate()
        {
            bool result = CalculateResult();

            if (!running)
            {
                lastResult = result;
            }

            if (!result && (observerAbort & ObserverAbort.Self) != 0)
            {
                return State.Failure;
            }

            if (result || running)
            {
                running = true;
                return UpdateChild();
            }

            return State.Failure;
        }

        /// <summary>
        /// Called every tick regardless of the node execution.
        /// </summary>
        protected override void OnFlowUpdate() { }

        /// <summary>
        /// Called when behaviour tree exit from node.
        /// </summary>
        protected override void OnExit()
        {
            base.OnExit();
            running = false;
        }

        /// <summary>
        /// Called when the value changes.
        /// </summary>
        private void OnValueChangeAction()
        {
            bool result = CalculateResult();

            switch (notifyObserver)
            {
                case NotifyObserver.OnValueChange:
                    OnNotifyObserver?.Invoke(result);
                    break;
                default:
                case NotifyObserver.OnResultChange:
                    if (lastResult != result)
                    {
                        OnNotifyObserver?.Invoke(result);
                    }
                    break;
            }

            lastResult = result;
        }

        /// <summary>
        /// Calculates the observed value of the result.
        /// </summary>
        public abstract bool CalculateResult();

        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = base.GetDescription();

            NodeContentAttribute content = GetType().GetCustomAttribute<NodeContentAttribute>();
            if (content != null)
            {
                string name = "Undefined";
                if (!string.IsNullOrWhiteSpace(content.name))
                {
                    name = content.name;
                }
                else if (!string.IsNullOrWhiteSpace(content.path))
                {
                    name = System.IO.Path.GetFileName(content.path);
                }
                description += name;
            }

            switch (observerAbort)
            {
                case ObserverAbort.None:
                    break;
                case ObserverAbort.Self:
                    description += $"\n( aborts self )";
                    break;
                case ObserverAbort.LowPriority:
                    description += $"\n( aborts lower priority )";
                    break;
                case ObserverAbort.Both:
                    description += $"\n( aborts both )";
                    break;
            }

            return description;
        }

        /// <summary>
        /// Dynamically hides the values of Observer Abort.
        /// </summary>
        /// <returns>Values to hide.</returns>
        protected virtual IEnumerable<ObserverAbort> GetHiddenObserverAborts()
        {
            // Temporary solution, to be fixed during a major update.
            // Move basic composites to the core assembly and perform type-based comparison.
            Node parent = GetParent();
            if (parent != null && parent.GetType().Name == "SequencerNode")
            {
                yield return ObserverAbort.LowPriority;
                yield return ObserverAbort.Both;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Called when the value in the inspector changes.
        /// </summary>
        protected override void OnInspectorChanged()
        {
            foreach (ObserverAbort hiddenObserverAbort in GetHiddenObserverAborts())
            {
                if ((observerAbort & hiddenObserverAbort) == hiddenObserverAbort)
                {
                    observerAbort &= ~hiddenObserverAbort;
                    break;
                }
            }

            base.OnInspectorChanged();

            if (Application.isPlaying)
            {
                OnValueChangeAction();
            }
        }
#endif

        #region [Events]
        /// <summary>
        /// An event that should be called when the observed value changes.
        /// </summary>
        public abstract event Action OnValueChange;

        /// <summary>
        /// An event that notifies the observer of a change in value.
        /// </summary>
        public event Action<bool> OnNotifyObserver;
        #endregion

        #region [Getter / Setter]
        /// <summary>
        /// Returns a notify observer.
        /// </summary>
        /// <returns>NotifyObserver</returns>
        public NotifyObserver GetNotifyObserver()
        {
            return notifyObserver;
        }

        /// <summary>
        /// Sets the notify observer.
        /// </summary>
        /// <param name="notifyObserver">NotifyObserver</param>
        public void SetNotifyObserver(NotifyObserver notifyObserver)
        {
            this.notifyObserver = notifyObserver;
        }

        /// <summary>
        /// Returns an observer abort.
        /// </summary>
        /// <returns>ObserverAbort</returns>
        public ObserverAbort GetObserverAbort()
        {
            return observerAbort;
        }

        /// <summary>
        /// Sets the observer abort.
        /// </summary>
        /// <param name="observerAbort">ObserverAbort</param>
        public void SetObserverAbort(ObserverAbort observerAbort)
        {
            this.observerAbort = observerAbort;
        }
        #endregion
    }
}