using Laylua.Library.Marshaler.UserData.Descriptor.Default.Callback;
using Laylua.Library.Marshaler.UserData.Descriptor.Default.MemberProvider;

namespace Laylua.Library.Marshaler.UserData.Descriptor.Default.Type;

public class InstanceTypeUserDataDescriptor : TypeUserDataDescriptor
{
    /// <inheritdoc/>
    public override string MetatableName => base.MetatableName + "__instance";

    public InstanceTypeUserDataDescriptor(System.Type type,
        TypeMemberProvider? memberProvider = null,
        UserDataNamingPolicy? namingPolicy = null,
        CallbackUserDataDescriptorFlags disabledCallbacks = CallbackUserDataDescriptorFlags.None)
        : base(type, false, memberProvider, namingPolicy, disabledCallbacks)
    { }
}
