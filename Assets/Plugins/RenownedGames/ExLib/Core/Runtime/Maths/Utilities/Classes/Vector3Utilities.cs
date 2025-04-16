/* ================================================================
   ----------------------------------------------------------------
   Project   :   ExLib
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright Renowned Games All rights reserved.
   ================================================================ */

using UnityEngine;

namespace RenownedGames.ExLib.Maths
{
    public static class Vector3Utilities
    {
        public static Vector2 ToXY(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static Vector2 ToYZ(this Vector3 vector)
        {
            return new Vector2(vector.y, vector.z);
        }

        public static Vector2 ToXZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }
    }
}
