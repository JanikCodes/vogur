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
    public abstract class IntComparisonOperation : ComparisonOperation<int> { }

    [Serializable]
    [SearchContent("==")]
    public class EqualIntOperation : IntComparisonOperation
    {
        public override bool Result(int lhs, int rhs)
        {
            return lhs == rhs;
        }
    }

    [Serializable]
    [SearchContent("!=")]
    public class NotEqualIntOperation : IntComparisonOperation
    {
        public override bool Result(int lhs, int rhs)
        {
            return lhs != rhs;
        }
    }

    [Serializable]
    [SearchContent(">")]
    public class GreaterIntOperation : IntComparisonOperation
    {
        public override bool Result(int lhs, int rhs)
        {
            return lhs > rhs;
        }
    }

    [Serializable]
    [SearchContent("<")]
    public class LessIntOperation : IntComparisonOperation
    {
        public override bool Result(int lhs, int rhs)
        {
            return lhs < rhs;
        }
    }

    [Serializable]
    [SearchContent(">=")]
    public class GreaterOrEqualIntOperation : IntComparisonOperation
    {
        public override bool Result(int lhs, int rhs)
        {
            return lhs >= rhs;
        }
    }

    [Serializable]
    [SearchContent("<=")]
    public class LessOrEqualIntOperation : IntComparisonOperation
    {
        public override bool Result(int lhs, int rhs)
        {
            return lhs <= rhs;
        }
    }
}