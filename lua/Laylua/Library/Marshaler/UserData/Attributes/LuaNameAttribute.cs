using System;

namespace Laylua.Library.Marshaler.UserData.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method)]
public class LuaNameAttribute : Attribute
{
    public string Name { get; }

    public LuaNameAttribute(string name)
    {
        Name = name;
    }
}
