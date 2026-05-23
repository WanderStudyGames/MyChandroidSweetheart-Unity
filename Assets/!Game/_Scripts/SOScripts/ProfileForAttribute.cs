using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ProfileFor : PropertyAttribute
{
    private Type _forType;
    public Type ComponentType => _forType;

    public ProfileFor(Type type)
    {
        _forType = type;
    }
    public static Type GetComponentTypeFromProfile(ComponentProfile profile)
    {
        var profileFor = Attribute.GetCustomAttribute(profile.GetType(), typeof(ProfileFor)) as ProfileFor;
        if (profileFor != null)
        {
            return profileFor.ComponentType;
        }
        return null;
    }
}
