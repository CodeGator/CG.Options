using CG.DataAnnotations;
using CG.Options;
using CG.Validations;
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
        /// <param name="configuration">The configuration to use for the operation.</param>
        /// <returns>True if the options were configured; false otherwise.</returns>
        /// <exception cref="ArgumentException">This exception is thrown whenever
        /// one or more of the required parameters is missing or invalid.</exception>
        /// <remarks>
        /// <para>
        /// In addition to registering the <typeparamref name="TOptions"/> object as 
        /// a service, this method also reads the option properties from the configuration,
        /// then decrypts any properties on the <typeparamref name="TOptions"/> object that 
        /// are decorated with a <see cref="ProtectedPropertyAttribute"/> attribute. It also
        /// validates the <typeparamref name="TOptions"/> object after the binding and decryption 
        /// steps are performed. All of this means that after the call to 
        /// <see cref="TryConfigureOptions{TOptions}(IServiceCollection, IConfiguration)"/>
        /// is over, the DI container will contain a singleton <typeparamref name="TOptions"/>
        /// instance whose properties are decrypted and validated.
        /// </para>
        /// </remarks>
        /// <example>
        /// This example demostrates a typical use of the <see cref="TryConfigureOptions{TOptions}(IServiceCollection, IConfiguration)"/>
        /// method:
        /// <code>
        /// public void ConfigureServices(IServiceCollection services)
        /// {
        ///     services.TryConfigureOptions{MyOptions}(Configuration);
        /// }
        /// </code>
        /// </example>
        public static bool TryConfigureOptions<TOptions>(
            this IServiceCollection serviceCollection,
            IConfiguration configuration
            ) where TOptions : class, new()
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
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
            configuration.DecryptProperties(options);

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
        /// <param name="configuration">The configuration to use for the operation.</param>
        /// <param name="options">The options that were created by the operation.</param>
        /// <returns>True if the options were configured; false otherwise.</returns>
        /// <exception cref="ArgumentException">This exception is thrown whenever
        /// one or more of the required parameters is missing or invalid.</exception>
        /// <remarks>
        /// <para>
        /// In addition to registering the <typeparamref name="TOptions"/> object as 
        /// a service, this method also reads the option properties from the configuration,
        /// then decrypts any properties on the <typeparamref name="TOptions"/> object that 
        /// are decorated with a <see cref="ProtectedPropertyAttribute"/> attribute. It 
        /// also validates the <typeparamref name="TOptions"/> object after the binding and 
        /// decryption steps are performed. All of this means that after the call to 
        /// <see cref="TryConfigureOptions{TOptions}(IServiceCollection, IConfiguration)"/>
        /// is over, the DI container will contain a singleton <typeparamref name="TOptions"/>
        /// instance whose properties are decrypted and validated.
        /// </para>
        /// </remarks>
        /// <example>
        /// This example demostrates a typical use of the <see cref="TryConfigureOptions{TOptions}(IServiceCollection, IConfiguration)"/>
        /// method:
        /// <code>
        /// public void ConfigureServices(IServiceCollection services)
        /// {
        ///     services.TryConfigureOptions{MyOptions}(Configuration, out var options);
        /// }
        /// </code>
        /// </example>
        public static bool TryConfigureOptions<TOptions>(
            this IServiceCollection serviceCollection,
            IConfiguration configuration,
            out TOptions options
            ) where TOptions : class, new()
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
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
            configuration.DecryptProperties(options);

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
        /// <remarks>
        /// <para>
        /// This method assumes the caller has configured the incoming properties 
        /// object. It then validates the results and registers the object as a 
        /// singleton service with the DI container.
        /// </para>
        /// </remarks>
        /// <example>
        /// This example demostrates a typical use of the <see cref="TryConfigureOptions{TOptions}(IServiceCollection, TOptions)"/>
        /// method:
        /// <code>
        /// public void ConfigureServices(IServiceCollection services)
        /// {
        ///     var myOptions = new MyOptions();
        ///     // TODO : set the option properties manually here ...
        ///     
        ///     services.TryConfigureOptions(myOptions);
        /// }
        /// </code>
        /// </example>
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
        /// <remarks>
        /// <para>
        /// In addition to registering the <typeparamref name="TOptions"/> object as 
        /// a service, this method also reads the option properties from the configuration,
        /// then decrypts any properties on the <typeparamref name="TOptions"/> object that 
        /// are decorated with a <see cref="ProtectedPropertyAttribute"/> attribute. It also 
        /// validates the <typeparamref name="TOptions"/> object after the binding and decryption 
        /// steps are performed. All of this means that after the call to 
        /// <see cref="ConfigureOptions{TOptions}(IServiceCollection, IConfiguration)"/>
        /// is over, the DI container will contain a singleton <typeparamref name="TOptions"/>
        /// instance whose properties are decrypted and validated.
        /// </para>
        /// </remarks>
        /// <example>
        /// This example demostrates a typical use of the <see cref="ConfigureOptions{TOptions}(IServiceCollection, IConfiguration)"/>
        /// method:
        /// <code>
        /// public void ConfigureServices(IServiceCollection services)
        /// {
        ///     services.ConfigureOptions{MyOptions}(Configuration);
        /// }
        /// </code>
        /// </example>
        public static IServiceCollection ConfigureOptions<TOptions>(
            this IServiceCollection serviceCollection,
            IConfiguration configuration
            ) where TOptions : class, new()
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
                .ThrowIfNull(configuration, nameof(configuration));

            // Create the options.
            var options = new TOptions();

            // Bind the options to the configuration.
            configuration.Bind(options);

            // Decrypt any protected properties.
            configuration.DecryptProperties(options);

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
        /// <remarks>
        /// <para>
        /// In addition to registering the <typeparamref name="TOptions"/> object as 
        /// a service, this method also reads the option properties from the configuration,
        /// then decrypts any properties on the <typeparamref name="TOptions"/> object that 
        /// are decorated with a <see cref="ProtectedPropertyAttribute"/>/ attribute. It also 
        /// validates the <typeparamref name="TOptions"/> object after/ the binding and decryption 
        /// steps are performed. All of this means that after the call to 
        /// <see cref="ConfigureOptions{TOptions}(IServiceCollection, IConfiguration)"/>
        /// is over, the DI container will contain a singleton <typeparamref name="TOptions"/>
        /// instance whose properties are decrypted and validated.
        /// </para>
        /// </remarks>
        /// <example>
        /// This example demostrates a typical use of the <see cref="ConfigureOptions{TOptions}(IServiceCollection, IConfiguration)"/>
        /// method:
        /// <code>
        /// public void ConfigureServices(IServiceCollection services)
        /// {
        ///     services.ConfigureOptions{MyOptions}(Configuration, out var options);
        /// }
        /// </code>
        /// </example>
        public static IServiceCollection ConfigureOptions<TOptions>(
            this IServiceCollection serviceCollection,
            IConfiguration configuration,
            out TOptions options
            ) where TOptions : class, new()
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
                .ThrowIfNull(configuration, nameof(configuration));

            // Create the options.
            options = new TOptions();

            // Bind the options to the configuration.
            configuration.Bind(options);

            // Decrypt any protected properties.
            configuration.DecryptProperties(options);

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
        /// <remarks>
        /// <para>
        /// </para>
        /// </remarks>
        /// <example>
        /// This example demostrates a typical use of the <see cref="ConfigureOptions{TOptions}(IServiceCollection, TOptions)"/>
        /// method:
        /// <code>
        /// public void ConfigureServices(IServiceCollection services)
        /// {
        ///     var myOptions = new MyOptions();
        ///     // TODO : configure the options manually here ...
        ///     
        ///     services.ConfigureOptions(myOptions);
        /// }
        /// </code>
        /// </example>
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
