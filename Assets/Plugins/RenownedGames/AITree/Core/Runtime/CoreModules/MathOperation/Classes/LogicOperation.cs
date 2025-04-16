/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System;

namespace RenownedGames.AITree
{
    [Serializable]
    public abstract class LogicOperation : ComparisonOperation<bool> { }

    [Serializable]
    [SearchContent("AND")]
    public class LogicANDOperation : LogicOperation
    {
        public override bool Result(bool lhs, bool rhs)
        {
            return lhs && rhs;
        }
    }

    [Serializable]
    [SearchContent("NAND")]
    public class LogicNANDOperation : LogicOperation
    {
        public override bool Result(bool lhs, bool rhs)
        {
            return !(lhs && rhs);
        }
    }

    [Serializable]
    [SearchContent("OR")]
    public class LogicORperation : LogicOperation
    {
        public override bool Result(bool lhs, bool rhs)
        {
            return lhs || rhs;
        }
    }

    [Serializable]
    [SearchContent("XOR")]
    public class LogicXORperation : LogicOperation
    {
        public override bool Result(bool lhs, bool rhs)
        {
            return lhs != rhs;
        }
    }
}