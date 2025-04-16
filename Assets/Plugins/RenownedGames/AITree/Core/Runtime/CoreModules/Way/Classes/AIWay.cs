/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using System;
using System.Collections.Generic;
using UnityEngine;
using RenownedGames.Apex;
using UnityEngine.SceneManagement;

namespace RenownedGames.AITree
{
    [HideMonoScript]
    [AddComponentMenu("Renowned Games/AI Tree/Ways/AI Way")]
    [DisallowMultipleComponent]
    public sealed class AIWay : MonoBehaviour
    {
        private static List<AIWay> Ways;

        static AIWay()
        {
            Ways = new List<AIWay>();

            SceneManager.sceneUnloaded += (scene) =>
            {
                Ways.Clear();
                Ways.Capacity = 0;
            };
        }

        [SerializeField]
        [Array]
        private AIWayPoint[] points;

        private void Awake()
        {
            Ways.Add(this);
            points = GetComponentsInChildren<AIWayPoint>();
        }

        public int GerNearestPointIndex(Vector3 position)
        {
            int nearestPointIndex = 0;
            float nearestPointSqrDistance = (position - points[0].GetPosition()).sqrMagnitude;
            for (int i = 1; i < points.Length; i++)
            {
                AIWayPoint wayPoint = points[i];

                float pointSqrDistance = (position - points[i].GetPosition()).sqrMagnitude;
                if (pointSqrDistance < nearestPointSqrDistance)
                {
                    nearestPointSqrDistance = pointSqrDistance;
                    nearestPointIndex = i;
                }
            }
            return nearestPointIndex;
        }

        #region [Static Methods]
        public static AIWay FindWay(string name)
        {
            for (int i = 0; i < Ways.Count; i++)
            {
                AIWay way = Ways[i];
                if (way.name == name)
                {
                    return way;
                }
            }

            return null;
        }
        #endregion

        #region [Getter / Setter]
        public int GetPointCount()
        {
            return points.Length;
        }

        public AIWayPoint GetPointByIndex(int index)
        {
            if (index < 0 || index >= points.Length)
            {
                throw new IndexOutOfRangeException();
            }

            return points[index];
        }
        #endregion
    }
}