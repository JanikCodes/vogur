/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

namespace RenownedGames.AITree.Nodes
{
    [NodeContent("Succeeder", "Succeeder", IconPath = "Images/Icons/Node/SucceederIcon.png")]
    public class SucceederDecorator : DecoratorNode
    {
        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        /// <returns>State.</returns>
        protected override State OnUpdate()
        {
            switch (UpdateChild())
            {
                case State.Success:
                    return State.Success;
                default:
                case State.Failure:
                    return State.Success;
                case State.Aborted:
                    return State.Aborted;
                case State.Running:
                    return State.Running;
            }
        }

        /// <summary>
        /// Called every tick regardless of the node execution.
        /// </summary>
        protected override void OnFlowUpdate() { }
    }
}