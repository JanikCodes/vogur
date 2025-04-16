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
    [NodeContent("Detection Signal", "Tasks/Perception System/Detection Signal", IconPath = "Images/Icons/Node/AlertIcon.png")]
    [RequireComponent(typeof(AIDetectionSignal))]
    public class DetectionSignalTask : TaskNode
    {
        [SerializeField]
        [Title("Node")]
        private SignalType signalType;

        [SerializeField]
        [ShowIf("signalType", SignalType.Alert)]
        [MinValue(0f)]
        private float detectionTime = 1f;

        [SerializeField]
        [ShowIf("signalType", SignalType.Neutral)]
        [MinValue(0f)]
        private float lossTime = 1f;

        [SerializeField]
        [ShowIf("signalType", SignalType.Alert)]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key lookTarget;

        // Stored required components.
        private AIDetectionSignal detectionSignal;
        private Transform ownerTransform;

        // Stored required properties.
        private float startTime;
        private float duration;
        private float delta;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            ownerTransform = GetOwner().transform;
            detectionSignal = ownerTransform.GetComponent<AIDetectionSignal>();
        }

        protected override void OnEntry()
        {
            base.OnEntry();
            startTime = Time.time;
            duration = signalType == SignalType.Alert ? detectionTime : lossTime;
            delta = (signalType == SignalType.Alert ? detectionSignal.GetValue() : (1 - detectionSignal.GetValue())) * duration;
        }

        protected override State OnUpdate()
        {
            if (detectionSignal == null)
            {
                return State.Failure;
            }

            float time = Time.time + delta - startTime;

            if (signalType == SignalType.Alert)
            {
                if (duration > 0)
                {
                    float percent = time / duration;
                    if (percent < 1)
                    {
                        detectionSignal.SetValue(percent, SignalType.Alert);
                        LootTarget();
                        return State.Running;
                    }
                }

                LootTarget();
                detectionSignal.SetValue(1, SignalType.Alert);
            }
            else
            {
                if (duration > 0)
                {
                    float percent = 1 - (time / duration);
                    if (percent > 0)
                    {
                        detectionSignal.SetValue(percent, SignalType.Neutral);
                        return State.Running;
                    }
                }

                detectionSignal.SetValue(0, SignalType.Neutral);
            }

            return State.Success;
        }

        private void LootTarget()
        {
            if (lookTarget != null)
            {
                Vector3? point = null;
                if (lookTarget.TryCastValueTo<Transform>(out Transform transform))
                {
                    point = transform.position;
                }
                else if (lookTarget.TryCastValueTo<Vector3>(out Vector3 position))
                {
                    point = position;
                }

                if (point.HasValue)
                {
                    ownerTransform.LookAt(new Vector3(point.Value.x, ownerTransform.position.y, point.Value.z), Vector3.up);
                }
            }
        }

        public override string GetDescription()
        {
            string description = $"Detection Signal: {signalType}";

            if (signalType == SignalType.Alert)
            {
                description += $"\nDuration: {detectionTime}";
            }
            else
            {
                description += $"\nDuration: {lossTime}";
            }

            return description;
        }
    }
}