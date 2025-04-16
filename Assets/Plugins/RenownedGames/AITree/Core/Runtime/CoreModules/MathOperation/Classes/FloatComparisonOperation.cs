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
    public abstract class FloatComparisonOperation : ComparisonOperation<float> { }

    [Serializable]
    [SearchContent("==")]
    public class EqualFloatOperation : FloatComparisonOperation
    {
        public override bool Result(float lhs, float rhs)
        {
            return lhs == rhs;
        }
    }

    [Serializable]
    [SearchContent("!=")]
    public class NotEqualFloatOperation : FloatComparisonOperation
    {
        public override bool Result(float lhs, float rhs)
        {
            return lhs != rhs;
        }
    }

    [Serializable]
    [SearchContent(">")]
    public class GreaterFloatOperation : FloatComparisonOperation
    {
        public override bool Result(float lhs, float rhs)
        {
            return lhs > rhs;
        }
    }

    [Serializable]
    [SearchContent("<")]
    public class LessFloatOperation : FloatComparisonOperation
    {
        public override bool Result(float lhs, float rhs)
        {
            return lhs < rhs;
        }
    }

    [Serializable]
    [SearchContent(">=")]
    public class GreaterOrEqualFloatOperation : FloatComparisonOperation
    {
        public override bool Result(float lhs, float rhs)
        {
            return lhs >= rhs;
        }
    }

    [Serializable]
    [SearchContent("<=")]
    public class LessOrEqualFloatOperation : FloatComparisonOperation
    {
        public override bool Result(float lhs, float rhs)
        {
            return lhs <= rhs;
        }
    }
}