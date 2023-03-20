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
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{EnvironmentName}.json", true, true);

            ConfigurationRoot = builder.Build();

            TransientFaultHandlingOptions transientFaultOptions = new();
            ConfigurationRoot.GetSection(nameof(TransientFaultHandlingOptions))
                             .Bind(transientFaultOptions);

            // Bind DomainConfiguration
            DomainConfiguration = new DomainConfiguration();
            ConfigurationRoot.Bind(DomainConfiguration);
        }
    }
}