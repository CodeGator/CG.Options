
namespace Microsoft.Extensions.DependencyInjection;

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
    /// This method attempts to configure the specified <typeparamref name="TOptions"/>
    /// object as a singleton service. 
    /// </summary>
    /// <typeparam name="TOptions">The type of associated options.</typeparam>
    /// <typeparam name="TImplementation">The type of associated options interface.</typeparam>
    /// <param name="serviceCollection">The service collection to use for the 
    /// operation.</param>
    /// <param name="configuration">The configuration to use for the operation.</param>
    /// <returns>True if the options were configured; false otherwise.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more of the required parameters is missing or invalid.</exception>
    public static bool TryConfigureOptions<TOptions, TImplementation>(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
        ) where TOptions : class
          where TImplementation : class, TOptions, new()
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
            .ThrowIfNull(configuration, nameof(configuration));

        // Create the options.
        var options = new TImplementation();

        // Bind the options to the configuration.
        configuration.Bind(options);

        // Attempt to validate the options.
        var validationResults = new List<ValidationResult>();
        if (!ValidatorEx.TryValidateObject(
            options,
            new ValidationContext(options),
            validationResults,
            true,
            true
            ))
        {
            // Return the results.
            return false;
        }

        // Add the options to the DI container.
        serviceCollection.TryAddSingleton<
            IOptions<TOptions>,
            IOptions<TImplementation>
            >(
                new OptionsWrapper<TImplementation>(options)
            );

        // Return the results.
        return true;
    }

    // *******************************************************************

    /// <summary>
    /// This method attempts to configure the specified <typeparamref name="TOptions"/>
    /// object as a singleton service. 
    /// </summary>
    /// <typeparam name="TOptions">The type of associated options.</typeparam>
    /// <typeparam name="TImplementation">The type of associated options interface.</typeparam>
    /// <param name="serviceCollection">The service collection to use for the 
    /// operation.</param>
    /// <param name="configuration">The configuration to use for the operation.</param>
    /// <param name="options">The options that were created by the operation.</param>
    /// <returns>True if the options were configured; false otherwise.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more of the required parameters is missing or invalid.</exception>
    public static bool TryConfigureOptions<TOptions, TImplementation>(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        out TImplementation options
        ) where TOptions : class
          where TImplementation : class, TOptions, new()
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
            .ThrowIfNull(configuration, nameof(configuration));

        // Create the options.
        options = new TImplementation();

        // Bind the options to the configuration.
        configuration.Bind(options);

        // Attempt to validate the options.
        var validationResults = new List<ValidationResult>();
        if (!ValidatorEx.TryValidateObject(
            options,
            new ValidationContext(options),
            validationResults,
            true,
            true
            ))
        {
            // Return the results.
            return false;
        }

        // Add the options to the DI container.
        serviceCollection.TryAddSingleton<
            IOptions<TOptions>,
            IOptions<TImplementation>
            >(
                new OptionsWrapper<TImplementation>(options as TImplementation)
            );

        // Return the results.
        return true;
    }

    // *******************************************************************

    /// <summary>
    /// This method attempts to configure the specified <typeparamref name="TOptions"/>
    /// object as a singleton service. 
    /// </summary>
    /// <typeparam name="TOptions">The type of associated options.</typeparam>
    /// <param name="serviceCollection">The service collection to use for the 
    /// operation.</param>
    /// <param name="configuration">The configuration to use for the operation.</param>
    /// <returns>True if the options were configured; false otherwise.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more of the required parameters is missing or invalid.</exception>
    public static bool TryConfigureOptions<TOptions>(
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

        // Attempt to validate the options.
        var validationResults = new List<ValidationResult>();
        if (!ValidatorEx.TryValidateObject(
            options,
            new ValidationContext(options),
            validationResults,
            true,
            true
            ))
        {
            // Return the results.
            return false;
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
    /// This method attempts to configure the specified <typeparamref name="TOptions"/>
    /// object as a singleton service. 
    /// </summary>
    /// <typeparam name="TOptions">The type of associated options.</typeparam>
    /// <param name="serviceCollection">The service collection to use for the 
    /// operation.</param>
    /// <param name="configuration">The configuration to use for the operation.</param>
    /// <param name="options">The options that were created by the operation.</param>
    /// <returns>True if the options were configured; false otherwise.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more of the required parameters is missing or invalid.</exception>
    public static bool TryConfigureOptions<TOptions>(
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

        // Attempt to validate the options.
        var validationResults = new List<ValidationResult>();
        if (!ValidatorEx.TryValidateObject(
            options,
            new ValidationContext(options),
            validationResults,
            true,
            true
            ))
        {
            // Return the results.
            return false;
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
    /// This method attempts to configure the specified <typeparamref name="TOptions"/>
    /// object as a singleton service. 
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

        // Attempt to validate the options.
        var validationResults = new List<ValidationResult>();
        if (!ValidatorEx.TryValidateObject(
            options,
            new ValidationContext(options),
            validationResults,
            true,
            true
            ))
        {
            // Return the results.
            return false;
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
    /// This method attempts to configure the specified <typeparamref name="TOptions"/>
    /// object as a singleton service. 
    /// </summary>
    /// <typeparam name="TOptions">The type of associated options.</typeparam>
    /// <typeparam name="TImplementation">The type of associated options interface.</typeparam>
    /// <param name="serviceCollection">The service collection to use for the 
    /// operation.</param>
    /// <param name="options">The options to use for the operation.</param>
    /// <returns>True if the options were configured; false otherwise.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more of the required parameters is missing or invalid.</exception>
    public static bool TryConfigureOptions<TOptions, TImplementation>(
        this IServiceCollection serviceCollection,
        TImplementation options
        ) where TOptions : class
          where TImplementation : class, TOptions, new()
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
            .ThrowIfNull(options, nameof(options));

        // Attempt to validate the options.
        var validationResults = new List<ValidationResult>();
        if (!ValidatorEx.TryValidateObject(
            options,
            new ValidationContext(options),
            validationResults,
            true,
            true
            ))
        {
            // Return the results.
            return false;
        }

        // Add the options to the DI container.
        serviceCollection.TryAddSingleton<IOptions<TOptions>>(
            new OptionsWrapper<TImplementation>(options)
            );

        // Return the results.
        return true;
    }

    // *******************************************************************

    /// <summary>
    /// This method configures the specified <typeparamref name="TOptions"/>
    /// object as a singleton service. 
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

        // Validate the options.
        Guard.Instance().ThrowIfInvalidObject(options, nameof(options));

        // Add the options to the DI container.
        serviceCollection.TryAddSingleton<IOptions<TOptions>>(
            new OptionsWrapper<TOptions>(options)
            );

        // Return the service collection.
        return serviceCollection;
    }

    // *******************************************************************

    /// <summary>
    /// This method configures the specified <typeparamref name="TOptions"/>
    /// object as a singleton service. 
    /// </summary>
    /// <typeparam name="TOptions">The type of associated options.</typeparam>
    /// <typeparam name="TImplementation">The type of associated options interface.</typeparam>
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
    public static IServiceCollection ConfigureOptions<TOptions, TImplementation>(
        this IServiceCollection serviceCollection,
        IConfiguration configuration
        ) where TOptions : class
          where TImplementation : class, TOptions, new()
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
            .ThrowIfNull(configuration, nameof(configuration));

        // Create the options.
        var options = new TImplementation();

        // Bind the options to the configuration.
        configuration.Bind(options);

        // Validate the options.
        Guard.Instance().ThrowIfInvalidObject(options, nameof(options));

        // Add the options to the DI container.
        serviceCollection.TryAddSingleton<
            IOptions<TOptions>,
            IOptions<TImplementation>
            >(
                new OptionsWrapper<TImplementation>(options)
            );

        // Return the service collection.
        return serviceCollection;
    }

    // *******************************************************************

    /// <summary>
    /// This method configures the specified <typeparamref name="TOptions"/>
    /// object as a singleton service. 
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

        // Validate the options.
        Guard.Instance().ThrowIfInvalidObject(options, nameof(options));

        // Add the options to the DI container.
        serviceCollection.TryAddSingleton<IOptions<TOptions>>(
            new OptionsWrapper<TOptions>(options)
            );

        // Return the service collection.
        return serviceCollection;
    }

    // *******************************************************************

    /// <summary>
    /// This method configures the specified <typeparamref name="TOptions"/>
    /// object as a singleton service. 
    /// parameter.
    /// </summary>
    /// <typeparam name="TOptions">The type of associated options.</typeparam>
    /// <typeparam name="TImplementation">The type of associated options interface.</typeparam>
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
    public static IServiceCollection ConfigureOptions<TOptions, TImplementation>(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        out TOptions options
        ) where TOptions : class
          where TImplementation : class, TOptions, new()
    {
        // Validate the parameters before attempting to use them.
        Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
            .ThrowIfNull(configuration, nameof(configuration));

        // Create the options.
        options = new TImplementation();

        // Bind the options to the configuration.
        configuration.Bind(options);

        // Validate the options.
        Guard.Instance().ThrowIfInvalidObject(options, nameof(options));

        // Add the options to the DI container.
        serviceCollection.TryAddSingleton<
            IOptions<TOptions>,
            IOptions<TImplementation>
            >(
                new OptionsWrapper<TImplementation>(options as TImplementation)
            );

        // Return the service collection.
        return serviceCollection;
    }

    // *******************************************************************

    /// <summary>
    /// This method configures the specified <typeparamref name="TOptions"/>
    /// object as a singleton service. 
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

        // Validate the options.
        Guard.Instance().ThrowIfInvalidObject(options, nameof(options));

        // Add the options to the DI container.
        serviceCollection.TryAddSingleton<IOptions<TOptions>>(
            new OptionsWrapper<TOptions>(options)
            );

        // Return the service collection.
        return serviceCollection;
    }

    #endregion
}
