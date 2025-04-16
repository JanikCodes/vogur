/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Zinnur Davleev
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using RenownedGames.ApexEditor;
using System;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.AITreeEditor
{
    [DrawerTarget(typeof(KeySetup))]
    public class KeySetupDrawer : FieldDrawer
    {
        private SerializedField keyField;
        private SerializedField receiverField;

        public override void Initialize(SerializedField serializedField, GUIContent label)
        {
            keyField = serializedField.GetChild<SerializedField>(0);
            receiverField = serializedField.GetChild<SerializedField>(1);
        }

        public override void OnGUI(Rect position, SerializedField serializedField, GUIContent label)
        {
            HorizontalContainer horizontalContainer = new HorizontalContainer("HorizontalContainer", new System.Collections.Generic.List<VisualEntity>());

            keyField.SetLabel(GUIContent.none);
            horizontalContainer.AddEntity(keyField);

            if (keyField.GetObject() != null && receiverField.GetManagedReference() != null)
            {
                SerializedField valueField = receiverField.GetChild<SerializedField>(0);
                valueField.SetLabel(GUIContent.none);
                horizontalContainer.AddEntity(valueField);
            }

            horizontalContainer.OnGUI(position);

            if (keyField.IsValueChanged())
            {
                Key key = keyField.GetObject() as Key;

                if (key is SelfKey && key != null)
                {
                    keyField.SetObject(null);
                    EditorUtility.DisplayDialog("AI Tree", "You can't change Self key.", "Close");
                }
                else
                {
                    OnValueChanged();
                }
            }
        }

        private void OnValueChanged()
        {
            Key key = keyField.GetObject() as Key;
            if (key != null)
            {
                TypeCache.TypeCollection receiverTypes = TypeCache.GetTypesDerivedFrom<KeyReceiver>();
                for (int i = 0; i < receiverTypes.Count; i++)
                {
                    Type receiverType = receiverTypes[i];
                    if (!receiverType.IsAbstract && !receiverType.IsGenericType)
                    {
                        Type receiverKeyType = FindReceiverKeyType(receiverType);
                        if (receiverKeyType != null && (key.GetType() == receiverKeyType || key.GetType().IsSubclassOf(receiverKeyType)))
                        {
                            receiverField.SetManagedReference(Activator.CreateInstance(receiverType));
                            receiverField.ApplyChildren();
                            return;
                        }
                    }
                }
            }
            else
            {
                receiverField.SetManagedReference((object)null);
            }
        }

        private Type FindReceiverKeyType(Type type)
        {
            if (type.IsGenericType)
            {
                return type.GetGenericArguments()[0];
            }
            else if (type.BaseType != null)
            {
                return FindReceiverKeyType(type.BaseType);
            }
            return null;
        }
    }
}
