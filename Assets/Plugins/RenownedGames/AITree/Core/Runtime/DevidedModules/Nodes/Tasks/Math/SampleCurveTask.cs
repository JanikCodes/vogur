/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Sample Curve", "Tasks/Math/Sample Curve", IconPath = "Images/Icons/Node/MathIcon.png")]
    public class SampleCurveTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        private FloatKey storeValue;

        [Title("Node")]
        [SerializeField]
        private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField]
        private FloatKey sampleAt;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (sampleAt == null || storeValue == null)
            {
                return State.Failure;
            }

            float value = curve.Evaluate(sampleAt.GetValue());
            storeValue.SetValue(value);

            return State.Success;
        }
    }
}