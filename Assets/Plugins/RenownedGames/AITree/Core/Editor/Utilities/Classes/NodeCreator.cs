/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.ExLibEditor;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.AITreeEditor
{
    public static class NodeCreator
    {
        [MenuItem("Assets/Create/Renowned Games/AI Tree/Script Templates/Task")]
        public static void CreateTask()
        {
            CreateScript("NewTask", "ScriptTemplates/TaskTemplate.txt", "Images/Icons/ScriptableObject/NodeIcon.png");
        }

        [MenuItem("Assets/Create/Renowned Games/AI Tree/Script Templates/Decorator/Condition Decorator")]
        public static void CreateConditionDecorator()
        {
            CreateScript("NewConditionDecorator", "ScriptTemplates/ConditionDecoratorTemplate.txt", "Images/Icons/ScriptableObject/DecoratorIcon.png");
        }

        [MenuItem("Assets/Create/Renowned Games/AI Tree/Script Templates/Decorator/Obserser Decorator")]
        public static void CreateObserserDecorator()
        {
            CreateScript("NewObserverDecorator", "ScriptTemplates/ObserverDecoratorTemplate.txt", "Images/Icons/ScriptableObject/DecoratorIcon.png");

        }

        [MenuItem("Assets/Create/Renowned Games/AI Tree/Script Templates/Service/Service")]
        public static void CreateService()
        {
            CreateScript("NewService", "ScriptTemplates/ServiceTemplate.txt", "Images/Icons/ScriptableObject/ServiceIcon.png");
        }

        [MenuItem("Assets/Create/Renowned Games/AI Tree/Script Templates/Service/Interval Service")]
        public static void CreateIntervalService()
        {
            CreateScript("NewIntervalService", "ScriptTemplates/IntervalServiceTemplate.txt", "Images/Icons/ScriptableObject/ServiceIcon.png");
        }

        private static void CreateScript(string fileName, string templateLocalPath, string iconLocalPath)
        {
            TextAsset templateAsset = EditorResources.LoadExact<TextAsset>("RenownedGames/AITree", templateLocalPath);
            if (templateAsset == null)
            {
                Debug.LogWarning("Missing template.");
                return;
            }
            string templatePath = AssetDatabase.GetAssetPath(templateAsset);
            Texture2D icon = EditorResources.LoadExact<Texture2D>("RenownedGames/AITree", iconLocalPath);

            ProjectWindowUtility.CreateScriptAssetFromTemplateFile(templatePath, fileName, icon);
        }
    }
}

