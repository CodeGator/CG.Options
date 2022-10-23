
namespace CG.Validations;

/// <summary>
/// This class contains extension methods related to the <see cref="IGuard"/>
/// type.
/// </summary>
public static partial class GuardExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method throws an exception if the <paramref name="argValue"/> 
    /// argument has contains any properties that fail validation checks.
    /// </summary>
    /// <param name="guard">The guard instance to use for the operation.</param>
    /// <param name="argValue">The argument to test.</param>
    /// <param name="argName">The name of the argument.</param>
    /// <param name="memberName">Not used. Supplied by the compiler.</param>
    /// <param name="sourceFilePath">Not used. Supplied by the compiler.</param>
    /// <param name="sourceLineNumber">Not used. Supplied by the compiler.</param>
    /// <returns>The <paramref name="guard"/> argument.</returns>
    /// <exception cref="ArgumentException">This exception is thrown when
    /// the <paramref name="argValue"/> argument contains an object whose
    /// properties fail a validation check.</exception>
    public static void ThrowIfInvalid(
        this IGuard guard,
        object argValue,
        string argName,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0
        )
    {
        // Make the check.
        var validationResults = new List<ValidationResult>();
        var retValue = ValidatorEx.TryValidateObject(
            argValue,
            new ValidationContext(argValue),
            validationResults,
            true,
            true
            );

        // Did we fail?
        if (!retValue)
        {
            // Panic!!!
            throw new ArgumentException(
                message: string.Format(
                    "The argument is invalid!\r\nerrors:{0}\r\n[called from {1} in {2}, line {3}]",
                    string.Join(",", validationResults.Select(x => x.ErrorMessage)),
                    memberName,
                    sourceFilePath,
                    sourceLineNumber
                    ),
                paramName: argName
                );
        }        
    }

    #endregion
}
