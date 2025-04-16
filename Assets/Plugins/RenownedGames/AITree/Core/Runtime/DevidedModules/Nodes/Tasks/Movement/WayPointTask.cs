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

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Way Point", "Tasks/Movement/Way Point", IconPath = "Images/Icons/Node/MoveObjectIcon.png")]
    public class WayPointTask : TaskNode
    {
        private enum SelectionType
        {
            Sequential,
            PingPong,
            Random
        }

        [Title("Blackboard")]
        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        private Key key;

        [Title("Node")]
        [SerializeField]
        private string wayName;

        [SerializeField]
        private SelectionType selectionType;

        [SerializeField]
        private bool startNearestPoint = true;

        [SerializeField]
        private IntKey lastIndex;

        // Stored required components.
        private AIWay way;
        private int currentIndex;

        protected override void OnStart()
        {
            base.OnStart();
            way = AIWay.FindWay(wayName);
            if (startNearestPoint && way != null)
            {
                currentIndex = way.GerNearestPointIndex(GetOwner().transform.position);
            }
        }

        protected override State OnUpdate()
        {
            if (way == null)
            {
                return State.Failure;
            }

            int index = GetIndexBySelectionType();
            AIWayPoint wayPoint = way.GetPointByIndex(index);

            if (key is TransformKey transformKey)
            {
                transformKey.SetValue(wayPoint.transform);
            }
            else if (key is Vector3Key vectorKey)
            {
                vectorKey.SetValue(wayPoint.transform.position);
            }

            if (lastIndex != null)
            {
                lastIndex.SetValue(index);
            }

            return State.Success;
        }

        private int GetIndexBySelectionType()
        {
            int length = way.GetPointCount();

            switch (selectionType)
            {
                case SelectionType.Random:
                    currentIndex = Random.Range(0, length);
                    return currentIndex;
                case SelectionType.Sequential:
                    return (currentIndex++) % length;
                case SelectionType.PingPong:
                    return (int)Mathf.PingPong(currentIndex++, length - 1);
            }

            return currentIndex;
        }
    }
}