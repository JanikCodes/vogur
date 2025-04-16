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
    [NodeContent("Find Closest", "Tasks/Game Object/Find Closest", IconPath = "Images/Icons/Node/GameObjectIcon.png")]
    public class GOFindClosestTask : TaskNode
    {
        [Title("Blackboard")]
        [SerializeField]
        [HideSelfKey]
        [NonLocal]
        private TransformKey storeObject;

        [SerializeField]
        [NonLocal]
        private FloatKey storeDistance;

        [Title("Node")]
        [SerializeField]
        [NonLocal]
        private TransformKey relativeObject;

        [SerializeField]
        private string withName;

        [SerializeField]
        [TagSelecter]
        private string withTag;

        [SerializeField]
        private bool ignoreOwner;

        [SerializeField]
        private bool mustBeVisuble;

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            if (relativeObject == null)
            {
                return State.Failure;
            }

            Transform _relativeObject = relativeObject.GetValue();
            if (_relativeObject == null)
            {
                return State.Failure;
            }

            GameObject[] gos = GameObject.FindGameObjectsWithTag(withTag);

            GameObject owner = _relativeObject.gameObject;
            Vector3 center = owner.transform.position;

            Transform closest = null;
            float minSqrDistance = float.PositiveInfinity;
            for (int i = 0; i < gos.Length; i++)
            {
                GameObject go = gos[i];
                if (ignoreOwner && go == owner ||
                    mustBeVisuble && !go.activeSelf ||
                    !string.IsNullOrEmpty(withName) && go.name != withName)
                {
                    continue;
                }

                Transform t = go.transform;

                Vector3 position = t.position;
                float sqrDistance = (center - position).sqrMagnitude;
                if (sqrDistance < minSqrDistance)
                {
                    closest = t;
                    minSqrDistance = sqrDistance;
                }
            }

            if (storeObject != null)
            {
                storeObject.SetValue(closest);
            }

            if (storeDistance != null)
            {
                storeDistance.SetValue(Mathf.Sqrt(minSqrDistance));
            }

            return State.Success;
        }
    }
}