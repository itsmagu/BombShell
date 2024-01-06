using System;
using System.Diagnostics.CodeAnalysis;

namespace Laylua.Library.Marshaler.UserData.Descriptor.Provider;

public abstract class UserDataDescriptorProvider
{
    public abstract void SetDescriptor(Type type, UserDataDescriptor? descriptor);

    public abstract bool TryGetDescriptor<T>(T obj, [MaybeNullWhen(false)] out UserDataDescriptor descriptor);
}
