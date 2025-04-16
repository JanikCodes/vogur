/* ================================================================
 ---------------------------------------------------
 Project   :    Apex
 Publisher :    Renowned Games
 Developer :    Tamerlan Shakirov
 ---------------------------------------------------
 Copyright 2020-2023 Renowned Games All rights reserved.
 ================================================================ */

using UnityEngine;

// Use this namespace to load apex inspector API.
using RenownedGames.Apex;

[System.Serializable]
public class TestCallback
{
    //[OnValueChanged(nameof(DisplayNewValue))]
    public float value;

    [Button]
    private void Message()
    {
        Debug.Log($"toto)) BTW: {value}");
    }

    private void DisplayNewValue()
    {
        Debug.Log(value);
    }
}

namespace RenownedGames.Apex.Demo
{
    // [HideMonoScript] attribute useful for hide default script field.
    [HideMonoScript]
    [AddComponentMenu("Apex/Demo/Example Component")]
    public class ExampleComponent : MonoBehaviour
    {
        public enum SomeValues
        {
            First,
            Second,
            Third,
        }

        public TestCallback[] testCallbacks;

        // With validators attributes you can validate field values.
        public float floatValue;

        [EnumToggleButtons]
        [OnValueChanged(nameof(TestCallback))]
        public SomeValues enumToggle;

        [SearchableEnum]
        public SomeValues searchableEnum;

        [ValueDropdown("valueReference")]
        public string dynamicValues;

        [Array]
        public string[] valueReference;

        private void TestCallback()
        {
            Debug.Log(enumToggle.ToString());
        }

        // With view attributes you can override GUI for fields.
        [Array]
        public int[] intArray;

        // With container attributes you can group your field and methods.
        [Group("Custom Group")]
        [Foldout("Nested Foldout Group", Style = "Group")]
        [Title("Properties")]
        public string stringValue;

        // With decorator attributes you can add additional GUI for fields
        [ObjectPreview]
        [Message("Hello this is a message!", Style = MessageStyle.Warning)]
        [HelpBox("Hello this is a helpbox message!", Style = MessageStyle.Info)]
        public GameObject objectValue;

        // With inline decorator attributes you can add additional GUI for fields
        [Prefix("Prefix")]
        [Suffix("Suffix")]
        public bool showButton;

#if UNITY_EDITOR
        [Button]
        [Color(0, 1, 0, 1, Target = ColorTarget.Background)]
        [HorizontalGroup("Custom Method Groups")]
        private void SayHello()
        {
            foreach (UnityEditor.SceneView scene in UnityEditor.SceneView.sceneViews)
            {
                scene.ShowNotification(new GUIContent("Hello Apex!"), 1.0f);
                scene.Repaint();
            }
        }

        [Button]
        [Color(1, 0, 0, 1, Target = ColorTarget.Background)]
        [ShowIf("showButton")]
        [HorizontalGroup("Custom Method Groups")]
        private void OpenProjectSettigns()
        {
            UnityEditor.SettingsService.OpenProjectSettings("Project/Player");
        }
#endif

        [OnObjectChanged]
        private void Verify()
        {
            Debug.Log("Verify");
        }

        //[OnObjectChanged(DelayCall = true)]
        //private void Verify2()
        //{
        //    Debug.Log("Verify DelayCall");
        //}

        [OnObjectGUIChanged]
        private void Verify3()
        {
            Debug.Log("Verify GUI DelayCall");
        }

        //private void OnValidate()
        //{
        //    Debug.Log("OnValidate");
        //}
    }
}