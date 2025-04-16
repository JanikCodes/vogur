/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022 - 2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree
{
    public abstract class IntervalServiceNode : ServiceNode
    {
        [Title("Service")]
        [SerializeField]
        [Min(0.001f)]
        private float interval = .5f;

        [SerializeField]
        [Min(0f)]
        private float randomDeviation = .1f;

        [SerializeField]
        private bool callTickOnEntry = false;

        [SerializeField]
        private bool restartTimerOnEntry = false;

        // Stored required properties.
        private float startTime;
        private float nextTickTime;

        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        protected override void OnEntry()
        {
            base.OnEntry();

            if (restartTimerOnEntry)
            {
                UpdateNextTickTime();
            }

            if (callTickOnEntry)
            {
                base.OnServiceUpdate();
            }
        }

        /// <summary>
        /// An internal method that includes the logic of calling the onTick() method.
        /// </summary>
        internal override void OnServiceUpdate()
        {
            if (nextTickTime < Time.time)
            {
                base.OnServiceUpdate();
                UpdateNextTickTime();
            }
        }

        /// <summary>
        /// Updates the time for the next update.
        /// </summary>
        private void UpdateNextTickTime()
        {
            float time = interval;
            if (randomDeviation > 0)
            {
                time += Random.Range(-randomDeviation, randomDeviation);
            }

            nextTickTime = Time.time + Mathf.Max(time, 0);
            startTime = Time.time;
        }

        /// <summary>
        /// Progress of node execution.
        /// </summary>
        /// <returns>If node support progress line, return value of normalized progress. Otherwise null.</returns>
        protected internal override float? GetProgress()
        {
            return (Time.time - startTime) / (nextTickTime - startTime);
        }


        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            if (randomDeviation > 0)
            {
                float min = Mathf.Max(interval - randomDeviation, 0);
                float max = interval + randomDeviation;
                return $"Tick every {min:0.##}s..{max:0.##}s";
            }
            else
            {
                return $"Tick every {interval:0.##}s";
            }
        }
    }
}