namespace vm2.RegexLib;

/// <summary>
/// Utility class for discovering and printing information about regular expression methods in the RegexLib project.
/// </summary>
public static class RegexLibReflector
{
    /// <summary>
    /// Gets all public regular expression methods marked with GeneratedRegexAttribute in the RegexLib project.
    /// </summary>
    /// <returns>A list of MethodInfo objects representing the generated regex methods.</returns>
    public static IEnumerable<MethodInfo> GetGeneratedRegexMethods()
        => typeof(RegexLibReflector)
            .Assembly
            .GetTypes()
            .Where(t => t.IsPublic && t.Namespace is "vm2.RegexLib")
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static)
                                .Where(m => m.GetCustomAttribute<GeneratedRegexAttribute>() is not null &&
                                            m.ReturnType == typeof(Regex) &&
                                            m.GetParameters().Length is 0))
            ;

    /// <summary>
    /// Prints all public regular expression methods marked with GeneratedRegexAttribute in the RegexLib project.
    /// </summary>
    /// <param name="includeFullSignature">If true, includes the full method signature; otherwise just the method name.</param>
    /// <param name="includeDeclaringType">If true, includes the declaring type name.</param>
    /// <param name="sortByType">If true, sorts by type then method name; otherwise sorts by method name only.</param>
    public static void PrintGeneratedRegexMethods(
        bool includeFullSignature = true,
        bool includeDeclaringType = true,
        bool sortByType = true)
    {
        var regexMethods = GetGeneratedRegexMethods();

        if (sortByType)
            regexMethods = [.. regexMethods
                                .OrderBy(m => m.DeclaringType?.Name)
                                .ThenBy(m => m.Name)];
        else
            regexMethods = [.. regexMethods
                                .OrderBy(m => m.Name)];

        Console.WriteLine($"Found {regexMethods.Count()} public methods marked with [GeneratedRegex] attribute:\n");

        foreach (var method in regexMethods)
            PrintMethodInfo(method, includeFullSignature, includeDeclaringType);
    }

    /// <summary>
    /// Gets all public regular expression methods from a specific class marked with GeneratedRegexAttribute.
    /// </summary>
    /// <param name="className">The name of the class to filter by.</param>
    /// <returns>A list of MethodInfo objects representing the generated regex methods from the specified class.</returns>
    public static IEnumerable<MethodInfo> GetGeneratedRegexMethodsByClass(string className)
        => GetGeneratedRegexMethods()
            .Where(m => m.DeclaringType?.Name == className)
            ;

    /// <summary>
    /// Gets a specific public regular expression method marked with GeneratedRegexAttribute.
    /// </summary>
    /// <param name="methodName">The name of the method to find.</param>
    /// <param name="className">Optional class name to narrow the search.</param>
    /// <returns>A list of MethodInfo objects (could be multiple if method name exists in multiple classes).</returns>
    public static IEnumerable<MethodInfo> GetGeneratedRegexMethodByName(
        string methodName,
        string? className = null)
        => GetGeneratedRegexMethods()
            .Where(m => m.Name.Equals(methodName))
            .Where(m => string.IsNullOrEmpty(className) ||
                        m.DeclaringType?.Name == className)
            ;

    /// <summary>
    /// Gets all available class names that contain generated regex methods.
    /// </summary>
    /// <returns>A list of class names.</returns>
    public static IEnumerable<string> GetAvailableClassNames()
        => GetGeneratedRegexMethods()
            .Select(m => m.DeclaringType?.Name)
            .Where(name => !string.IsNullOrEmpty(name))
            .Cast<string>()
            .Distinct()
            .OrderBy(name => name)
            ;

    /// <summary>
    /// Prints detailed information about all generated regex methods grouped by class.
    /// </summary>
    public static void PrintGeneratedRegexMethodsGrouped()
    {
        var regexMethods = GetGeneratedRegexMethods().ToList();
        var groupedMethods = regexMethods
                                .GroupBy(m => m.DeclaringType)
                                .OrderBy(g => g.Key?.Name)
                                .ToList()
                                ;

        Console.WriteLine($"Found {regexMethods.Count} public methods marked with [GeneratedRegex] attribute in {groupedMethods.Count} classes:\n");

        foreach (var group in groupedMethods)
        {
            var className = group.Key?.Name ?? "Unknown";
            var methods = group.OrderBy(m => m.Name).ToList();

            Console.WriteLine($"📁 {className} ({methods.Count} methods):");

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<GeneratedRegexAttribute>();

                Console.WriteLine($"   • {method.Name}()");
                PrintGeneratedRegexAttribute(attr);
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Prints a summary table of all generated regex methods.
    /// </summary>
    public static void PrintGeneratedRegexSummaryTable()
    {
        static string TrimPattern(string pattern, int maxLength = 100)
        {
            pattern = pattern.Replace(Environment.NewLine, "");
            if (pattern.Length > maxLength)
            {
                pattern = pattern[..(maxLength - 3)];
                pattern += "...";
            }

            return pattern;
        }

        var data = GetGeneratedRegexMethods()
                        .Select(
                            m => new
                            {
                                Class   = m.DeclaringType?.Name ?? "Unknown",
                                Method  = m.Name,
                                Pattern = TrimPattern(m.GetCustomAttribute<GeneratedRegexAttribute>()?.Pattern ?? ""),
                                Options = m.GetCustomAttribute<GeneratedRegexAttribute>()?.Options ?? RegexOptions.None
                            })
                        .OrderBy(x => x.Class)
                        .ThenBy(x => x.Method)
                        .ToList()
                        ;

        // Calculate column widths
        var classWidth   = Math.Max(5, data.Max(x => x.Class.Length) + 2);
        var methodWidth  = Math.Max(6, data.Max(x => x.Method.Length) + 2);
        var patternWidth = Math.Max(7, Math.Min(100, data.Max(x => x.Pattern.Length) + 2));

        // Print header
        Console.WriteLine($"{"Class".PadRight(classWidth)} | {"Method".PadRight(methodWidth)} | {"Pattern".PadRight(patternWidth)} | Options");
        Console.WriteLine(new string('-', classWidth) + "-+-" + new string('-', methodWidth) + "-+-" + new string('-', patternWidth) + "-+-" + new string('-', 20));

        // Print data
        foreach (var item in data)
        {
            var truncatedPattern = item.Pattern.Length > patternWidth - 2
                                        ? item.Pattern[..(patternWidth - 5)] + "..."
                                        : item.Pattern;

            Console.WriteLine($"{item.Class.PadRight(classWidth)} | {item.Method.PadRight(methodWidth)} | {truncatedPattern.PadRight(patternWidth)} | {item.Options}");
        }

        Console.WriteLine($"\nTotal: {data.Count} generated regex methods");
    }

    private static void PrintMethodInfo(
        MethodInfo method,
        bool includeFullSignature,
        bool includeDeclaringType)
    {
        var generatedRegexAttr = method.GetCustomAttribute<GeneratedRegexAttribute>();

        if (includeDeclaringType && method.DeclaringType != null)
            Console.Write($"{method.DeclaringType.Name}.");

        if (includeFullSignature)
            Console.Write($"public static Regex {method.Name}()");
        else
            Console.Write(method.Name);

        if (generatedRegexAttr != null)
        {
            Console.WriteLine();
            PrintGeneratedRegexAttribute(generatedRegexAttr);
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Validates that all generated regex methods can be instantiated without errors.
    /// </summary>
    public static void ValidateGeneratedRegexMethods()
    {
        var regexMethods = GetGeneratedRegexMethods().ToList();
        var errors = new List<string>();
        var success = 0;

        Console.WriteLine($"Validating {regexMethods.Count} generated regex methods...\n");

        foreach (var method in regexMethods)
            try
            {
                var regex = (Regex?)method.Invoke(null, null);

                if (regex is not null)
                {
                    Console.WriteLine($"✅ {method.DeclaringType?.Name}.{method.Name}() - OK");
                    success++;
                }
                else
                {
                    var error = $"❌ {method.DeclaringType?.Name}.{method.Name}() - Returned null";

                    Console.WriteLine(error);
                    errors.Add(error);
                }
            }
            catch (Exception ex)
            {
                var error = $"❌ {method.DeclaringType?.Name}.{method.Name}() - {ex.GetType().Name}: {ex.Message}";
                Console.WriteLine(error);
                errors.Add(error);
            }

        Console.WriteLine($"\nValidation Summary:");
        Console.WriteLine($"✅ Success: {success}");
        Console.WriteLine($"❌ Errors:  {errors.Count}");

        if (errors.Count > 0)
        {
            Console.WriteLine("\nErrors:");
            foreach (var error in errors)
                Console.WriteLine($"  {error}");
        }
    }

    /// <summary>
    /// Prints information about regex methods from a specific class.
    /// </summary>
    /// <param name="className">The class name to filter by.</param>
    public static void PrintGeneratedRegexMethodsByClass(string className)
    {
        var methods = GetGeneratedRegexMethodsByClass(className).ToList();

        if (methods.Count is 0)
        {
            Console.WriteLine($"No generated regex methods found in class '{className}'.");
            Console.WriteLine("\nAvailable classes:");
            foreach (var availableClass in GetAvailableClassNames())
                Console.WriteLine($"  • {availableClass}");

            return;
        }

        Console.WriteLine($"Found {methods.Count} generated regex methods in class '{className}':\n");

        foreach (var method in methods.OrderBy(m => m.Name))
        {
            var attr = method.GetCustomAttribute<GeneratedRegexAttribute>();

            Console.WriteLine($"• {method.Name}()");
            PrintGeneratedRegexAttribute(attr);
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Prints information about a specific regex method.
    /// </summary>
    /// <param name="methodName">The method name to find.</param>
    /// <param name="className">Optional class name to narrow the search.</param>
    public static void PrintGeneratedRegexMethodByName(
        string methodName,
        string? className = null)
    {
        var methods = GetGeneratedRegexMethodByName(methodName, className).ToList();

        if (methods.Count is 0)
        {
            Console.WriteLine($"No generated regex method named '{methodName}' found" +
                             (string.IsNullOrEmpty(className) ? "." : $" in class '{className}'."));
            return;
        }

        if (methods.Count > 1)
            Console.WriteLine($"Found {methods.Count} methods named '{methodName}' in different classes:\n");

        foreach (var method in methods.OrderBy(m => m.DeclaringType?.Name))
        {
            var attr = method.GetCustomAttribute<GeneratedRegexAttribute>();

            Console.WriteLine($"• {method.DeclaringType?.Name}.{method.Name}()");
            if (attr != null)
            {
                PrintGeneratedRegexAttribute(attr);

                // Show a sample test
                try
                {
                    var regex = (Regex?)method.Invoke(null, null);

                    if (regex != null)
                        Console.WriteLine($"  Regex compiled successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ⚠️  Error compiling regex: {ex.Message}");
                }
            }

            Console.WriteLine();
        }
    }

    static void PrintGeneratedRegexAttribute(GeneratedRegexAttribute? attr)
    {
        if (attr is null)
            return;

        if (attr.Options != RegexOptions.None)
            Console.WriteLine($"  Options: {attr.Options}");
        Console.WriteLine($"  Pattern: ==>{attr.Pattern}<==");
    }
}