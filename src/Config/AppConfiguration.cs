using Microsoft.Extensions.Configuration;

namespace PassRegulaParser.Config;

public static class AppConfiguration
{
    private static IConfiguration? _configuration;

    public static IConfiguration Configuration
    {
        get
        {
            _configuration ??= new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            return _configuration;
        }
    }
}
