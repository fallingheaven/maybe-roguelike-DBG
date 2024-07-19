using Editor;
using Utility.Attribute;

namespace Editor
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class ScriptableObjectField : VisualElement
    {
        private Label m_TitleLabel;
        private ObjectField m_ObjectField;
        private VisualElement m_ScriptableObjectView;
        private SerializedProperty m_SerializedProperty;
        private UnityEngine.Object m_Owner;

        public ScriptableObjectField(Type targetType, string bindPath, SerializedProperty serializedProperty)
        {
            var visualTree = Resources.Load<VisualTreeAsset>("VisualTree/ScriptableObjectField");
            if (visualTree == null) return;
            
            visualTree.CloneTree(this);
            AddToClassList("scriptableObjectField");

            s_Owners.Add(serializedProperty.serializedObject.targetObject);
            m_SerializedProperty = serializedProperty;
            m_Owner = serializedProperty.serializedObject.targetObject;

            var top = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row
                },
                name = "top"
            };
            Add(top);

            m_TitleLabel = new Label(serializedProperty.displayName);
            m_TitleLabel.AddManipulator(new Clickable(() => m_ScriptableObjectView.style.display = m_ScriptableObjectView.style.display == DisplayStyle.Flex ? DisplayStyle.None : DisplayStyle.Flex));
            m_TitleLabel.AddToClassList("title");
            top.Add(m_TitleLabel);

            m_ObjectField = new ObjectField
            {
                name = "objectField",
                objectType = targetType,
                bindingPath = bindPath
            };
            m_ObjectField.Bind(m_SerializedProperty.serializedObject);
            m_ObjectField.RegisterValueChangedCallback(OnValueChanged);
            top.Add(m_ObjectField);

            m_ScriptableObjectView = new VisualElement
            {
                name = "scriptableObjectView"
            };
            Add(m_ScriptableObjectView);

            RegisterCallback<DetachFromPanelEvent>(OnDestroy);

        }

        private void OnDestroy(DetachFromPanelEvent e)
        {
            s_Owners.Remove(m_Owner);
            UnregisterCallback<DetachFromPanelEvent>(OnDestroy);
        }

        private void OnValueChanged(ChangeEvent<UnityEngine.Object> changeEvent)
        {
            var newValue = changeEvent.newValue;
            schedule.Execute(() =>
            {
                m_TitleLabel.text = m_SerializedProperty.displayName;
                m_ScriptableObjectView.style.display = newValue ? DisplayStyle.Flex : DisplayStyle.None;
                m_ScriptableObjectView.Clear();
                if (s_Owners.Contains(newValue))
                {
                    m_ObjectField.style.display = DisplayStyle.Flex;
                    return;
                }
                else if (newValue)
                {
                    m_TitleLabel.text = newValue.name;
                    var serializedObject = new SerializedObject(m_SerializedProperty.objectReferenceValue);

                    foreach (var item in GetFields(serializedObject.targetObject.GetType()))
                    {
                        var serializedProperty = serializedObject.FindProperty(item.Name);
                        
                        if (serializedProperty == null) continue;
                        
                        var propertyField = new PropertyField(serializedProperty);
                        propertyField.Bind(serializedObject);
                        m_ScriptableObjectView.Add(propertyField);
                    }
                }
            });
        }

        static List<UnityEngine.Object> s_Owners = new List<UnityEngine.Object>();
        static Dictionary<Type, FieldInfo[]> s_FieldInfoMapCached = new Dictionary<Type, FieldInfo[]>();
        public static FieldInfo[] GetFields(Type type)
        {
            if (s_FieldInfoMapCached.ContainsKey(type))
            {
                return s_FieldInfoMapCached[type];
            }
            else
            {
                FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                s_FieldInfoMapCached.Add(type, fields);
                return fields;
            }
        }
    }

    [CustomPropertyDrawer(typeof(ScriptableObjectFieldAttribute), true)]
    public class ScriptableObjectDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var customAttribute = fieldInfo.GetCustomAttribute<ScriptableObjectFieldAttribute>();
            var scriptableObjectField = new ScriptableObjectField(customAttribute.type, property.propertyPath, property);
            return scriptableObjectField;
        }
    }
}