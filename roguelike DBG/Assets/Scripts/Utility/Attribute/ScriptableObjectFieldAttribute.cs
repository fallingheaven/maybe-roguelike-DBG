using System;
using UnityEngine;

namespace Utility.Attribute
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ScriptableObjectFieldAttribute : PropertyAttribute
    {
        public readonly Type type;
        public ScriptableObjectFieldAttribute(Type type)
        {
            this.type = type;
        }
    }
}