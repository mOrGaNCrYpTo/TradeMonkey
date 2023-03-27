using Kucoin.Net.Objects;

namespace TradeMonkey.Core
{
    public sealed class TradeMonkeyConfigurationBuilder
    {
        public IConfigurationRoot ConfigurationRoot { get; private set; }

        public DomainConfiguration DomainConfiguration { get; private set; }

        public TradeMonkeyConfigurationBuilder(string EnvironmentName)
        {
            // Create a new instance of ConfigurationBuilder
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            //.AddJsonFile($"appsettings.{EnvironmentName}.json", true, true);

            ConfigurationRoot = builder.Build();

            // Bind DomainConfiguration
            DomainConfiguration = new DomainConfiguration();
            ConfigurationRoot.Bind(DomainConfiguration);

            ConfigurationRoot.GetSection(nameof(TransientFaultHandlingOptions))
                  .Bind(DomainConfiguration.TransientFaultOptions);

            ConfigurationRoot.GetSection(nameof(KucoinApi))
                  .Bind(DomainConfiguration.KucoinApi);

            ConfigurationRoot.GetSection(nameof(TokenMetricsApi))
                  .Bind(DomainConfiguration.TokenMetricsApi);

            ConfigurationRoot.GetSection(nameof(TimerSettings))
                  .Bind(DomainConfiguration.TimerSettings);

            ConfigurationRoot.GetSection(nameof(DatabaseSettings))
                  .Bind(DomainConfiguration.DatabaseSettings);
        }
    }
}