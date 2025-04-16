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
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [HideMonoScript]
    [NodeContent("Cone Check", "Cone Check", IconPath = "Images/Icons/Node/ConeCheckIcon.png")]
    public class ConeCheckDecorator : ObserverDecorator
    {
        [Title("Condition")]
        [SerializeField]
        private bool inverseCondition;

        [Title("Blackboard")]
        [SerializeField]
        [NonLocal]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key coneOrigin;

        [SerializeField]
        [NonLocal]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key coneDirection;

        [SerializeField]
        [NonLocal]
        private TransformKey observed;

        [Title("Decorator")]
        [SerializeField]
        private float coneHalfAngle = 45f;

        /// <summary>
        /// Called every tick regardless of the node execution.
        /// </summary>
        protected override void OnFlowUpdate()
        {
            base.OnFlowUpdate();
            OnValueChange?.Invoke();
        }

        #region [ObserverDecorator Implementation]
        /// <summary>
        /// An event that should be called when the observed value changes.
        /// </summary>
        public override event Action OnValueChange;

        /// <summary>
        /// Calculates the observed value of the result.
        /// </summary>
        public override bool CalculateResult()
        {
            if (coneOrigin == null || coneDirection == null || observed == null) return false;

            Vector3 origin = Vector3.zero;
            {
                if (coneOrigin is TransformKey transformKey)
                {
                    if (transformKey != null)
                    {
                        origin = transformKey.GetValue().position;
                    }
                }
                else if (coneOrigin is Vector3Key vector3Key)
                {
                    origin = vector3Key.GetValue();
                }
            }

            Vector3 direction = Vector3.zero;
            {
                if (coneDirection is TransformKey transformKey)
                {
                    if (transformKey != null)
                    {
                        direction = transformKey.GetValue().forward;
                    }
                }
                else if (coneDirection is Vector3Key vector3Key)
                {
                    direction = vector3Key.GetValue();
                }
            }

            Vector3 observedDirection = observed.GetValue().position - origin;

            float angle = Vector3.Angle(direction, observedDirection);
            bool result = angle <= coneHalfAngle;
            return inverseCondition ? !result : result;
        }
        #endregion

        #region [IEntityDescription Implementation]
        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = base.GetDescription();

            if (!string.IsNullOrEmpty(description))
            {
                description += "\n";
            }

            description += "Cone Check: ";
            description += $"is {(observed != null ? observed.name : "None")} in {coneHalfAngle * 2f} degree ";
            description += $"{(coneOrigin != null ? coneOrigin.name : "None")} - {(coneDirection != null ? coneDirection.name : "None")} cone";

            return description;
        }
        #endregion
    }
}