using FastTrackEServices.Implementation;

namespace FastTrackEServices.ServiceResolver;

public class FactoryHelper 
{
    public static IServiceProvider CreateServiceProvider ()
    {
        var host = Host.CreateDefaultBuilder()
        .ConfigureServices(ConfigureServices)
        .Build();

        return host.Services;
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        // services.AddOptions<LabelGenOptions>()
    }
}
