/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace RenownedGames.AITree.PerceptionSystem
{
    public abstract class AIDetectionSignal : MonoBehaviour
    {
        // Stored required property
        private float value;
        private SignalType lastSignalType;
        private bool detected;

        /// <summary>
        /// Called when detection value has been changed.
        /// </summary>
        /// <param name="value">Value between 0 and 1.</param>
        protected abstract void OnDetection(float value);

        #region [Events]
        public event Action<float, SignalType> OnDetecting;
        public event Action OnDetectCompletely;
        public event Action OnLostCompletely;
        public event Action OnLostSightOf;
        #endregion

        #region [Getter / Setter]
        public void SetValue(float value, SignalType signalType)
        {
            this.value = Mathf.Clamp01(value);

            if (this.value == 0 && detected)
            {
                detected = false;
                OnLostCompletely?.Invoke();
            }

            if (this.value == 1 && !detected)
            {
                detected = true;
                OnDetectCompletely?.Invoke();
            }

            if (lastSignalType != signalType && signalType == SignalType.Neutral && !detected)
            {
                OnLostSightOf?.Invoke();
            }

            lastSignalType = signalType;
            OnDetection(this.value);
            OnDetecting?.Invoke(this.value, signalType);
        }

        public float GetValue()
        {
            return value;
        }
        #endregion
    }
}