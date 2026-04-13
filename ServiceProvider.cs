using System.Dynamic;
using Microsoft.AspNetCore.Http.HttpResults;

public class ServiceProvider
{
    private readonly List<ServiceDescriptor> _services;

    public ServiceProvider(List<ServiceDescriptor> services)
    {
        _services = services;
    }

    public T GetService<T>() => (T)GetService(typeof(T));

    public object GetService(Type serviceType)
    {
        var descriptor = _services.FirstOrDefault(x => x.ServiceType == serviceType)
            ?? throw new Exception($"Service {serviceType.Name} isn't registered");

        return descriptor.LifeTime switch
        {
            ServiceLifetime.Transient => CreateInstance(descriptor.ImplementationType),
            ServiceLifetime.Singleton => GetOrCreateSIngleton(descriptor),
            ServiceLifetime.Scoped => GetOrCreateScoped(descriptor),
            _ => throw new Exception("Unknown lifetime")
        };
    }

    private object CreateInstance(Type implType)
    {
        // var ctor = implType.GetConstructors().First();
        var ctor = implType.GetConstructors();
        var firstConstructor = ctor.First();

        var deps = firstConstructor.GetParameters()
            .Select(p => GetService(p.ParameterType))
            .ToArray();

        return Activator.CreateInstance(implType, deps)!;
    }

    private readonly Dictionary<Type, object> _singletons = [];

    private object GetOrCreateSIngleton(ServiceDescriptor descriptor)
    {
        if(_singletons.TryGetValue(descriptor.ServiceType, out var cached)) return cached;
        var created = CreateInstance(descriptor.ImplementationType);
        _singletons[descriptor.ServiceType] = created;
        return created;
    }

    private readonly Dictionary<Type, object> _scoped = [];

    private object GetOrCreateScoped(ServiceDescriptor descriptor)
    {
        if(_scoped.TryGetValue(descriptor.ServiceType, out var cached)) return cached;
        var created = CreateInstance(descriptor.ImplementationType);
        _scoped[descriptor.ServiceType] = created;
        return created;
    }

    public List<Type> GetControllerTypes()
    {
        return _services
            .Where(d => d.ServiceType.Name.EndsWith("Controller"))
            .Select(d => d.ServiceType)
            .ToList();
    }
}