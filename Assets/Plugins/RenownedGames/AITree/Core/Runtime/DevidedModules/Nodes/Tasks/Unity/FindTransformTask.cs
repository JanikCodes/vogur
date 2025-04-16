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
using UnityEngine;

namespace RenownedGames.AITree.Nodes
{
    [Obsolete("Use Find task instead")]
    [NodeContent("Find Transform", "Tasks/Unity/Find Transform", IconPath = "Images/Icons/Node/FindTransformIcon.png", Hide = true)]
    public class FindTransformTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [KeyTypes(typeof(Transform), typeof(Vector3))]
        [NonLocal]
        private Key key;

        [Title("Node")]
        [SerializeField]
        private string objectName;

        protected override State OnUpdate()
        {
            if (key == null)
            {
                return State.Failure;
            }

            GameObject go = GameObject.Find(objectName);
            if (go != null)
            {
                Transform transform = go.transform;

                if (key is TransformKey transformKey)
                {
                    transformKey.SetValue(transform);
                }
                else if (key is Vector3Key vector3Key)
                {
                    vector3Key.SetValue(transform.position);
                }

                return State.Success;
            }

            return State.Failure;
        }
    }
}