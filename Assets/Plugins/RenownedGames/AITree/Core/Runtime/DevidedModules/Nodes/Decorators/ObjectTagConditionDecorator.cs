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
    [NodeContent("Object Tag Condition", "Object Tag Condition", IconPath = "Images/Icons/Node/ObjectTagIcon.png")]
    public class ObjectTagConditionDecorator : ConditionDecorator
    {
        [Title("Node")]
        [SerializeField]
        [NonLocal]
        private TransformKey objectToCheck;

        [SerializeField]
        [Array]
        private string[] tags;

        /// <summary>
        /// Calculates the result of the condition.
        /// </summary>
        protected override bool CalculateResult()
        {
            if (objectToCheck == null) return false;

            Transform transform = objectToCheck.GetValue();
            if (transform == null) return false;

            string objectTag = transform.gameObject.tag;
            for (int i = 0; i < tags.Length; i++)
            {
                if (tags[i] == objectTag)
                {
                    return true;
                }
            }

            return false;
        }

        #region [Getter / Setter]
        public TransformKey GetObjectToCheck()
        {
            return objectToCheck;
        }

        public string[] GetTags()
        {
            return tags;
        }

        public void SetTags(string[] tags)
        {
            this.tags = tags;
        }
        #endregion
    }
}