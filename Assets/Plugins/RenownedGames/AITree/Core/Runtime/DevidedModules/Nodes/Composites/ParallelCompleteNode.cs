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
    [NodeContent("Parallel Complete", "Composites/Parallels/Parallel Complete", IconPath = "Images/Icons/Node/ParallelIcon.png")]
    public class ParallelCompleteNode : ParallelNode
    {
        /// <summary>
        /// Called every tick during node execution.
        /// </summary>
        protected override State OnUpdate()
        {
            bool skipLeftNodes = GetSkipLeftNodes();

            for (int i = 0; i < childrenLeftToExecute.Length; i++)
            {
                Node child = GetChild(i);
                if (!skipLeftNodes || skipLeftNodes && childrenLeftToExecute[i] == State.Running)
                {
                    if (child == null)
                    {
                        AbortRunningChildren();
                        return State.Failure;
                    }

                    State state = child.Update();
                    switch (state)
                    {
                        case State.Success:
                            AbortRunningChildren();
                            return State.Success;
                        case State.Failure:
                            AbortRunningChildren();
                            return State.Failure;
                        case State.Aborted:
                            AbortRunningChildren();
                            return State.Aborted;
                        case State.Running:
                            break;
                    }

                    childrenLeftToExecute[i] = state;
                }
            }

            return State.Running;
        }
    }
}