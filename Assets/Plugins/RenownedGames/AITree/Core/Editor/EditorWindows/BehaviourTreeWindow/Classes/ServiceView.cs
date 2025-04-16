/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov, Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using UnityEditor;

namespace RenownedGames.AITreeEditor
{
    internal class ServiceView : AuxiliaryView
    {
        /// <summary>
        /// Service view constructor.
        /// </summary>
        /// <param name="graph">The graph where is contained view.</param>
        /// <param name="service">Service node reference.</param>
        public ServiceView(BehaviourTreeGraph graph, ServiceNode service) : base(graph, service, AssetDatabase.GetAssetPath(AITreeSettings.instance.GetServiceUXML())) { }

        /// <summary>
        /// Called once when loading node view to initialize styles.
        /// </summary>
        protected override void InitializeStyles()
        {
            if (GetNode() is TempServiceNode)
            {
                AddToClassList("temp-service");
            }
            else
            {
                AddToClassList("service");
            }
        }
    }
}