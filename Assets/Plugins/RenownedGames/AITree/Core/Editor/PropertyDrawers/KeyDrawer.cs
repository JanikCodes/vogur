/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using RenownedGames.ApexEditor;
using RenownedGames.ExLibEditor;
using RenownedGames.ExLib.Reflection;
using RenownedGames.ExLibEditor.Windows;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RenownedGames.AITreeEditor
{
    [DrawerTarget(typeof(Key), Subclasses = true)]
    public sealed class KeyDrawer : FieldDrawer
    {
        /// <summary>
        /// Miscellaneous helper GUI stuff for key drawer.
        /// </summary>
        public static class Styles
        {
            private static Texture NoneIcon;
            private static Texture LocalIcon;
            private static Texture SyncIcon;
            private static GUIStyle PopupStyle;

            static Styles()
            {
                NoneIcon = CreateEmptyIcon();
                LocalIcon = EditorResources.LoadExact<Texture>("RenownedGames/AITree", "Images/Icons/Keys/LocalKeyIcon.png");
                SyncIcon = EditorResources.LoadExact<Texture>("RenownedGames/AITree", "Images/Icons/Keys/SyncKeyIcon.png");
            }

            public static Texture GetLocalKeyIcon()
            {
                return LocalIcon;
            }

            public static Texture GetSyncKeyIcon()
            {
                return SyncIcon;
            }

            public static Texture GetNoneKeyIcon()
            {
                return NoneIcon;
            }

            /// <summary>
            /// Use it only in GUI calls.
            /// </summary>
            public static GUIStyle GetPopupStyle()
            {
                if(PopupStyle == null)
                {
                    PopupStyle = new GUIStyle(EditorStyles.popup);
                    PopupStyle.richText = true;
                }
                return PopupStyle;
            }

            private static Texture2D CreateEmptyIcon()
            {
                Texture2D texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, Color.clear);
                texture.Apply();
                return texture;
            }
        }

        private bool localizable;
        private SerializedProperty runnerProperty;
        private Func<BehaviourRunner> runnerGetter;
        private SerializedField localKeyField;

        /// <summary>
        /// Called once when initializing serialized field drawer.
        /// </summary>
        /// <param name="serializedField">Serialized field with DrawerAttribute.</param>
        /// <param name="label">Label of serialized field.</param>
        public override void Initialize(SerializedField serializedField, GUIContent label)
        {
            localizable = IsLocalizable(serializedField);
            InitializeRunnerLink(serializedField);
        }

        /// <summary>
        /// Called for rendering and handling drawer GUI.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the serialized field drawer GUI.</param>
        /// <param name="serializedField">Reference of serialized field with drawer attribute.</param>
        /// <param name="label">Display label of serialized field.</param>
        public override void OnGUI(Rect position, SerializedField serializedField, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);

            if (localizable)
            {
                position.width -= 20;
                DrawToggle(position, serializedField);
            }

            Key key = serializedField.GetObject() as Key;
            if (key == null || !key.IsLocal())
            {
                DrawPopup(position, serializedField);
            }
            else
            {
                DrawLocalEditor(position, serializedField);
            }
        }

        /// <summary>
        /// Get height which needed to serialized field drawer.
        /// </summary>
        /// <param name="serializedField">Serialized field with DrawerAttribute.</param>
        /// <param name="label">Label of serialized field.</param>
        public override float GetHeight(SerializedField serializedField, GUIContent label)
        {
            Key key = serializedField.GetObject() as Key;
            if (key == null || !key.IsLocal())
            {
                return EditorGUIUtility.singleLineHeight;
            }
            else
            {
                ValidateLocalField(serializedField, ref localKeyField);
                return localKeyField.GetHeight();
            }
        }

        /// <summary>
        /// Draw key popup control field.
        /// </summary>
        /// <param name="position">Rectangle position.</param>
        /// <param name="serializedField">Serialized field of key.</param>
        private void DrawPopup(Rect position, SerializedField serializedField)
        {
            Key key = serializedField.GetObject() as Key;

            string keyLabel = "None";
            if (key != null)
            {
                keyLabel = key.name;
                if (EditorApplication.isPlaying)
                {
                    bool isShared = AssetDatabase.IsNativeAsset(key);
                    keyLabel += isShared ? "<i> (Shared)</i>" : "<i> (Associated)</i>";
                }
            }

            if (GUI.Button(position, keyLabel, Styles.GetPopupStyle()))
            {
                SelectDropdown(position, serializedField);
            }
        }

        /// <summary>
        /// Draw editor field of local key.
        /// </summary>
        /// <param name="position">Rectangle position.</param>
        /// <param name="serializedField">Serialized field of key.</param>
        private void DrawLocalEditor(Rect position, SerializedField serializedField)
        {
            ValidateLocalField(serializedField, ref localKeyField);

            localKeyField.GetSerializedObject().Update();
            localKeyField.OnGUI(position);
            if (localKeyField.GetSerializedObject().hasModifiedProperties)
            {
                localKeyField.GetSerializedObject().ApplyModifiedProperties();
            }
        }

        /// <summary>
        /// Draw toggle control field to create or delete local key.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="serializedField">Serialize field of key.</param>
        private void DrawToggle(Rect position, SerializedField serializedField)
        {
            Key key = serializedField.GetObject() as Key;
            bool isActive = key?.IsLocal() ?? false;

            Rect toggleRect = new Rect(position.xMax + 2, position.y + 1, 18, position.height);
            EditorGUI.BeginChangeCheck();
            EditorGUI.Toggle(toggleRect, isActive, EditorStyles.radioButton);
            if (EditorGUI.EndChangeCheck())
            {
                OnToggleClicked(position, serializedField, !isActive);
            }
        }

        /// <summary>
        /// Dropdown search window of all available keys from blackboard for this key field.
        /// </summary>
        /// <param name="position">Rectangle position of button.</param>
        /// <param name="serializedField">Serialized field of key</param>
        private void SelectDropdown(Rect position, SerializedField serializedField)
        {
            Type keyType = serializedField.GetMemberType();
            Type[] validTypes = GetValidTypes(serializedField);
            ExSearchWindow window = ExSearchWindow.Create($"Keys");
            window.SetSortType(ExSearchWindow.SortType.None);

            window.AddEntry(new GUIContent("None", Styles.GetNoneKeyIcon()), null, SelectKey);

            if (TryGetSelfBlackboard(serializedField, out Blackboard selfBlackboard))
            {
                string bbName = $"{selfBlackboard.name} (this)";
                foreach (Key key in selfBlackboard.Keys)
                {
                    if(keyType != key.GetType() && !key.GetType().IsSubclassOf(keyType))
                    {
                        continue;
                    }

                    if (validTypes != null && !validTypes.Contains(key.GetValueType()))
                    {
                        continue;
                    }

                    string path = System.IO.Path.Combine(bbName, key.GetCategory(), key.name);
                    path = path.Replace('\\', '/');
                    GUIContent content = new GUIContent(path, key.IsSync() ? Styles.GetSyncKeyIcon() : Styles.GetLocalKeyIcon());
                    window.AddEntry(content, key, SelectKey);
                }
            }

            Blackboard[] blackboards = BlackboardUtility.GetAllBlackboards();
            for (int i = 0; i < blackboards.Length; i++)
            {
                Blackboard blackboard = blackboards[i];
                if(selfBlackboard != null && blackboard == selfBlackboard)
                {
                    continue;
                }

                if (blackboard != null)
                {
                    foreach (Key key in blackboard.Keys)
                    {
                        if (key.IsSync())
                        {
                            if (keyType != key.GetType() && !key.GetType().IsSubclassOf(keyType))
                            {
                                continue;
                            }

                            if (validTypes != null && !validTypes.Contains(key.GetValueType()))
                            {
                                continue;
                            }

                            string path = System.IO.Path.Combine("All Synced", blackboard.name, key.GetCategory(), key.name);
                            path = path.Replace('\\', '/');
                            GUIContent content = new GUIContent(path, Styles.GetSyncKeyIcon());
                            window.AddEntry(content, key, SelectKey);
                        }
                    }
                }
            }
            window.Open(position);

            void SelectKey(object data)
            {
                Key key = data as Key;
                Object target = serializedField.GetSerializedObject().targetObject;

                serializedField.SetObject(key);
                serializedField.GetSerializedObject().ApplyModifiedProperties();

                if (target is Node node)
                {
                    new SerializedObject(node.GetBehaviourTree()).ApplyModifiedProperties();
                }
            }

        }

        /// <summary>
        /// Dropdown search window of available key types to create as local for this key field.
        /// </summary>
        /// <param name="position">Rectangle position of button.</param>
        /// <param name="serializedField">Serialized field of key</param>
        private void CreateDropdown(Rect position, SerializedField serializedField)
        {
            ExSearchWindow searchWindow = ExSearchWindow.Create("Keys");

            Type keyType = serializedField.GetMemberType();
            Type[] validTypes = GetValidTypes(serializedField);
            TypeCache.TypeCollection keyTypeImpls = TypeCache.GetTypesDerivedFrom<Key>();
            for (int i = 0; i < keyTypeImpls.Count; i++)
            {
                Type keyImpl = keyTypeImpls[i];
                if (keyImpl.IsGenericType || keyImpl == typeof(SelfKey))
                {
                    continue;
                }

                if (keyType != keyImpl && !keyImpl.IsSubclassOf(keyType))
                {
                    continue;
                }

                Type valueType = KeyEditorUtility.GetValueType(keyImpl);
                if (validTypes != null && !validTypes.Contains(valueType))
                {
                    continue;
                }

                string name = KeyEditorUtility.NicifyValueType(valueType);
                if (keyImpl.GetCustomAttribute<ObsoleteAttribute>() != null)
                {
                    name = $"Deprecated/{name}";
                }

                searchWindow.AddEntry(name, () => CreateLocalKey(serializedField, keyImpl));
            }

            searchWindow.Open(position);
        }

        /// <summary>
        /// Called when toggle radio button of local key has been clicked.
        /// </summary>
        /// <param name="position">Rectangle position of popup control field.</param>
        /// <param name="serializedField">Serialized field of key.</param>
        /// <param name="isActive">True if toggle radio button is turned on. Otherwise false.</param>
        private void OnToggleClicked(Rect position, SerializedField serializedField, bool isActive)
        {
            if (isActive)
            {
                Type keyType = typeof(Key);
                Type type = serializedField.GetMemberType();
                if (type.IsGenericType)
                {
                    CreateLocalKey(serializedField, type.GetGenericArguments()[0]);
                }
                else if (type.IsSubclassOf(keyType))
                {
                    CreateLocalKey(serializedField, type);
                }
                else if (type == keyType)
                {
                    CreateDropdown(position, serializedField);
                }
            }
            else
            {
                DeleteLocalKey(serializedField);
            }
        }

        /// <summary>
        /// Validate serialized field of local key instance if is null create it.
        /// </summary>
        /// <param name="serializedField">Serialized field of key.</param>
        /// <param name="localKeyField">Reference of local key serialized field.</param>
        private void ValidateLocalField(SerializedField serializedField, ref SerializedField localKeyField)
        {
            try
            {
                localKeyField.GetSerializedObject().targetObject.GetType();
            }
            catch
            {
                Key key = serializedField.GetObject() as Key;
                SerializedObject serializedObject = new SerializedObject(key);
                localKeyField = new SerializedField(serializedObject, "value");
                localKeyField.SetLabel(GUIContent.none);
            }
        }

        /// <summary>
        /// Create local key instance.
        /// </summary>
        /// <param name="serializedField">Serialized field of key.</param>
        /// <param name="type">Type of new key object.</param>
        private void CreateLocalKey(SerializedField serializedField, Type type)
        {
            Undo.IncrementCurrentGroup();

            Object target = serializedField.GetSerializedObject().targetObject;

            Key instanceKey = ScriptableObject.CreateInstance(type) as Key;
            instanceKey.name = $"{serializedField.GetSerializedProperty().name}";
            instanceKey.IsLocal(true);

            if (!EditorApplication.isPlaying)
            {
                if (target is ScriptableObject scriptableObject)
                {
                    AssetDatabase.AddObjectToAsset(instanceKey, scriptableObject);

                    bool isBehaviourTree = false;
                    if (BehaviourTreeWindow.HasOpenInstances() && scriptableObject is Node node)
                    {
                        BehaviourTree currentTree = node.GetBehaviourTree();
                        if(currentTree != null)
                        {
                            BehaviourTreeWindow[] windows = BehaviourTreeWindow.GetInstances();
                            for (int i = 0; i < windows.Length; i++)
                            {
                                BehaviourTreeWindow window = windows[i];
                                if(window.GetSelectedTree() == currentTree)
                                {
                                    window.MarkAsChanged();
                                    isBehaviourTree = true;
                                }
                            }
                        }
                    }

                    if (!isBehaviourTree)
                    {
                        EditorUtility.SetDirty(target);
                        AssetDatabase.SaveAssetIfDirty(target);
                    }
                }
            }

            Undo.RegisterCreatedObjectUndo(instanceKey, "[Key] Create local key");

            Undo.RecordObject(target, "[Key] Add key to asset");

            serializedField.GetSerializedObject().Update();
            serializedField.SetObject(instanceKey);
            serializedField.GetSerializedObject().ApplyModifiedProperties();

            Undo.SetCurrentGroupName("[Key] Create local key");
        }

        /// <summary>
        /// Delete local key instance.
        /// </summary>
        /// <param name="serializedField">Serialized field of key.</param>
        private void DeleteLocalKey(SerializedField serializedField)
        {
            Undo.IncrementCurrentGroup();

            Object target = serializedField.GetSerializedObject().targetObject;
            Object keyToRemove = serializedField.GetObject();

            Undo.RecordObject(target, "[Key] Delete key from node");

            serializedField.GetSerializedObject().Update();
            serializedField.SetObject(null);
            serializedField.GetSerializedObject().ApplyModifiedProperties();

            Node node = target as Node;
            if (node != null)
            {
                new SerializedObject(node.GetBehaviourTree()).ApplyModifiedProperties();
            }

            if (target is ScriptableObject)
            {
                Undo.DestroyObjectImmediate(keyToRemove);

                if (!EditorApplication.isPlaying)
                {
                    bool isBehaviourTree = false;
                    if (BehaviourTreeWindow.HasOpenInstances() && node != null)
                    {
                        BehaviourTree currentTree = node.GetBehaviourTree();
                        if (currentTree != null)
                        {
                            BehaviourTreeWindow[] windows = BehaviourTreeWindow.GetInstances();
                            for (int i = 0; i < windows.Length; i++)
                            {
                                BehaviourTreeWindow window = windows[i];
                                if (window.GetSelectedTree() == currentTree)
                                {
                                    window.MarkAsChanged();
                                    isBehaviourTree = true;
                                }
                            }
                        }
                    }

                    if (!isBehaviourTree)
                    {
                        EditorUtility.SetDirty(target);
                        AssetDatabase.SaveAssetIfDirty(target);
                    }
                }
            }

            Undo.SetCurrentGroupName("[Key] Delete local key");
        }

        /// <summary>
        /// Try to find Behaviour Runner or Behaviour Tree and load its blackboard.
        /// </summary>
        /// <param name="selfBlackboard">Output reference of backboard.</param>
        /// <returns>Blackboard reference. Otherwise null.</returns>
        private bool TryGetSelfBlackboard(SerializedField field, out Blackboard selfBlackboard)
        {
            Object target = field.GetSerializedObject().targetObject;

            BehaviourRunner runner = null;
            if (target is Node node)
            {
                if (!EditorApplication.isPlaying)
                {
                    selfBlackboard = node.GetBehaviourTree().GetBlackboard();
                    return selfBlackboard != null;
                }
                else
                {
                    runner = node.GetOwner();
                }
            }
            else
            {
                runner = GetRunnerLink();
                if (runner == null && target is MonoBehaviour monoBehaviour)
                {
                    runner = monoBehaviour.GetComponentInParent<BehaviourRunner>();
                }
            }

            if (runner != null)
            {
                selfBlackboard = EditorApplication.isPlaying ? runner.GetBlackboard() : runner.GetSharedBlackboard();
                return selfBlackboard != null;
            }

            selfBlackboard = null;
            return false;
        }

        /// <summary>
        /// Check if [RunnerLink] attribute is defined and cache of link to BuhaviourRunner.
        /// </summary>
        /// <param name="serializedField">Serialized field of key.</param>
        private void InitializeRunnerLink(SerializedField serializedField)
        {
            RunnerLinkAttribute attribute = serializedField.GetMemberInfo().GetCustomAttribute<RunnerLinkAttribute>();
            if (attribute != null)
            {
                if (!string.IsNullOrEmpty(attribute.Property))
                {
                    SerializedObject serializedObject = serializedField.GetSerializedObject();

                    string path = attribute.Property;
                    string keyPath = serializedField.GetSerializedProperty().propertyPath;

                    int index = keyPath.LastIndexOf('.');
                    if (index > 0)
                    {
                        path = $"{keyPath.Remove(index)}.{path}";
                    }

                    runnerProperty = serializedObject.FindProperty(path);
                    if (runnerProperty == null)
                    {
                        Debug.LogError($"AI Tree [RunnerLink]: {attribute.Property} property not found.");
                    }
                }
                else if (!string.IsNullOrEmpty(attribute.Method))
                {
                    Type type = serializedField.GetMemberType();
                    foreach (MethodInfo methodInfo in type.AllMethods(typeof(MonoBehaviour)))
                    {
                        if (methodInfo.Name == attribute.Method
                            && methodInfo.ReturnType == typeof(BehaviourRunner)
                            && methodInfo.GetParameters().Length == 0)
                        {
                            runnerGetter = (Func<BehaviourRunner>)methodInfo.CreateDelegate(typeof(Func<BehaviourRunner>), serializedField.GetDeclaringObject());
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get valid types of this key value type defined by [KeyTypes] attribute.
        /// </summary>
        /// <param name="field">Serialized field of key.</param>
        /// <returns>If field has [KeyTypes] attribute with specified keys return type array. Otherwise null.</returns>
        private Type[] GetValidTypes(SerializedField field)
        {
            KeyTypesAttribute keyTypesAttribute = field.GetMemberInfo().GetCustomAttribute<KeyTypesAttribute>();
            if (keyTypesAttribute != null && keyTypesAttribute.types != null && keyTypesAttribute.types.Length > 0)
            {
                return keyTypesAttribute.types;
            }
            return null;
        }

        /// <summary>
        /// Get BehaviourRunner for [RunnerLink] attribute if is defined.
        /// </summary>
        /// <returns>BehaviourRunner, otherwise null.</returns>
        private BehaviourRunner GetRunnerLink()
        {
            if(runnerProperty != null)
            {
                return runnerProperty.objectReferenceValue as BehaviourRunner;
            }
            else if(runnerGetter != null)
            {
                return runnerGetter.Invoke();
            }
            return null;
        }

        /// <summary>
        /// Check if key serialized field can be used as local.
        /// </summary>
        /// <param name="serializedField">Serialized field of key.</param>
        /// <returns>True if key can be used as local. Otherwise false.</returns>
        private bool IsLocalizable(SerializedField serializedField)
        {
            return serializedField.GetMemberInfo().GetCustomAttribute<NonLocalAttribute>() == null;
        }
    }
}