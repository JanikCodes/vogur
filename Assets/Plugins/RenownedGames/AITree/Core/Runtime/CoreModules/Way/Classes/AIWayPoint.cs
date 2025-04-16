/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Company   :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using UnityEngine;

namespace RenownedGames.AITree
{
    [HideMonoScript]
    [AddComponentMenu("Renowned Games/AI Tree/Ways/AI Way Point")]
    [DisallowMultipleComponent]
    public class AIWayPoint : MonoBehaviour
    {
        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}