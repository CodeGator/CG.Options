using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace CG.Options.QuickStart
{
    /// <summary>
    /// This class represents options for this sample.
    /// </summary>
    class MyOptions : OptionsBase
    {
        /// <summary>
        /// This property will throw an exception if missing.
        /// </summary>
        [Required]
        public string A { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Alright, quick and dirty example of how to use this library ...

            // We'll need a configuration to read from ...
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appSettings.json");
            var cfg = builder.Build();

            // We'll need a DI container ...
            var services = new ServiceCollection();

            // This call binds the options to the configuration, validates
            //   the results, then registers the options as a service with the
            //   DI container.
            var result = services.TryConfigureOptions<MyOptions>(
                cfg.GetSection("Bad")
                );

            // TODO : look at the 'result' variable in the debugger, verify that
            //   it contains false, because the required value is missing in the
            //   configuration.

            // Let's try again, this time with the required data in the configuration.
            result = services.TryConfigureOptions<MyOptions>(
                cfg.GetSection("Good")
                );

            // TODO : see? now the return value is true, because the required data
            //   is in the configuration, so the options now validate.


            // Ok, now let's verify that the options are in the DI container ...

            // We'll need the service provider ...
            var provider = services.BuildServiceProvider();

            // Get our options from the DI container.
            var options = provider.GetRequiredService<IOptions<MyOptions>>().Value;

            // TODO : look at 'options' in the debugger, verify that the variable now
            //   contains data from appSettings.json.
        }
    }
}
