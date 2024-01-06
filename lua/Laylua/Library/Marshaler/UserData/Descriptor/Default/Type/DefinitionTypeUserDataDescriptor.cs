using Laylua.Library.Marshaler.UserData.Descriptor.Default.Callback;
using Laylua.Library.Marshaler.UserData.Descriptor.Default.MemberProvider;

namespace Laylua.Library.Marshaler.UserData.Descriptor.Default.Type;

public class DefinitionTypeUserDataDescriptor : TypeUserDataDescriptor
{
    /// <inheritdoc/>
    public override string MetatableName => base.MetatableName + "__definition";

    public DefinitionTypeUserDataDescriptor(System.Type type,
        TypeMemberProvider? memberProvider = null,
        UserDataNamingPolicy? namingPolicy = null,
        CallbackUserDataDescriptorFlags disabledCallbacks = CallbackUserDataDescriptorFlags.None)
        : base(type, true, memberProvider, namingPolicy, disabledCallbacks)
    { }
}
