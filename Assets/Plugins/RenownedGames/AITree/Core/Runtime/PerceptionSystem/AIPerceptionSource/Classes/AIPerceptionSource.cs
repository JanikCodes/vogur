/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.PerceptionSystem
{
    [HideMonoScript]
    [AddComponentMenu("Renowned Games/AI Tree/Perception System/AI Perception Source")]
    [ObjectIcon("Images/Logotype/AITreeIcon.png")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class AIPerceptionSource : MonoBehaviour 
    {
        [SerializeField]
        private bool observable = true;

        // Stored required properties.
        private new Collider collider;

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            collider = GetComponent<Collider>();
            AIPerceptionSourceUtility.Sources.Add(collider, this);
        }

        /// <summary>
        /// Called when the source become destroy.
        /// </summary>
        protected virtual void OnDestroy()
        {
            AIPerceptionSourceUtility.Sources.Remove(collider);
        }

        #region [Getter / Setter]
        public bool IsObservable()
        {
            return observable;
        }

        public void IsObservable(bool value)
        {
            observable = value;
        }
        #endregion
    }
}