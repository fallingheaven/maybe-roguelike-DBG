using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ScriptableObjectUtility
{
    public static T Clone<T>(T original) where T : ScriptableObject
    {
        if (original == null)
            return null;

        // var clone = ScriptableObject.CreateInstance<T>();
        var clone = ScriptableObject.Instantiate(original);
        CopyFields(original, clone);
        return clone;
    }

    private static void CopyFields(object original, object clone)
    {
        var type = original.GetType();
        while (type != null && type != typeof(ScriptableObject))
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var fieldValue = field.GetValue(original);
                Debug.Log(fieldValue);
                if (fieldValue is ScriptableObject value)
                {
                    var fieldClone = Clone(value);
                    field.SetValue(clone, fieldClone);
                }
                else if (fieldValue is IList<ScriptableObject> list)
                {
                    var listClone = Activator.CreateInstance(fieldValue.GetType()) as IList<ScriptableObject>;
                    foreach (var item in list)
                    {
                        listClone?.Add(Clone(item));
                    }
                    field.SetValue(clone, listClone);
                }
                else if (fieldValue is ScriptableObject[] array)
                {
                    var listClone = Activator.CreateInstance(fieldValue.GetType()) as IList<ScriptableObject>;
                    foreach (var item in array)
                    {
                        listClone?.Add(Clone(item));
                    }

                    var arrayClone = listClone.ToArray();
                    field.SetValue(clone, array);
                }
                else
                {
                    field.SetValue(clone, fieldValue);
                }
            }
            type = type.BaseType;
        }
    }
}