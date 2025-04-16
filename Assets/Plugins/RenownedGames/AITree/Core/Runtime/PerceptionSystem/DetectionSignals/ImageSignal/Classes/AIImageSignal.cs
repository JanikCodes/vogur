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
using UnityEngine.UI;

namespace RenownedGames.AITree.PerceptionSystem
{
    [HideMonoScript]
    [AddComponentMenu("Renowned Games/AI Tree/Detection Signals/AI Image Signal")]
    [ObjectIcon("Images/Logotype/AITreeIcon.png")]
    public sealed class AIImageSignal : AIDetectionSignal
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private Color startColor = Color.yellow;

        [SerializeField]
        private Color endColor = Color.red;

        protected override void OnDetection(float value)
        {
            image.fillAmount = value;
            image.color = Color.Lerp(startColor, endColor, value);
        }
    }
}