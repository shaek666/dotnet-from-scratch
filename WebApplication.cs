using System.Reflection;
using http;

class MiniWebApplication(ServiceProvider services)
{
    // public readonly ServiceCollection Services = services;
    public readonly ServiceProvider Services = services;
    private readonly Router _router = new();

    public MiniWebApplication MapGet(string pattern, Func<RequestContext, string> handler)
    {
        _router.MapGet(pattern, handler);
        return this;
    }

    public MiniWebApplication MapPost(string pattern, Func<RequestContext, string> handler)
    {
        _router.MapPost(pattern, handler);
        return this;
    }

    public MiniWebApplication MapControllers()
    {
        var controllerTypes = Services.GetControllerTypes();
        foreach(var controller in controllerTypes)
        {
            var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public);

            foreach(var method in methods)
            {
                var attr = method.GetCustomAttributes<HttpMethodAttribute>().FirstOrDefault();
                if(attr != null)
                {
                    if(attr.Method == "GET")
                    {
                        _router.MapGet(attr.Path, ctx =>
                        {
                            var instance = Activator.CreateInstance(controller);
                            var result = method.Invoke(instance, null);
                            return result?.ToString() ?? "";
                        });
                    }
                    else if(attr.Method == "POST")
                    {
                        _router.MapPost(attr.Path, ctx =>
                        {
                            var instance = Activator.CreateInstance(controller);
                            var result = method.Invoke(instance, null);
                            return result?.ToString() ?? "";
                        });
                    }
                }
            }
        }

        return this;
    }

    public async Task RunAsync(int port = 5000)
    {
        var server = new TcpServer(port, _router);
        await server.StartAsync();
    }
}

class MiniWebApplicationBuilder
{
    public ServiceCollection Services { get; } = new ServiceCollection();

    public MiniWebApplication Build()
    {
        var provider = Services.BuildServiceProvider();
        return new(provider);
    }
}

static class WebApplicationFactory
{
    public static MiniWebApplicationBuilder CreateBuilder()
    {
        return new MiniWebApplicationBuilder();
    }
}