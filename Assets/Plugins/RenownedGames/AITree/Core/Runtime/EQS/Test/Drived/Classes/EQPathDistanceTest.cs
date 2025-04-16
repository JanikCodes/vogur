/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;
using UnityEngine.AI;

namespace RenownedGames.AITree.EQS
{
    [SearchContent("Path Dist", Image = "Images/Icons/EQS/Tests/PathDistTestIcon.png")]
    public class EQPathDistanceTest : EQTest
    {
        [Header("Path Distance")]
        [SerializeField]
        private TargetType distanceTo;

        [SerializeField]
        [ShowIf("distanceTo", TargetType.Key)]
        private string keyName;

        [SerializeField]
        private bool allowPartialPath;

        [SerializeField]
        [MinValue(1)]
        private int maxQuery = 128;

        // Stored required properties.
        private Vector3[] points;

        protected override float CalculateWeight(EQItem item)
        {
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(GetPositionByTarget(TargetType.Item), GetPositionByTarget(distanceTo, keyName), NavMesh.AllAreas, path))
            {
                if (path.status == NavMeshPathStatus.PathInvalid)
                {
                    return 0;
                }

                if (path.status == NavMeshPathStatus.PathPartial && allowPartialPath)
                {
                    return GetPathDistance(path);
                }

                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    return GetPathDistance(path);
                }
            }
            return 0;
        }

        private float GetPathDistance(NavMeshPath path)
        {
            if(points == null || points.Length != maxQuery)
            {
                points = new Vector3[maxQuery];
            }

            float distance = 0;
            int count = path.GetCornersNonAlloc(points);
            for (int i = 1; i < count; i++)
            {
                distance += Vector3.Distance(points[i-1], points[i]);
            }
            return distance;
        }
    }
}