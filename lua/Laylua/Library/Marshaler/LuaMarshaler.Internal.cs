using System;
using System.Runtime.CompilerServices;
using Laylua.Library.Entities.Reference;
using Laylua.Library.Marshaler.UserData.Descriptor.Default.Handle;

namespace Laylua.Library.Marshaler;

public abstract partial class LuaMarshaler
{
    internal abstract void RemoveUserDataHandle(UserDataHandle handle);

    internal void ReturnReference(LuaReference reference)
    {
        if (LuaReference.IsAlive(reference))
        {
            _leakedEntities.Push(reference);
        }
        else
        {
            ResetReference(reference);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void ResetReference(LuaReference reference)
    {
        reference.Reset();

        if (_entityPool.Return(reference))
        {
            GC.ReRegisterForFinalize(reference);
        }
    }
}
