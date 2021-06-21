using System.Collections.Generic;
using UnityEngine;

public static class ComponentExtension
{
    public static T[] ExtGetComponentsInChild <T> (this Component component) where T : Component
    {
        Transform transform = component.transform;

        List<T> components = new List<T>();

        foreach (Transform child in transform)
        {
            T childComponent = child.GetComponent<T>();
            if(childComponent != null)
            {
                components.Add(childComponent);
            }
        }
        return components.ToArray();
    }

    public static T ExtGetComponentInChild<T>(this Component component) where T : Component
    {
        Transform transform = component.transform;

        foreach (Transform child in transform)
        {
            T childComponent = child.GetComponent<T>();
            if (childComponent != null)
            {
                return childComponent;
            }
        }

        return default;
    }
}
