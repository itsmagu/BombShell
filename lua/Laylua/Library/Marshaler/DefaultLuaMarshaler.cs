using System.Collections.Generic;
using Laylua.Library.Marshaler.UserData.Descriptor;
using Laylua.Library.Marshaler.UserData.Descriptor.Default.Handle;
using Laylua.Library.Marshaler.UserData.Descriptor.Provider;

namespace Laylua.Library.Marshaler;

public partial class DefaultLuaMarshaler : LuaMarshaler
{
    private readonly Dictionary<(object Value, UserDataDescriptor Descriptor), UserDataHandle> _userDataHandleCache;

    public DefaultLuaMarshaler(Lua lua, UserDataDescriptorProvider userDataDescriptorProvider)
        : base(lua, userDataDescriptorProvider)
    {
        _userDataHandleCache = new();
    }

    internal override void RemoveUserDataHandle(UserDataHandle handle)
    {
        if (!handle.TryGetType(out var type) || type.IsValueType || !handle.TryGetValue<object>(out var value))
            return;

        _userDataHandleCache.Remove((value, handle.Descriptor));
    }
}
