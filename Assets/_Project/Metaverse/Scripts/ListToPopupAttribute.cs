using System;
using UnityEngine;

namespace Metaverse
{
    public sealed class ListToPopupAttribute : PropertyAttribute
    {
        public Type Type { get; }
        public string PropertyName { get; }

        public ListToPopupAttribute(Type type, string propertyName)
        {
            Type = type;
            PropertyName = propertyName;
        }
    }
}