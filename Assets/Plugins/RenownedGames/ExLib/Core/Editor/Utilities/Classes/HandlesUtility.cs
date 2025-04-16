/* ================================================================
   ----------------------------------------------------------------
   Project   :   ExLib
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright Renowned Games All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEngine;

namespace RenownedGames.ExLibEditor
{
    public static class HandlesUtility
    {
        public static Color color
        {
            get
            {
                return Handles.color;
            }
            set
            {
                Handles.color = value;
            }
        }

        #region [Extension Methods]
        public static void DrawSegmentCap(Vector3 from, Vector3 to, float thickness = 1)
        {
            Vector3 d = Vector3.Cross(Camera.current.transform.forward, to - from).normalized * .1f;
            Handles.DrawLine(to - d, to + d, thickness);
        }

        public static void DrawArrowCap(Vector3 from, Vector3 to, float thickness = 1)
        {
            Vector3 r = Vector3.Cross(Camera.current.transform.forward, to - from).normalized * .1f;
            Vector3 d = (from - to) * .1f;

            Handles.DrawLine(to, to + d - r, thickness);
            Handles.DrawLine(to, to + d + r, thickness);
        }

        public static void DrawSphere(Vector3 position, float radius)
        {
            Handles.SphereHandleCap(0, position, Quaternion.identity, radius * 2f, EventType.Repaint);
        }

        public static void DrawWireCircle(Vector3 position, float radius, Vector3 normal, float thickness)
        {
            Handles.DrawWireDisc(position, normal, radius, thickness);
        }

        public static void DrawSolidCircle(Vector3 position, float radius, Vector3 normal)
        {
            Handles.DrawSolidDisc(position, normal, radius);
        }
        #endregion
    }
}