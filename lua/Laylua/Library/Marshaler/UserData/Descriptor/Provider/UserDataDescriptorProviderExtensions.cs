namespace Laylua.Library.Marshaler.UserData.Descriptor.Provider;

public static class UserDataDescriptorProviderExtensions
{
    public static void SetDescriptor<T>(this UserDataDescriptorProvider provider, UserDataDescriptor descriptor)
    {
        provider.SetDescriptor(typeof(T), descriptor);
    }
}
