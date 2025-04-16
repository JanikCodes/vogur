/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using System;
using UnityEngine;

namespace RenownedGames.AITree.PerceptionSystem
{
    [SearchContent("Sight", Image = "Images/Icons/Perception/EyeIcon.png")]
    public class AIPerceptionSightConfig : AIPerceptionConfig
    {
        [SerializeField]
        private float heightOffset = 1.635f;

        [SerializeField]
        [Slider(0f, 360f)]
        private float fov = 90f;

        [SerializeField]
        private Vector2 range = new Vector2(2, 10);

        [SerializeField]
        [DisableInPlayMode]
        [MinValue(1)]
        private int maxQuery = 10;

        [SerializeField]
        private LayerMask cullingLayer = ~0;

        [SerializeField]
        private LayerMask obstacleLayer = ~0;

        // Stored required properties.
        private Transform transform;
        private Collider[] colliders;

        /// <summary>
        /// Called once when initializing config.
        /// </summary>
        /// <param name="owner">Reference of AI perception owner.</param>
        public override void Initialize(AIPerception owner)
        {
            base.Initialize(owner);
            transform = owner.transform;
            colliders = new Collider[maxQuery];
        }

        /// <summary>
        /// Called every fixed frame-rate frame, 
        /// 0.02 seconds (50 calls per second) is the default time between calls.
        /// </summary>
        protected internal override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            AIPerceptionSource source = FindSource();
            OnTargetUpdated?.Invoke(source);
        }

        private AIPerceptionSource FindSource()
        {
            Vector3 eyePos = transform.position + Vector3.up * heightOffset;

            float minDistance = Mathf.Min(range.x, range.y);
            float maxDistance = Mathf.Max(range.x, range.y);

            int count = Physics.OverlapSphereNonAlloc(eyePos, maxDistance, colliders, cullingLayer);
            for (int i = 0; i < count; i++)
            {
                Collider collider = colliders[i];
                AIPerceptionSource source = collider.GetAIPerceptionSource();
                if (source != null && source.IsObservable() && source.transform != transform)
                {
                    Vector3 point = collider.bounds.center;
                    if (Physics.Linecast(eyePos, point, out RaycastHit hitInfo, cullingLayer | obstacleLayer))
                    {
                        if(hitInfo.transform == source.transform)
                        {
                            Vector3 direction = point - eyePos;
                            direction.y = 0f;

                            float angle = Vector3.Angle(transform.forward, direction.normalized);
                            if (angle <= fov / 2 || Vector3.Distance(eyePos, point) <= minDistance)
                            {
                                return source;
                            }
                        }
                    }
                }
            }
            return null;
        }

        #region [Event Callback Functions]
        /// <summary>
        /// Called when perception config has updated the target.
        /// <br><b>Param type of(AIPerceptionSource)</b>: New reference of AI perception source.</br>
        /// </summary>
        public override event Action<AIPerceptionSource> OnTargetUpdated;
        #endregion

        #region [Gizmos]
        /// <summary>
        /// Called while selecting object, use for debug.
        /// </summary>
        protected internal override void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;

            Vector3 center = GetOwner().transform.position + Vector3.up * heightOffset;
            Vector3 forward = GetOwner().transform.forward;

            float minDistance = Mathf.Min(range.x, range.y);
            float maxDistance = Mathf.Max(range.x, range.y);

            DrawFOV(fov, maxDistance, center, forward);
            DrawArc(360 - fov, minDistance, center, -forward);
        }

        /// <summary>
        /// Draw field of view angle.
        /// <br><b>Editor-only, use only in OnGizmos methods.</b></br>
        /// </summary>
        /// <param name="angle">Field of view angle.</param>
        /// <param name="radius">Field of view radius.</param>
        /// <param name="center">Center vector.</param>
        /// <param name="forward">Forward vector.</param>
        private void DrawFOV(float angle, float radius, Vector3 center, Vector3 forward)
        {
            DrawArc(angle, radius, center, forward);

            float halfRad = angle * .5f * Mathf.Deg2Rad;
            Vector3 right = Vector3.Cross(forward, Vector3.up);
            Vector3 a = center + right * Mathf.Sin(-halfRad) * radius + forward * Mathf.Cos(-halfRad) * radius;
            Vector3 b = center + right * Mathf.Sin(halfRad) * radius + forward * Mathf.Cos(halfRad) * radius;

            Gizmos.DrawLine(center, a);
            Gizmos.DrawLine(center, b);
        }

        /// <summary>
        /// Draw field of view arc.
        /// <br><b>Editor-only, use only in OnGizmos methods.</b></br>
        /// </summary>
        /// <param name="angle">Field of view angle.</param>
        /// <param name="radius">Field of view radius.</param>
        /// <param name="center">Center vector.</param>
        /// <param name="forward">Forward vector.</param>
        private void DrawArc(float angle, float radius, Vector3 center, Vector3 forward)
        {
            angle = Mathf.Clamp(angle, 0, 360);
            int count = 16;
            Vector3 right = Vector3.Cross(forward, Vector3.up);

            float halfRad = angle * .5f * Mathf.Deg2Rad;
            float radStep = halfRad / count * 2f;

            Vector3? lastPoint = null;
            for (int i = 0; i <= count; i++)
            {
                float rad = -halfRad + radStep * i;
                
                float x = Mathf.Sin(rad) * radius;
                float y = Mathf.Cos(rad) * radius;

                Vector3 point = center + right * x + forward * y;
                if (lastPoint.HasValue)
                {
                    Gizmos.DrawLine(lastPoint.Value, point);
                }
                lastPoint = point;
            }
        }
        #endregion

        #region [Getter / Setter]
        public float GetFov()
        {
            return fov;
        }

        public float GetMinDistance()
        {
            return Mathf.Min(range.x, range.y);
        }

        public float GetMaxDistnce()
        {
            return Mathf.Max(range.x, range.y);
        }

        public float GetHeightOffset()
        {
            return heightOffset;
        }

        public LayerMask GetCullingLayer()
        {
            return cullingLayer;
        }

        public LayerMask GetObstacleLayer()
        {
            return obstacleLayer;
        }
        #endregion
    }
}