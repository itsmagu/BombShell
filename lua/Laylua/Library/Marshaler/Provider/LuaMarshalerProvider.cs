﻿using Laylua.Library.Marshaler.Provider.Default;

namespace Laylua.Library.Marshaler.Provider;

/// <summary>
///     Represents a type responsible for providing <see cref="LuaMarshaler"/> instances.
/// </summary>
public abstract class LuaMarshalerProvider
{
    /// <summary>
    ///     Gets the default marshaler provider instance.
    /// </summary>
    public static DefaultLuaMarshalerProvider Default { get; } = new();

    /// <summary>
    ///     Creates a marshaler for the specified Lua instance.
    /// </summary>
    /// <param name="lua"> The Lua instance. </param>
    /// <returns>
    ///     The created marshaler.
    /// </returns>
    public abstract LuaMarshaler GetMarshaler(Lua lua);
}
