/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree
{
    [HideMonoScript]
    [AddComponentMenu("Renowned Games/AI Tree/Blackboard/Blackboard Instance")]
    [RequireComponent(typeof(BehaviourRunner))]
    [System.Obsolete("Use BehaviourRunner instead. BlackboardInstance will be removed in next updates.")]
    public sealed class BlackboardInstance : MonoBehaviour, IBlackboardInstance, IBlackboardShared
    {
        private Blackboard blackboard;
        private Blackboard sharedBlackboard;

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (TryGetComponent<BehaviourRunner>(out BehaviourRunner runner))
            {
                sharedBlackboard = runner.GetSharedBlackboard();
                blackboard = runner.GetBlackboard();
            }

            //if(TryGetComponent<BehaviourRunner>(out BehaviourRunner behaviourRunner))
            //{
            //    sharedBlackboard = behaviourRunner.GetBehaviourTree().GetBlackboard();
            //    if (sharedBlackboard != null)
            //    {
            //        blackboard = sharedBlackboard.Clone();
            //        NotifySyncReceivers(blackboard);
            //    }
            //}

            //Debug.Assert(behaviourRunner != null, "BlackboardInstance can be used only with BehaviourRunner!");

            //if (blackboard != null)
            //{
            //    foreach (Key key in blackboard.Keys)
            //    {
            //        if (key is SelfKey selfKey)
            //        {
            //            selfKey.SetValue(transform);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Notify all self and children components which implement ISyncKeyReceiver interface.
        /// </summary>
        /// <param name="blackboard">Actual cloned blackboard instance.</param>
        //private void NotifySyncReceivers(Blackboard blackboard)
        //{
        //    ISyncKeyReceiver[] receivers = GetComponentsInChildren<ISyncKeyReceiver>();
        //    for (int i = 0; i < receivers.Length; i++)
        //    {
        //        receivers[i].OnSynchronization(blackboard.GetAllKeys());
        //    }
        //}

        #region [IBlackboardInstance Implementation]
        public Blackboard GetBlackboard()
        {
            return blackboard;
        }
        #endregion

        #region [IBlackboardShared Implementation]
        public Blackboard GetSharedBlackboard()
        {
            return sharedBlackboard;
        }
        #endregion
    }
}