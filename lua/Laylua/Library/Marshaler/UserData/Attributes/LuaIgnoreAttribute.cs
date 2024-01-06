using System;

namespace Laylua.Library.Marshaler.UserData.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Constructor)]
public class LuaIgnoreAttribute : Attribute
{ }
