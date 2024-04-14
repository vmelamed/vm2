namespace TestUtilities;

/// <summary>
/// Class FluentAssertionsExceptionFormatter enables the display of inner exceptions
/// when call.Should().NotThrow() fails. Implements the FluentAssertions.Formatting.IValueFormatter
/// Implements the <see cref="IValueFormatter" />
/// </summary>
/// <seealso cref="IValueFormatter" />
public class FluentAssertionsExceptionFormatter : IValueFormatter
{
    static readonly FluentAssertionsExceptionFormatter formatter = new();

    /// <summary>
    /// Indicates
    /// whether the current <see cref="IValueFormatter" /> can handle the specified <paramref name="value" />.
    /// </summary>
    /// <param name="value">The value for which to create a <see cref="string" />.</param>
    /// <returns><see langword="true" /> if the current <see cref="IValueFormatter" /> can handle the specified value;
    /// otherwise, <see langword="false" />.</returns>
    public bool CanHandle(object value) => value is Exception;

    /// <summary>
    /// Returns a human-readable representation of <paramref name="value" />.
    /// </summary>
    /// <param name="value">The value to format into a human-readable representation</param>
    /// <param name="formattedGraph">An object to write the textual representation to.</param>
    /// <param name="context">Contains additional information that the implementation should take into account.</param>
    /// <param name="formatChild">Allows the formatter to recursively format any child objects.</param>
    /// <remarks>
    /// DO NOT CALL <see cref="Formatting.Formatter.ToString(object,FormattingOptions)" /> directly, but use 
    /// <paramref name="formatChild" /> instead. This will ensure cyclic dependencies are properly detected.
    /// Also, the <see cref="Formatting.FormattedObjectGraph" /> may throw
    /// an <see cref="Formatting.MaxLinesExceededException" /> that must be ignored by implementations of this interface.
    /// </remarks>
    public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
    {
        try
        {
            formattedGraph.AddFragment(((Exception)value).ToString());
        }
        catch (MaxLinesExceededException)
        {
        }
    }

    //
    // Summary:
    //     Enables the display of inner exceptions when call.Should().NotThrow() fails
    public static void EnableDisplayOfInnerExceptions() => Formatter.AddFormatter(formatter);
}
