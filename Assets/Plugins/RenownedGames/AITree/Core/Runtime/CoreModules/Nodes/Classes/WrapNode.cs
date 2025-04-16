/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022 - 2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RenownedGames.AITree
{
    public abstract class WrapNode : Node
    {
        [SerializeReference]
        [HideInInspector]
        private List<DecoratorNode> decorators = new List<DecoratorNode>();

        [SerializeReference]
        [HideInInspector]
        private List<ServiceNode> services = new List<ServiceNode>();

        // Stored required properties.
        private Dictionary<ObserverDecorator, bool> observerConditions;

        /// <summary>
        /// Called on behaviour tree is awake.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            observerConditions = new Dictionary<ObserverDecorator, bool>();
            for (int i = 0; i < decorators.Count; i++)
            {
                if (decorators[i] is ObserverDecorator observerDecorator)
                {
                    if ((observerDecorator.GetObserverAbort() & ObserverAbort.LowPriority) != 0)
                    {
                        observerConditions.Add(observerDecorator, false);
                    }

                    observerDecorator.OnNotifyObserver += (result) => OnNotified(observerDecorator, result);
                }
            }
        }

        /// <summary>
        /// Sets a reference to the parent node.
        /// </summary>
        internal override void SetupParentReference()
        {
            for (int i = 0; i < decorators.Count; i++)
            {
                DecoratorNode decorator = decorators[i];
                if(decorator != null)
                {
                    decorator.SetContainerNode(this);
                }
            }

            for (int i = 0; i < services.Count; i++)
            {
                ServiceNode service = services[i];
                if(service != null)
                {
                    service.SetContainerNode(this);
                }
            }
        }

        /// <summary>
        /// Executes node logic.
        /// </summary>
        /// <returns>State</returns>
        public override State Update()
        {
            if (decorators.Count > 0 && !decorators[0].IsProcessing())
            {
                for (int i = 0; i < decorators.Count - 1; i++)
                {
                    decorators[i].SetChild(decorators[i + 1]);
                }

                decorators[decorators.Count - 1].SetChild(this);
                DecoratorNode headDecorator = decorators[0];
                headDecorator.IsProcessing(true);
                State decoratorState = headDecorator.Update();
                headDecorator.IsProcessing(false);

                SetState(decoratorState);

                return decoratorState;
            }
            else if (services.Count > 0 && !services[0].IsProcessing())
            {
                for (int i = 0; i < services.Count - 1; i++)
                {
                    services[i].SetChild(services[i + 1]);
                }

                services[services.Count - 1].SetChild(this);
                ServiceNode headService = services[0];
                headService.IsProcessing(true);
                State serviceState = headService.Update();
                headService.IsProcessing(false);

                SetState(serviceState);

                return serviceState;
            }
            else
            {
                base.Update();
            }

            return GetState();
        }

        /// <summary>
        /// Traverse through all next linked nodes with visitor callback.
        /// </summary>
        public override void Traverse(Action<Node> visiter)
        {
            for (int i = 0; i < decorators.Count; i++)
            {
                visiter(decorators[i]);
            }

            base.Traverse(visiter);

            for (int i = 0; i < services.Count; i++)
            {
                visiter(services[i]);
            }
        }

        /// <summary>
        /// Clone node with all its stuff.
        /// </summary>
        /// <returns>New cloned copy of node.</returns>
        public override Node Clone()
        {
            decorators.RemoveAll(t => t == null);
            decorators.TrimExcess();

            services.RemoveAll(t => t == null);
            services.TrimExcess();

#if UNITY_EDITOR
            if(!UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
            }
#endif

            WrapNode wrap = (WrapNode)base.Clone();
            
            if(decorators != null)
            {
                for (int i = 0; i < decorators.Count; i++)
                {
                    wrap.decorators[i] = (DecoratorNode)decorators[i].Clone();
                }
            }

            if (services != null)
            {
                for (int i = 0; i < services.Count; i++)
                {
                    wrap.services[i] = (ServiceNode)services[i].Clone();
                }
            }

            return wrap;
        }

        /// <summary>
        /// Called when the value of the observed result changes
        /// </summary>
        private void OnNotified(ObserverDecorator observer, bool result)
        {
            int currentOrder = GetBehaviourTree().GetCurrentNode()?.GetOrder() ?? -1;
            if (GetOrder() <= currentOrder)
            {
                ObserverAbort observerAbort = observer.GetObserverAbort();

                if (observerAbort == ObserverAbort.LowPriority || observerAbort == ObserverAbort.Both)
                {
                    if (observerConditions.ContainsKey(observer))
                    {
                        observerConditions[observer] = result;
                    }
                }

                Node current = GetBehaviourTree().GetCurrentNode();
                bool isSubNode = current != null && BehaviourTree.IsSubNode(this, current);
                if (!isSubNode)
                {
                    if (result)
                    {
                        bool valid = observerConditions.Count != 0;
                        foreach (bool value in observerConditions.Values)
                        {
                            if (!value)
                            {
                                valid = false;
                                break;
                            }
                        }

                        if (valid)
                        {
                            GetBehaviourTree().JumpToNode(this);
                        }
                    }
                }
                else
                {
                    if (observerAbort == ObserverAbort.Self || observerAbort == ObserverAbort.Both)
                    {
                        if (!result)
                        {
                            GetBehaviourTree().JumpToNode(this);
                        }
                    }
                }
            }
        }

        internal override void OnClone(CloneData cloneData)
        {
            base.OnClone(cloneData);

            for (int i = 0; i < decorators.Count; i++)
            {
                decorators[i] = cloneData.cloneNodeMap[decorators[i]] as DecoratorNode;
            }

            for (int i = 0; i < services.Count; i++)
            {
                services[i] = cloneData.cloneNodeMap[services[i]] as ServiceNode;
            }
        }

        #region [Getter / Setter]
        /// <summary>
        /// Returns a decorators.
        /// </summary>
        /// <returns>List of decorator nodes.</returns>
        public List<DecoratorNode> GetDecorators()
        {
            return decorators;
        }

        /// <summary>
        /// Sets a decorators.
        /// </summary>
        /// <param name="decorators">List of decorator nodes.</param>
        public void SetDecorators(List<DecoratorNode> decorators)
        {
            this.decorators = decorators;
        }

        /// <summary>
        /// Adds a decorator.
        /// </summary>
        /// <param name="decorator">Decorator node.</param>
        public void AddDecorator(DecoratorNode decorator)
        {
            decorators.Add(decorator);
        }

        /// <summary>
        /// Adds a decorator.
        /// </summary>
        /// <param name="decorator">Decorator node.</param>
        /// <param name="index">Insertion index.</param>
        public void AddDecorator(DecoratorNode decorator, int index)
        {
            decorators.Insert(index, decorator);
        }

        /// <summary>
        /// Adds a list of decorators.
        /// </summary>
        /// <param name="decorators">List of decorators.</param>
        public void AddDecorators(List<DecoratorNode> decorators)
        {
            this.decorators.AddRange(decorators);
        }

        /// <summary>
        /// Removes the decorator.
        /// </summary>
        /// <param name="decorator">Decorator node.</param>
        public void RemoveDecorator(DecoratorNode decorator)
        {
            decorators.Remove(decorator);
        }

        /// <summary>
        /// Removes all decorators.
        /// </summary>
        public void RemoveAllDecorators()
        {
            decorators.Clear();
        }

        /// <summary>
        /// Returns a services.
        /// </summary>
        /// <returns>List of service nodes.</returns>
        public List<ServiceNode> GetServices()
        {
            return services;
        }

        /// <summary>
        /// Sets a services.
        /// </summary>
        /// <param name="services">List of service nodes.</param>
        public void SetServices(List<ServiceNode> services)
        {
            this.services = services;
        }

        /// <summary>
        /// Adds a service.
        /// </summary>
        /// <param name="service">Service node.</param>
        public void AddService(ServiceNode service)
        {
            services.Add(service);
        }

        /// <summary>
        /// Adds a service.
        /// </summary>
        /// <param name="service">Service node.</param>
        /// <param name="index">Insertion index.</param>
        public void AddService(ServiceNode service, int index)
        {
            services.Insert(index, service);
        }

        /// <summary>
        /// Adds a list of services.
        /// </summary>
        /// <param name="services">List of services.</param>
        public void AddServices(List<ServiceNode> services)
        {
            this.services.AddRange(services);
        }

        /// <summary>
        /// Removes the service.
        /// </summary>
        /// <param name="service">Service node.</param>
        public void RemoveService(ServiceNode service)
        {
            services.Remove(service);
        }

        /// <summary>
        /// Removes all services.
        /// </summary>
        public void RemoveAllServices()
        {
            services.Clear();
        }

        /// <summary>
        /// Enumerate all auxiliary nodes.
        /// </summary>
        public IEnumerable<AuxiliaryNode> AllAuxiliaryNodes()
        {
            for (int i = 0; i < decorators.Count; i++)
            {
                DecoratorNode decorator = decorators[i];
                yield return decorator;
            }

            for (int i = 0; i < services.Count; i++)
            {
                ServiceNode service = services[i];
                yield return service;
            }
        }

        protected internal override void SetParent(Node parent)
        {
            base.SetParent(parent);

            for (int i = 0; i < decorators.Count; i++)
            {
                DecoratorNode decorator = decorators[i];
                if(decorator != null)
                {
                    decorator.SetParent(GetParent());
                }
            }

            for (int i = 0; i < services.Count; i++)
            {
                ServiceNode service = services[i];
                if(service != null)
                {
                    service.SetParent(GetParent());
                }
            }
        }
        #endregion
    }
}