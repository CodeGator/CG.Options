using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;

namespace CG.Options.QuickStart
{
    /// <summary>
    /// This class represents options for this sample.
    /// </summary>
    class MyOptions : OptionsBase
    {
        /// <summary>
        /// This property will automatically decrypt.
        /// </summary>
        [ProtectedProperty]
        public string A { get; set; }

        /// <summary>
        /// This property will throw an exception if missing.
        /// </summary>
        [Required]
        public string B { get; set; }
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

            // This call binds the options to the configuration, decrypts any
            //   properties marked as protected, validates the results, then
            //   registers the options as a service with the DI container.
            var result = services.TryConfigure<MyOptions>(cfg);

            // Let's verify that ...

            // TODO : look at the 'result' variable in the debugger, verify that
            //   it contains true.

            // We'll need the service provider ...
            var provider = services.BuildServiceProvider();

            // Get our options from the DI container.
            var options = provider.GetRequiredService<IOptions<MyOptions>>().Value;

            // TODO : look at 'options' in the debugger, verify that the variable now
            //   contains data from appSettings.json.

            // More things to try ...
            // (1) try encrypting the 'A' property, in options (try CG.Tools.QuickCrypto)
            //   then verify that we still decrypt the value. [Since we assume DPAPI, that
            //   means we can't encrypt on our machine and have it decrypt on yours. You'll
            //   have to encrypt the value on your machine.]
            // (2) try removing the value for the 'B' property, in options, then verify that
            //   the 'TryConfigure' method returns false - due to that missing property, which
            //   we have decorated with the RequiredAttribute.

            // Also, if you aren't using dependency injection, or you otherwise need 
            //   direct access to our underling encryption/decryption extension methods ...

            // We'll need to create the options and bind them to the configuration.
            options = new MyOptions();
            cfg.Bind(options);

            // TODO : look at 'options' in the debugger, verify that the variable contains 
            //   the encrypted data from appSettings.json.

            // Here is how to decrypt the options manually.
            cfg.DecryptProperties<MyOptions>(options);

            // TODO : look at 'options' in the debugger, verify that the variable now
            //   contains (now unencrypted) data from appSettings.json.

            // Note, there is also this option for manually encrypting the options ...
            options.A = "this is a secret";
            cfg.EncryptProperties<MyOptions>(options);

            // TODO : look at 'options' in the debugger, verify that the variable now
            //   contains an encrypted string for the 'A' property.

            // NOTE: when we manually encrypt options using 'EncrypProperties', we only
            //   change the options instace. We don't actually write those changes back
            //   to the original source, which in this case is the appSettings.json file.
            // For that, you're on your own.
        }
    }
}
