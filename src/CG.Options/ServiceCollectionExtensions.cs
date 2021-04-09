using CG.DataAnnotations;
using CG.Options;
using CG.Validations;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// This class contains extension methods related to the <see cref="IServiceCollection"/>
    /// type.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method attempts to configure the specified options as a 
        /// singleton service. 
        /// </summary>
        /// <typeparam name="TOptions">The type of associated options.</typeparam>
        /// <param name="serviceCollection">The service collection to use for the 
        /// operation.</param>
        /// <param name="dataProtector">The data protector to use for the operation.</param>
        /// <param name="configuration">The configuration to use for the operation.</param>
        /// <returns>True if the options were configured; false otherwise.</returns>
        /// <exception cref="ArgumentException">This exception is thrown whenever
        /// one or more of the required parameters is missing or invalid.</exception>
        public static bool TryConfigureOptions<TOptions>(
            this IServiceCollection serviceCollection,
            IDataProtector dataProtector,
            IConfiguration configuration
            ) where TOptions : class, new()
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
                .ThrowIfNull(dataProtector, nameof(dataProtector))
                .ThrowIfNull(configuration, nameof(configuration));

            // Is the configuration missing or empty?
            if (false == configuration.GetChildren().Any())
            {
                // Return the results.
                return false;
            }

            // Create the options.
            var options = new TOptions();

            // Bind the options to the configuration.
            configuration.Bind(options);

            // Decrypt any protected properties.
            dataProtector.DecryptProperties(
                configuration,
                options
                );

            // Are the options verifiable?
            if (options is OptionsBase)
            {
                // Are the options not valid?
                if (false == (options as OptionsBase).IsValid())
                {
                    // Return the results.
                    return false;
                }
            }

            // Add the options to the DI container.
            serviceCollection.TryAddSingleton<IOptions<TOptions>>(
                new OptionsWrapper<TOptions>(options)
                );

            // Return the results.
            return true;
        }

        // *******************************************************************

        /// <summary>
        /// This method attempts to configure the specified options as a 
        /// singleton service. 
        /// </summary>
        /// <typeparam name="TOptions">The type of associated options.</typeparam>
        /// <param name="serviceCollection">The service collection to use for the 
        /// operation.</param>
        /// <param name="dataProtector">The data protector to use for the operation.</param>
        /// <param name="configuration">The configuration to use for the operation.</param>
        /// <param name="options">The options that were created by the operation.</param>
        /// <returns>True if the options were configured; false otherwise.</returns>
        /// <exception cref="ArgumentException">This exception is thrown whenever
        /// one or more of the required parameters is missing or invalid.</exception>
        public static bool TryConfigureOptions<TOptions>(
            this IServiceCollection serviceCollection,
            IDataProtector dataProtector,
            IConfiguration configuration,
            out TOptions options
            ) where TOptions : class, new()
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
                .ThrowIfNull(dataProtector, nameof(dataProtector))
                .ThrowIfNull(configuration, nameof(configuration));

            // Make the compiler happy.
            options = null;

            // Is the configuration missing or empty?
            if (false == configuration.GetChildren().Any())
            {
                // Return the results.
                return false;
            }

            // Create the options.
            options = new TOptions();

            // Bind the options to the configuration.
            configuration.Bind(options);

            // Decrypt any protected properties.
            dataProtector.DecryptProperties(
                configuration,
                options
                );

            // Are the options verifiable?
            if (options is OptionsBase)
            {
                // Are the options not valid?
                if (false == (options as OptionsBase).IsValid())
                {
                    // Return the results.
                    return false;
                }
            }

            // Add the options to the DI container.
            serviceCollection.TryAddSingleton<IOptions<TOptions>>(
                new OptionsWrapper<TOptions>(options)
                );

            // Return the results.
            return true;
        }

        // *******************************************************************

        /// <summary>
        /// This method attempts to configure the specified options as a 
        /// singleton service. 
        /// </summary>
        /// <typeparam name="TOptions">The type of associated options.</typeparam>
        /// <param name="serviceCollection">The service collection to use for the 
        /// operation.</param>
        /// <param name="options">The options to use for the operation.</param>
        /// <returns>True if the options were configured; false otherwise.</returns>
        /// <exception cref="ArgumentException">This exception is thrown whenever
        /// one or more of the required parameters is missing or invalid.</exception>
        public static bool TryConfigureOptions<TOptions>(
            this IServiceCollection serviceCollection,
            TOptions options
            ) where TOptions : class, new()
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
                .ThrowIfNull(options, nameof(options));

            // Are the options verifiable?
            if (options is OptionsBase)
            {
                // Are the options not valid?
                if (false == (options as OptionsBase).IsValid())
                {
                    // Return the results.
                    return false;
                }
            }

            // Add the options to the DI container.
            serviceCollection.TryAddSingleton<IOptions<TOptions>>(
                new OptionsWrapper<TOptions>(options)
                );

            // Return the results.
            return true;
        }

        // *******************************************************************

        /// <summary>
        /// This method attempts to configure the specified options as a 
        /// singleton service with the specified service collection. The options
        /// are validated and if the results are not valid it throws an exception.
        /// </summary>
        /// <typeparam name="TOptions">The type of associated options.</typeparam>
        /// <param name="serviceCollection">The service collection to use for the 
        /// operation.</param>
        /// <param name="dataProtector">The data protector to use for the operation.</param>
        /// <param name="configuration">The configuration to use for the operation.</param>
        /// <returns>The value of the <paramref name="serviceCollection"/> parameter,
        /// for chaining calls together.</returns>
        /// <exception cref="ArgumentException">This exception is thrown whenever
        /// one or more of the required parameters is missing or invalid.</exception>
        /// <exception cref="ValidationException">This exception is thrown whenever
        /// the <typeparamref name="TOptions"/> object fails to validate properly
        /// after the bind operation.</exception>
        /// <exception cref="OptionsException">This exception is thrown whenever the method
        /// encounters a configuration with no settings.</exception>
        public static IServiceCollection ConfigureOptions<TOptions>(
            this IServiceCollection serviceCollection,
            IDataProtector dataProtector,
            IConfiguration configuration
            ) where TOptions : class, new()
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
                .ThrowIfNull(dataProtector, nameof(dataProtector))
                .ThrowIfNull(configuration, nameof(configuration));

            // Create the options.
            var options = new TOptions();

            // Bind the options to the configuration.
            configuration.Bind(options);

            // Decrypt any protected properties.
            dataProtector.DecryptProperties(
                configuration,
                options
                );

            // Verify the result - if possible.
            (options as OptionsBase)?.ThrowIfInvalid();

            // Add the options to the DI container.
            serviceCollection.TryAddSingleton<IOptions<TOptions>>(
                new OptionsWrapper<TOptions>(options)
                );

            // Return the service collection.
            return serviceCollection;
        }

        // *******************************************************************

        /// <summary>
        /// This method attempts to configure the specified options as a 
        /// singleton service with the specified service collection. The options
        /// are validated and if the results are not valid it throws an exception.
        /// The validated options are returned in the <paramref name="options"/>
        /// parameter.
        /// </summary>
        /// <typeparam name="TOptions">The type of associated options.</typeparam>
        /// <param name="serviceCollection">The service collection to use for the 
        /// operation.</param>
        /// <param name="dataProtector">The data protector to use for the operation.</param>
        /// <param name="configuration">The configuration to use for the operation.</param>
        /// <param name="options">The bound and validated options instance.</param>
        /// <returns>The value of the <paramref name="serviceCollection"/> parameter,
        /// for chaining calls together.</returns>
        /// <exception cref="ArgumentException">This exception is thrown whenever
        /// one or more of the required parameters is missing or invalid.</exception>
        /// <exception cref="ValidationException">This exception is thrown whenever
        /// the <typeparamref name="TOptions"/> object fails to validate properly
        /// after the bind operation.</exception>
        /// <exception cref="OptionsException">This exception is thrown whenever the method
        /// encounters a configuration with no settings.</exception>
        public static IServiceCollection ConfigureOptions<TOptions>(
            this IServiceCollection serviceCollection,
            IDataProtector dataProtector,
            IConfiguration configuration,
            out TOptions options
            ) where TOptions : class, new()
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
                .ThrowIfNull(dataProtector, nameof(dataProtector))
                .ThrowIfNull(configuration, nameof(configuration));

            // Create the options.
            options = new TOptions();

            // Bind the options to the configuration.
            configuration.Bind(options);

            // Decrypt any protected properties.
            dataProtector.DecryptProperties(
                configuration,
                options
                );

            // Verify the result - if possible.
            (options as OptionsBase)?.ThrowIfInvalid();

            // Add the options to the DI container.
            serviceCollection.TryAddSingleton<IOptions<TOptions>>(
                new OptionsWrapper<TOptions>(options)
                );

            // Return the service collection.
            return serviceCollection;
        }

        // *******************************************************************

        /// <summary>
        /// This method attempts to configure the specified options as a 
        /// singleton service with the specified service collection. The options
        /// are validated and if the results are not valid, an exception is thrown.
        /// </summary>
        /// <typeparam name="TOptions">The type of associated options.</typeparam>
        /// <param name="serviceCollection">The service collection to use for the 
        /// operation.</param>
        /// <param name="options">The options to use for the operation.</param>
        /// <returns>The value of the <paramref name="serviceCollection"/> parameter,
        /// for chaining calls together.</returns>
        /// <exception cref="ArgumentException">This exception is thrown whenever
        /// one or more of the required parameters is missing or invalid.</exception>
        /// <exception cref="ValidationException">This exception is thrown whenever
        /// the <typeparamref name="TOptions"/> object fails to validate properly
        /// after the bind operation.</exception>
        public static IServiceCollection ConfigureOptions<TOptions>(
            this IServiceCollection serviceCollection,
            TOptions options
            ) where TOptions : class, new()
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
                .ThrowIfNull(options, nameof(options));

            // Verify the result - if possible.
            (options as OptionsBase)?.ThrowIfInvalid();

            // Add the options to the DI container.
            serviceCollection.TryAddSingleton<IOptions<TOptions>>(
                new OptionsWrapper<TOptions>(options)
                );

            // Return the service collection.
            return serviceCollection;
        }

        #endregion
    }
}
