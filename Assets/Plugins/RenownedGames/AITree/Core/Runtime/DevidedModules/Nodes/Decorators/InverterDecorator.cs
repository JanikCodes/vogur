/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;

namespace RenownedGames.AITree.Nodes
{
    [HideMonoScript]
    [NodeContent("Inverter", "Inverter", IconPath = "Images/Icons/Node/InverterIcon.png")]
    public class InverterDecorator : DecoratorNode
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
                    return State.Failure;
                case State.Failure:
                    return State.Success;
                case State.Running:
                    return State.Running;
                default:
                    return State.Running;
            }
        }

        /// <summary>
        /// Called every tick regardless of the node execution.
        /// </summary>
        protected override void OnFlowUpdate() { }
    }
}