/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEngine;
using RenownedGames.Apex;
using System;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RenownedGames.AITree.EQS
{
    [HideMonoScript]
    [CreateAssetMenu(menuName = "Renowned Games/AI Tree/Environment Query")]
    public class EnvironmentQuery : ScriptableObject
    {
        [SerializeReference]
        [DropdownReference]
        private EQGenerator generator;

        [SerializeReference]
        [DropdownReference]
        private EQTraceMode traceMode;

        [SerializeReference]
        [ReferenceArray]
        private List<EQTest> tests;

        // Stored required components.
        private Transform querier;

        // Stored required properties
        private List<EQItem> items;
        private Blackboard blackboard;

        public EQItem GetRandomlyItemBySampling(float sampling)
        {
            if (!PrepareItems())
            {
                return null;
            }

            List<EQItem> suitableItems = new List<EQItem>(items);
            for (int i = 0; i < suitableItems.Count; i++)
            {
                EQItem item = suitableItems[i];
                if (!item.IsValid())
                {
                    suitableItems.RemoveAt(i--);
                }
                else
                {
                    float? score = item.GetScore();
                    if (!score.HasValue || ((1 - score.Value) > sampling))
                    {
                        suitableItems.RemoveAt(i--);
                    }
                }
            }

            if (suitableItems.Count == 0)
            {
                return null;
            }

            onItemsUpdate?.Invoke(items);

            return suitableItems[Random.Range(0, suitableItems.Count)];
        }

        private bool PrepareItems()
        {
            GenerateItems(ref items);

            if(items.Count == 0)
            {
                return false;
            }

            ApplyTests(items);

            float maxScore = 0;
            for (int i = 0; i < items.Count; i++)
            {
                EQItem item = items[i];
                if (item.IsValid())
                {
                    float? score = item.GetScore();
                    if (score.HasValue && score.Value > maxScore && !Mathf.Approximately(score.Value, maxScore))
                    {
                        maxScore = score.Value;
                    }
                }
            }

            if (maxScore != 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    EQItem item = items[i];
                    if (!item.IsValid()) continue;

                    float? score = item.GetScore();
                    if (!score.HasValue) continue;

                    items[i].SetScore(score.Value / maxScore);
                }
            }

            return true;
        }

        private void GenerateItems(ref List<EQItem> items)
        {
            if (generator == null)
            {
                return;
            }

            if(items == null)
            {
                items = new List<EQItem>();
            }
            else
            {
                items.Clear();
            }

            generator.SetQuerier(querier);

            foreach (Vector3 point in generator.GeneratePoints())
            {
                if (traceMode == null)
                {
                    items.Add(new EQItem(point));
                }
                else
                {
                    foreach (Vector3 tracedPoint in traceMode.TryTracePosition(point))
                    {
                        items.Add(new EQItem(tracedPoint));
                    }
                }
            }
        }

        private void ApplyTests(List<EQItem> items)
        {
            for (int i = 0; i < tests.Count; i++)
            {
                EQTest test = tests[i];
                test.SetQuerier(querier);
                test.SetBlackboard(blackboard);
                test.Run(items);
            }
        }

#if UNITY_EDITOR
        #region [Debug]
        private GUIStyle textStyle;

        public void Visualize(bool visualzieBounds, bool showScore)
        {
            if (generator == null || !PrepareItems()) 
            {
                return;
            }

            if (visualzieBounds)
            {
                Handles.color = Color.black;
                generator.VisualizeBounds();
            }

            Handles.matrix = Matrix4x4.identity;

            for (int i = 0; i < items.Count; i++)
            {
                EQItem item = items[i];
                Color color = new Color(0, 0, 1, .5f);
                if (item.IsValid())
                {
                    color = Color.Lerp(new Color(1, 0, 0, .75f), new Color(0, 1, 0, .75f), item.GetScore());
                }

                Handles.color = color;
                Handles.SphereHandleCap(0, item.GetPosition(), Quaternion.identity, 0.25f, EventType.Repaint);

                if (showScore)
                {
                    if (textStyle == null)
                    {
                        textStyle = new GUIStyle();
                    }

                    textStyle.alignment = TextAnchor.MiddleCenter;
                    textStyle.contentOffset = new Vector2(0, 0);
                    textStyle.normal.textColor = Color.black;
                    textStyle.fontStyle = FontStyle.BoldAndItalic;

                    if (item.IsValid())
                    {
                        Handles.Label(item.GetPosition(), Mathf.RoundToInt(item.GetScore() * 100f).ToString(), textStyle);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(item.message))
                        {
                            Handles.Label(item.GetPosition(), item.message, textStyle);
                        }
                    }
                }
            }
        }
        #endregion
#endif

        #region [Events]
        public event Action<List<EQItem>> onItemsUpdate;
        #endregion

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
    }
}