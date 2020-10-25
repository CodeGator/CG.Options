using CG.Options;
using CG.Options.Properties;
using CG.Validations;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// This class contains extension methods related to the <see cref="IConfiguration"/>
    /// type.
    /// </summary>
    public static partial class ConfigurationExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method encrypts the value of any properties on the specified 
        /// options object that are decorated with a <see cref="ProtectedPropertyAttribute"/> 
        /// attribute. The underlying configuration itself isn't modified, just 
        /// the options object itself.
        /// </summary>
        /// <typeparam name="T">The type of associated options object.</typeparam>
        /// <param name="configuration">The configuration object to use for the 
        /// operation.</param>
        /// <param name="dataProtectionScope">The data protection scope for the 
        /// operation.</param>
        /// <param name="options">The options object to use for the operation.</param>
        /// <param name="entropy">Optional entropy bytes to use for the operation.</param>
        /// <returns>The value of the <paramref name="configuration"/></returns>
        public static IConfiguration EncryptProperties<T>(
            this IConfiguration configuration,
            T options,
            DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine,
            byte[] entropy = null
            ) where T : class
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(configuration, nameof(configuration))
                .ThrowIfNull(options, nameof(options));

            // Get a list of all object type properties.
            var props = options.GetType().GetProperties()
                .Where(x => x.PropertyType.IsClass && x.PropertyType != typeof(string))
                .ToList();

            // Loop and protect each property, recursively.
            props.ForEach(prop =>
            {
                object obj = null;
                try
                {
                    // Get the object reference.
                    obj = prop.GetGetMethod().Invoke(
                        options,
                        new object[0]
                        );

                    // Check for missing references first ...
                    if (null != obj)
                    {
                        // Protect any properties for the object.
                        configuration.EncryptProperties(
                            obj,
                            dataProtectionScope,
                            entropy
                            );
                    }                    
                }
                catch (Exception ex)
                {
                    // Wrap the exception.
                    throw new InvalidOperationException(
                        message: string.Format(
                            Resources.ConfigurationExtensions_EncryptProperties,
                            prop.Name,
                            obj.GetType().Name
                            ),
                        innerException: ex
                        );
                }
            });

            // Get a list of all the read/write properties of type: string.
            props = options.GetType().GetProperties()
                .Where(x => x.CanRead && x.CanWrite && x.PropertyType == typeof(string))
                .ToList();

            // Loop and check each writeable property of type: string.
            props.ForEach(prop =>
            {
                try
                {
                    // Look for a custom attribute on the property.
                    var attr = prop.GetCustomAttributes(
                        true
                        ).OfType<ProtectedPropertyAttribute>();

                    // Did we find one?
                    if (null != attr && attr.Any())
                    {
                        // If we get here then we should try to protect the value
                        //   of the property.
                        var unprotectedPropertyValue = prop.GetGetMethod().Invoke(
                            options, new object[0]
                            ) as string;

                        // Check for empty strings first ...
                        if (false == string.IsNullOrEmpty(unprotectedPropertyValue))
                        {
                            // Convert the unencrypted value to bytes.
                            var unprotectedBytes = Encoding.UTF8.GetBytes(
                                unprotectedPropertyValue
                                );

                            // Should we supply default entropy?
                            if (null == entropy || entropy.Length == 0)
                            {
                                entropy = new byte[] { 12, 48, 8, 20 };
                            }

                            // Protect the bytes.
                            var protectedBytes = ProtectedData.Protect(
                                unprotectedBytes,
                                entropy,
                                dataProtectionScope
                                );

                            // Convert the bytes back to a string.
                            var protectedPropertyValue = Convert.ToBase64String(
                                protectedBytes
                                );

                            // Write the protected/encoded string to the original property.
                            prop.GetSetMethod().Invoke(
                                options,
                                new[] { protectedPropertyValue }
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Wrap the exception.
                    throw new InvalidOperationException(
                        message: string.Format(
                            Resources.ConfigurationExtensions_EncryptProperties,
                            prop.Name,
                            options.GetType().Name
                        ),
                        innerException: ex
                    );
                }
            });

            // Return the configuration.
            return configuration;
        }

        // *******************************************************************

        /// <summary>
        /// This method decrypts the value of any properties on the specified 
        /// options object that are decorated with a <see cref="ProtectedPropertyAttribute"/> 
        /// attribute. The underlying configuration isn't modified, just the 
        /// options object itself.
        /// </summary>
        /// <typeparam name="T">The type of associated options object.</typeparam>
        /// <param name="configuration">The configuration object to use for 
        /// the operation.</param>
        /// <param name="options">The options object to use for the operation.</param>
        /// <param name="dataProtectionScope">The data protection scope for the 
        /// operation.</param>
        /// <param name="entropy">Optional entropy bytes to use for the operation.</param>
        /// <returns>A new instance of <typeparamref name="T"/> if successful; 
        /// default(T) otherwise.</returns>
        public static IConfiguration DecryptProperties<T>(
            this IConfiguration configuration,
            T options,
            DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine,
            byte[] entropy = null
            ) where T : class
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(configuration, nameof(configuration))
                .ThrowIfNull(options, nameof(options));

            // Get a list of all object type properties.
            var props = options.GetType().GetProperties()
                .Where(x => x.PropertyType.IsClass && x.PropertyType != typeof(string))
                .ToList();

            // Loop and unprotect each property, recursively.
            props.ForEach(prop =>
            {
                object obj = null;
                try
                {
                    // Get the object reference.
                    obj = prop.GetGetMethod().Invoke(
                        options,
                        new object[0]
                        );

                    // Check for missing references first ...
                    if (null != obj)
                    {
                        // Unprotect any properties for the object.
                        configuration.DecryptProperties(
                            obj,
                            dataProtectionScope,
                            entropy
                            );
                    }
                }
                catch (Exception ex)
                {
                    // Wrap the exception.
                    throw new InvalidOperationException(
                        message: string.Format(
                            Resources.ConfigurationExtensions_DecryptProperties,
                            prop.Name,
                            obj.GetType().Name
                            ),
                        innerException: ex
                        );
                }
            });

            // Get a list of all the read/write properties of type: string.
            props = options.GetType().GetProperties()
                .Where(x => x.CanRead && x.CanWrite && x.PropertyType == typeof(string))
                .ToList();

            // Loop and unprotect each property.
            props.ForEach(prop =>
            {
                try
                {
                    // Look for a custom attribute on the property.
                    var attr = prop.GetCustomAttributes(
                        true
                        ).OfType<ProtectedPropertyAttribute>();

                    // Did we find one?
                    if (null != attr && attr.Any())
                    {
                        // If we get here then we should try to unprotect the value
                        //   of the property.
                        var encryptedPropertyValue = prop.GetGetMethod().Invoke(
                            options, new object[0]
                            ) as string;

                        // Check for empty strings first ...
                        if (!string.IsNullOrEmpty(encryptedPropertyValue))
                        {
                            // Convert the encrypted value to bytes.
                            var encryptedBytes = Convert.FromBase64String(
                                encryptedPropertyValue
                                );

                            // Should we supply default entropy?
                            if (null == entropy || entropy.Length == 0)
                            {
                                entropy = new byte[] { 12, 48, 8, 20 };
                            }

                            // Unprotect the bytes.
                            var unprotectedBytes = ProtectedData.Unprotect(
                                encryptedBytes,
                                entropy,
                                dataProtectionScope
                                );

                            // Convert the bytes back to a (non-encoded) string.
                            var unprotectedPropertyValue = Encoding.UTF8.GetString(
                                unprotectedBytes
                                );

                            // Write the unprotected string to the original property.
                            prop.GetSetMethod().Invoke(
                                options,
                                new[] { unprotectedPropertyValue }
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Wrap the exception.
                    throw new InvalidOperationException(
                        message: string.Format(
                            Resources.ConfigurationExtensions_DecryptProperties,
                            prop.Name,
                            options.GetType().Name
                            ),
                        innerException: ex
                        );
                }
            });

            // Return the configuration.
            return configuration;
        }

        #endregion
    }
}
