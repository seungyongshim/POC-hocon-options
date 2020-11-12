namespace HoconOptions.Tests
{
    using System.Runtime.Serialization;
    using FluentAssertions;
    using Hocon.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using Xunit;

    public class HoconOptions
    {
        public PetabridgeImpl Petabridge { get; init; }
        public class PetabridgeImpl
        {
            public CmdImpl Cmd { get; init; }
            
            public class CmdImpl
            {
                public string Host { get; init; }
                public int Port { get; init; }

                public string LogPalettesOnStartup { get; init; }

            }
        }

        public class UnitTest1
        {
            [Fact]
            public void Test1()
            {
                var host = Host.CreateDefaultBuilder()
                               .ConfigureAppConfiguration((context, config) =>
                               {
                                   config.AddHoconFile("appsettings.hocon", optional: false, reloadOnChange: true);
                               })
                               .ConfigureServices((context, services) =>
                               {
                                   var configuration = context.Configuration;

                                   services.AddOptions();
                                   services.Configure<HoconOptions>(configuration);
                               })
                               .Build();

                var configuration = host.Services.GetService<IConfiguration>();

                var hoconOptions = host.Services.GetService<IOptions<HoconOptions>>().Value;

                hoconOptions.Petabridge.Cmd.Host.Should()
                                                .Be("0.0.0.0");
            }
        }
    }
}
