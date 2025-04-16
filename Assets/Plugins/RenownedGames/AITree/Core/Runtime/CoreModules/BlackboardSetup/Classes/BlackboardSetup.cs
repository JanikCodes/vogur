/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RenownedGames.AITree
{
    [Experimental]
    [HideMonoScript]
    [AddComponentMenu("Renowned Games/AI Tree/Blackboard/Blackboard Setup")]
    [DisallowMultipleComponent]
    public sealed class BlackboardSetup : MonoBehaviour, ISyncKeyReceiver
    {
        [SerializeField]
        [Array]
        private List<KeySetup> keySetups;

        /// <summary>
        /// Called when an enabled script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (!TryGetComponent<BehaviourRunner>(out BehaviourRunner runner))
            {
                for (int i = 0; i < keySetups.Count; i++)
                {
                    KeySetup keySetup = keySetups[i];
                    keySetup.keyReceiver.Apply(keySetup.key);
                }
            }
        }

        #region [ISyncKeyReceiver Implentation]
        public void OnSynchronization(List<Key> keys)
        {
            for (int i = 0; i < keySetups.Count; i++)
            {
                KeySetup keySetup = keySetups[i];
                for (int j = 0; j < keys.Count; j++)
                {
                    Key cloneKey = keys[j];
                    if (keySetup.key.name == cloneKey.name)
                    {
                        keySetup.keyReceiver.Apply(cloneKey);
                        break;
                    }
                }
            }
        }
        #endregion
    }
}