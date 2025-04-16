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
    [NodeContent("Compare Blackboard Keys", "Compare Blackboard Keys", IconPath = "Images/Icons/Node/BlackboardConditionIcon.png")]
    public class CompareBlackboardKeysDecorator : ObserverDecorator
    {
        public enum Operator
        {
            IsEqualTo,
            IsNotEqualTo
        }

        [Title("Blackboard")]
        [SerializeField]
        [Label("Operator")]
        private Operator compareOperator;

        [SerializeField]
        [NonLocal]
        private Key keyA;

        [SerializeField]
        [NonLocal]
        private Key keyB;

        /// <summary>
        /// Called on behaviour tree is awake.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (keyA != null && keyB != null)
            {
                keyA.ValueChanged += OnValueChange;
                keyB.ValueChanged += OnValueChange;
            }
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
            if (keyA == null || keyB == null) return false;

            if (keyA.GetValueType() == keyB.GetValueType())
            {
                bool equal = keyA.GetValueObject().Equals(keyB.GetValueObject());
                return compareOperator == Operator.IsEqualTo ? equal : !equal;
            }

            return false;
        }
        #endregion

        #region [IEntityDescription Implementation]
        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = base.GetDescription();

            if (keyA != null && keyB != null)
            {
                if (!string.IsNullOrEmpty(description)) description += "\n";
                description += "Compare Blackboard Keys:\n";
                description += $"{keyA.name} and {keyB.name}\n";
                description += $"contain {(compareOperator == Operator.IsEqualTo ? "EQUAL" : "NOT EQUAL")} values";
            }

            return description;
        }
        #endregion

        #region [Getter / Setter]
        public Operator GetOperator()
        {
            return compareOperator;
        }

        public void SetOperator(Operator compareOperator)
        {
            this.compareOperator = compareOperator;
        }

        public Key GetKeyA()
        {
            return keyA;
        }

        public Key GetKeyB()
        {
            return keyB;
        }
        #endregion
    }
}