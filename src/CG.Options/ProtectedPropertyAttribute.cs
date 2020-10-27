using System;
using System.Security.Cryptography;

namespace CG.Options
{
    /// <summary>
    /// This class represents an encrypted option property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ProtectedPropertyAttribute : Attribute
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains optional entropy bytes.
        /// </summary>
        public byte[] Entropy { get; set; }

        /// <summary>
        /// This property contains optional encryption scope.
        /// </summary>
        public DataProtectionScope? Scope { get; set; }

        #endregion
    }
}
