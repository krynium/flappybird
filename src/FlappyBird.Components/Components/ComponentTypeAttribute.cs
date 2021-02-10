using System;
using Microsoft.AspNetCore.Components;

namespace FlappyBird.Components.Components
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ComponentTypeAttribute : Attribute
    {
        public Type ComponentType { get; }

        public ComponentTypeAttribute(Type type)
        {
            if (!typeof(ComponentBase).IsAssignableFrom(type))
            {
                throw new ArgumentException("given type should of type ComponentBase or derived");
            }

            ComponentType = type;
        }
    }
}