using System.Reflection;
using http;

public class ServiceCollection
{
    private readonly List<ServiceDescriptor> _services = [];
    // private readonly List<Type> _controllerTypes = [];

    public void AddTransient<TService>()
    {
        var typeOfService = typeof(TService);
        AddTransient(typeOfService, typeOfService);
    }

    public void AddTransient<TService, TImplementation>()
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
    }

    public void AddTransient(Type serviceType, Type implementationType)
    {
        _services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient));
    }

    //-------------------------------------------SIngleton---------------------------------------------------------------------

    public void AddSingleton<TService>()
    {
        var typeOfService = typeof(TService);
        AddSingleton(typeOfService, typeOfService);
    }

    public void AddSingleton(Type serviceType, Type implementationType)
    {
        _services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));
    }

    public void AddSingleton<TService, TImplementation>()
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
    }

    //---------------------------------------------------Scoped-----------------------------------------------------------------------

    public void AddScoped<TService>()
    {
        var typeOfService = typeof(TService);
        AddScoped(typeOfService, typeOfService);
    }

    public void AddScoped(Type serviceType, Type implementationType)
    {
        _services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Scoped));
    }

    public void AddScoped<TService, TImplementation>()
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped));
    }

    //-----------------------------------COntrollers------------------------------------------------------------------------

    public void AddControllers()
    {
        var controllers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && t.Name.EndsWith("Controller"));
        
        // _controllerTypes.AddRange(controllers);
        foreach(var ctrl in controllers)
        {
            AddTransient(ctrl, ctrl);
        }
    }

    // public List<Type> GetControllerTypes() => _controllerTypes;
    public ServiceProvider BuildServiceProvider() => new(_services);
}