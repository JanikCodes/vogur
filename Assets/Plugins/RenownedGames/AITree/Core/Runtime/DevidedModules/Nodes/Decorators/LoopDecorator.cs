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
    [NodeContent("Loop", "Loop", IconPath = "Images/Icons/Node/LoopIcon.png")]
    public class LoopDecorator : DecoratorNode
    {
        private enum LoopType
        {
            Count,
            RandomCount,
            Infinite,
            Until
        }

        private enum UntilType
        {
            UntilSuccess,
            UntilFailure
        }

        [Title("Decorator")]
        [SerializeField]
        private LoopType type = LoopType.Count;

        [SerializeField]
        [ShowIf("type", LoopType.Count)]
        [MinValue(1)]
        private int numLoops = 3;

        [SerializeField]
        [ShowIf("type", LoopType.RandomCount)]
        [MinVector2(1, "rangeLoops.x")]
        private Vector2Int rangeLoops = new Vector2Int(2, 3);

        [SerializeField]
        [ShowIf("type", LoopType.Infinite)]
        private float infiniteLoopTimeoutTime = -1f;

        [SerializeField]
        [ShowIf("type", LoopType.Infinite)]
        [DisableIf("FinishStateDisableIf")]
        private State finishState = State.Failure;

        [SerializeField]
        [ShowIf("type", LoopType.Until)]
        private UntilType untilType;

        // Stored required properties.
        private int currentLoop;
        private float startTime;
        private int storedLoopCount;

        /// <summary>
        /// Called when behaviour tree enter in node.
        /// </summary>
        protected override void OnEntry()
        {
            base.OnEntry();

            currentLoop = 0;
            startTime = Time.time;
            storedLoopCount = numLoops;
            if (type == LoopType.RandomCount)
            {
                storedLoopCount = Random.Range(rangeLoops.x, rangeLoops.y + 1);
            }
        }

        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            switch (type)
            {
                case LoopType.Count:
                case LoopType.RandomCount:
                    State state = UpdateChild();
                    if (state != State.Running)
                    {
                        currentLoop++;
                    }
                    return currentLoop >= storedLoopCount ? state : State.Running;
                case LoopType.Infinite:
                    UpdateChild();
                    if (infiniteLoopTimeoutTime > 0)
                    {
                        if (Time.time - startTime >= infiniteLoopTimeoutTime)
                        {
                            ResetChild(finishState);
                            return finishState;
                        }
                    }
                    return State.Running;
                case LoopType.Until:
                    switch (untilType)
                    {
                        case UntilType.UntilSuccess:
                            switch (UpdateChild())
                            {
                                case State.Running:
                                    return State.Running;
                                case State.Success:
                                    return State.Success;
                                case State.Failure:
                                    return State.Running;
                                default:
                                    return State.Running;
                            }
                        case UntilType.UntilFailure:
                            switch (UpdateChild())
                            {
                                case State.Running:
                                    return State.Running;
                                case State.Success:
                                    return State.Running;
                                case State.Failure:
                                    return State.Failure;
                                default:
                                    return State.Running;
                            }
                    }
                    break;
            }

            return State.Failure;
        }

        /// <summary>
        /// Called every tick regardless of the node execution.
        /// </summary>
        protected override void OnFlowUpdate() { }

        #region [IEntityDescription Implementation]
        /// <summary>
        /// Detail description of entity.
        /// </summary>
        public override string GetDescription()
        {
            string description = "Loop: ";

            switch (type)
            {
                case LoopType.Count:
                    description += $"{numLoops} loops";
                    break;
                case LoopType.RandomCount:
                    description += $"[{rangeLoops.x}, {rangeLoops.y}] loops";
                    break;
                case LoopType.Infinite:
                    if (infiniteLoopTimeoutTime < 0)
                    {
                        description += "infinite";
                    }
                    else
                    {
                        description += $"loop for {infiniteLoopTimeoutTime} seconds";
                    }
                    break;
                case LoopType.Until:
                    switch (untilType)
                    {
                        case UntilType.UntilSuccess:
                            description += "Until Success";
                            break;
                        case UntilType.UntilFailure:
                            description += "Until Failure";
                            break;
                    }
                    break;
            }

            return description;
        }
        #endregion

        #region [Editor]
#if UNITY_EDITOR
        private bool FinishStateDisableIf()
        {
            return infiniteLoopTimeoutTime <= 0;
        }
#endif
        #endregion
    }
}
