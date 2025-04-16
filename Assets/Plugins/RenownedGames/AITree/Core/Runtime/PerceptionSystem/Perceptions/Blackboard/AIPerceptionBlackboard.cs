/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System.Collections.Generic;
using UnityEngine;

namespace RenownedGames.AITree.PerceptionSystem
{
    [HideMonoScript]
    [AddComponentMenu("Renowned Games/AI Tree/Perception System/AI Perception Blackboard")]
    [ObjectIcon("Images/Logotype/AITreeIcon.png")]
    public class AIPerceptionBlackboard : AIPerception, ISyncKeyReceiver
    {
        [SerializeField]
        [NonLocal]
        [HideSelfKey]
        [Order(-100)]
        private TransformKey key;

        /// <summary>
        /// Called when AI perception source has been changed.
        /// </summary>
        /// <param name="target">Changed AI perception source instance.</param>
        protected override void OnTargetChanged(AIPerceptionSource source)
        {
            base.OnTargetChanged(source);

            if (key != null)
            {
                if (source != null)
                {
                    key.SetValue(source.transform);
                }
                else
                {
                    key.SetValue(null);
                }
            }
        }

        #region [ISyncKeyReceiver Implementation]
        public void OnSynchronization(List<Key> keys)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                Key syncKey = keys[i];
                if(key != null && !syncKey.IsSync() && syncKey.name == key.name)            
                {
                    key = (TransformKey)syncKey;
                    return;
                }
            }
        }
        #endregion
    }
}