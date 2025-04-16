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
    [HideMonoScript]
    [NodeContent("Is At Location", "Is At Location", IconPath = "Images/Icons/Node/IsAtLocationIcon.png")]
    public class IsAtLocationDecorator : ConditionDecorator
    {
        [Title("Blackboard")]
        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key key;

        [Title("Node")]
        [SerializeField]
        private FloatKey acceptableRadius;

        /// <summary>
        /// Calculates the result of the condition.
        /// </summary>
        protected override bool CalculateResult()
        {
            if (key == null || acceptableRadius == null) return false;

            Vector3 ownerPosition = GetOwner().transform.position;
            Vector3 targetPosition = Vector3.zero;
            if (key is TransformKey transformKey)
            {
                Transform transform = transformKey.GetValue();
                if (transform != null)
                {
                    targetPosition = transform.position;
                }
            }
            else if (key is Vector3Key vector3Key)
            {
                targetPosition = vector3Key.GetValue();
            }

            float radius = acceptableRadius.GetValue();
            float distance = Vector3.Distance(ownerPosition, targetPosition);
            return distance <= radius;
        }

        #region [IEntityDescription Implementation]
        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            return $"Is At Location: {(key != null ? key.name : "None")}";
        }
        #endregion
    }
}