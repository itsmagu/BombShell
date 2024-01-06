using Laylua.Library.Marshaler.UserData.Descriptor.Provider;
using Laylua.Library.Marshaler.UserData.Descriptor.Provider.Default;

namespace Laylua.Library.Marshaler.Provider.Default;

/// <summary>
///     Represents the default <see cref="LuaMarshalerProvider"/> implementation.
/// </summary>
public class DefaultLuaMarshalerProvider : LuaMarshalerProvider
{
    /// <summary>
    ///     Instantiates a new <see cref="DefaultLuaMarshalerProvider"/>.
    /// </summary>
    public DefaultLuaMarshalerProvider()
    { }

    /// <inheritdoc/>
    public override LuaMarshaler GetMarshaler(Lua lua)
    {
        return new DefaultLuaMarshaler(lua, GetUserDataDescriptorProvider());
    }

    protected virtual UserDataDescriptorProvider GetUserDataDescriptorProvider()
    {
        return new DefaultUserDataDescriptorProvider();
    }
}
