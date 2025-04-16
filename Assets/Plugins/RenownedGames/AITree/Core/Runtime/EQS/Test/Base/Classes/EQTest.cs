/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RenownedGames.AITree.EQS
{
    [Serializable]
    public abstract class EQTest
    {
        [Title("Test")]
        [SerializeField]
        private bool mute;

        [SerializeField]
        private TestPurpose testPurpose = TestPurpose.ScoreOnly;

        [Title("Score")]
        [SerializeField]
        private ScoreSamplingOperation scoreSamplingOperation = ScoreSamplingOperation.Multiply;

        [SerializeField]
        private AnimationCurve scoringEquation = AnimationCurve.Linear(0, 0, 1, 1);

        [Title("Filter")]
        [SerializeField]
        private FilerType filerType = FilerType.Minimum;

        [SerializeField]
        [ShowIf("FilterExpression", FilerType.Minimum)]
        private float minValue;

        [SerializeField]
        [ShowIf("FilterExpression", FilerType.Maximum)]
        private float maxValue;

        // Stored required components.
        private Transform querier;

        // Stored required properties.
        private EQItem temporaryItem;
        private Blackboard blackboard;

        public void Run(List<EQItem> items)
        {
            if (mute)
            {
                return;
            }

            float[] weights = new float[items.Count];

            for (int i = 0; i < items.Count; i++)
            {
                EQItem item = items[i];
                temporaryItem = item;

                if (!item.IsValid()) continue;

                float weight = CalculateWeight(item);

                if ((testPurpose & TestPurpose.ScoreOnly) == TestPurpose.ScoreOnly)
                {
                    weights[i] = weight;
                }

                if ((testPurpose & TestPurpose.FilterOnly) == TestPurpose.FilterOnly)
                {
                    items[i].IsValid(FilterByWeight(weight), GetType().Name);
                }
            }

            if ((testPurpose & TestPurpose.ScoreOnly) == TestPurpose.ScoreOnly)
            {
                ComputeScores(items, weights);
            }
        }

        protected abstract float CalculateWeight(EQItem item);

        protected EQItem GetCurrentItem()
        {
            return temporaryItem;
        }

        protected virtual Vector3 GetPositionByTarget(TargetType targetType, string keyName = null)
        {
            switch (targetType)
            {
                case TargetType.Querier:
                    return querier.position;
                case TargetType.Item:
                    return GetCurrentItem().GetPosition();
                case TargetType.Key:
                    if (!string.IsNullOrEmpty(keyName))
                    {
                        if (blackboard != null)
                        {
                            foreach (Key key in blackboard.Keys)
                            {
                                if (key.name == keyName)
                                {
                                    if(key.TryGetPosition(Space.World, out Vector3 position))
                                    {
                                        return position;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    break;
            }

            return Vector3.zero;
        }

        protected virtual Vector3 GetForwardByKey(string keyName)
        {
            if (!string.IsNullOrEmpty(keyName))
            {
                if (blackboard != null)
                {
                    foreach (Key key in blackboard.Keys)
                    {
                        if (key.name == keyName)
                        {
                            if (key.TryCastValueTo<Transform>(out Transform transform))
                            {
                                if (transform != null)
                                {
                                    return transform.forward;
                                }
                            }
                            break;
                        }
                    }
                }
            }

            return Vector3.zero;
        }

        private bool FilterByWeight(float weight)
        {
            switch (filerType)
            {
                case FilerType.Minimum:
                    return minValue < weight;
                case FilerType.Maximum:
                    return weight <= maxValue;
                case FilerType.Range:
                    return minValue <= weight && weight <= maxValue;
            }
            return false;
        }

        private void ComputeScores(List<EQItem> items, float[] weights)
        {
            float[] rawScores = CalculateRawScores(weights);
            for (int i = 0; i < items.Count; i++)
            {
                EQItem item = items[i];
                float score = 0;
                switch (scoreSamplingOperation)
                {
                    case ScoreSamplingOperation.AverageScore:
                        score = (item.GetScore() + rawScores[i]) * .5f;
                        break;
                    case ScoreSamplingOperation.MinScore:
                        score = Mathf.Min(item.GetScore(), rawScores[i]);
                        break;
                    case ScoreSamplingOperation.MaxScore:
                        score = Mathf.Max(item.GetScore(), rawScores[i]);
                        break;
                    case ScoreSamplingOperation.Multiply:
                        score = item.GetScore() * rawScores[i];
                        break;
                }
                item.SetScore(score);
            }
        }

        private float[] CalculateRawScores(float[] weights)
        {
            float minWeight = float.MaxValue;
            float maxWeight = float.MinValue;

            for (int i = 0; i < weights.Length; i++)
            {
                float weight = weights[i];

                if (weight < minWeight)
                {
                    minWeight = weight;
                }

                if (weight > maxWeight)
                {
                    maxWeight = weight;
                }
            }

            if (Mathf.Approximately(minWeight, maxWeight))
            {
                return weights;
            }

            float[] rawScores = new float[weights.Length];
            
            if (minWeight < maxWeight)
            {
                for (int i = 0; i < weights.Length; i++)
                {
                    float score = Mathf.InverseLerp(minWeight, maxWeight, weights[i]);
                    rawScores[i] = Mathf.Clamp01(scoringEquation.Evaluate(score));
                }
            }

            return rawScores;
        }

        #region [Getter / Setter]
        public Transform GetQuerier()
        {
            return querier;
        }

        public void SetQuerier(Transform querier)
        {
            this.querier = querier;
        }

        public Blackboard GetBlackboard()
        {
            return blackboard;
        }

        public void SetBlackboard(Blackboard blackboard)
        {
            this.blackboard = blackboard;
        }
        #endregion

        #region [Show If Expressions]
        private bool FilterExpression(FilerType type)
        {
            return (filerType & type) == type;
        }
        #endregion
    }
}