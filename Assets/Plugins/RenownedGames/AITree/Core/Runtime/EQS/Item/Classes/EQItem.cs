/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.AITree.EQS
{
    public class EQItem
    {
        private Vector3 position;
        private float score = 1f;
        private bool isValid = true;
        public string message;

        public EQItem(Vector3 position)
        {
            this.position = position;
        }

        #region [Getter / Setter]
        public Vector3 GetPosition()
        {
            return position;
        }

        public float GetScore()
        {
            return score;
        }

        public void SetScore(float score)
        {
            this.score = score;
        }

        public bool IsValid()
        {
            return isValid;
        }

        public void IsValid(bool value, string message = null)
        {
            isValid = value;
            this.message = message;
        }
        #endregion
    }
}