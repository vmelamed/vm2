using System.CommandLine;
using vm2.RegexLib;

namespace RegexLibReflectorDemo;

/// <summary>
/// Command-line utility to explore and inspect generated regex methods in the RegexLib project.
/// </summary>
class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("RegexLib Inspector - Explore generated regex methods in the RegexLib project");

        // List all methods command
        var listCommand = new Command("list", "List all generated regex methods")
        {
            new Option<string?>(
                aliases: ["--format", "-f"],
                description: "Output format: table, grouped, simple, or full")
            {
                ArgumentHelpName = "format"
            },
            new Option<bool>(
                aliases: ["--validate", "-v"],
                description: "Validate that all regex methods compile successfully")
        };

        listCommand.SetHandler(async (string? format, bool validate) =>
        {
            await HandleListCommand(format, validate);
        }, listCommand.Options.OfType<Option<string?>>().First(), listCommand.Options.OfType<Option<bool>>().First());

        // Class-specific command
        var classCommand = new Command("class", "Show regex methods from a specific class")
        {
            new Argument<string>("className", "Name of the class to inspect")
        };

        classCommand.SetHandler(async (string className) =>
        {
            await HandleClassCommand(className);
        }, classCommand.Arguments.OfType<Argument<string>>().First());

        // Method-specific command
        var methodCommand = new Command("method", "Show details for a specific regex method")
        {
            new Argument<string>("methodName", "Name of the method to inspect"),
            new Option<string?>(
                aliases: ["--class", "-c"],
                description: "Limit search to specific class")
            {
                ArgumentHelpName = "className"
            }
        };

        methodCommand.SetHandler(async (string methodName, string? className) =>
        {
            await HandleMethodCommand(methodName, className);
        }, methodCommand.Arguments.OfType<Argument<string>>().First(), methodCommand.Options.OfType<Option<string?>>().First());

        // Classes command - list available classes
        var classesCommand = new Command("classes", "List all classes that contain generated regex methods");

        classesCommand.SetHandler(() =>
        {
            HandleClassesCommand();
        });

        // Add commands to root
        rootCommand.AddCommand(listCommand);
        rootCommand.AddCommand(classCommand);
        rootCommand.AddCommand(methodCommand);
        rootCommand.AddCommand(classesCommand);

        // Set default behavior when no command is specified
        rootCommand.SetHandler(() =>
        {
            Console.WriteLine("RegexLib Inspector");
            Console.WriteLine("==================");
            Console.WriteLine();
            Console.WriteLine("Use --help to see available commands.");
            Console.WriteLine();
            Console.WriteLine("Quick start:");
            Console.WriteLine("    dotnet run list           # List all regex methods");
            Console.WriteLine("    dotnet run list -f table  # Show as table");
            Console.WriteLine("    dotnet run classes        # List available classes");
            Console.WriteLine("    dotnet run class Uris     # Show methods from Uris class");
            Console.WriteLine("    dotnet run method Urn     # Show details for Urn method");
            Console.WriteLine();
        });

        return await rootCommand.InvokeAsync(args);
    }

    private static async Task HandleListCommand(string? format, bool validate)
    {
        Console.WriteLine("=== Generated Regex Methods in RegexLib ===\n");

        if (validate)
        {
            Console.WriteLine("Validating all regex methods...\n");
            RegexLibReflector.ValidateGeneratedRegexMethods();
            return;
        }

        format = format?.ToLowerInvariant() ?? "table";

        switch (format)
        {
            case "table":
                RegexLibReflector.PrintGeneratedRegexSummaryTable();
                break;

            case "grouped":
                RegexLibReflector.PrintGeneratedRegexMethodsGrouped();
                break;

            case "simple":
                RegexLibReflector.PrintGeneratedRegexMethods(
                    includeFullSignature: false,
                    includeDeclaringType: true,
                    sortByType: true);
                break;

            case "full":
                RegexLibReflector.PrintGeneratedRegexMethods(
                    includeFullSignature: true,
                    includeDeclaringType: true,
                    sortByType: true);
                break;

            default:
                Console.WriteLine($"Unknown format '{format}'. Available formats: table, grouped, simple, full");
                break;
        }

        await Task.CompletedTask;
    }

    private static async Task HandleClassCommand(string className)
    {
        Console.WriteLine($"=== Regex Methods in '{className}' Class ===\n");
        RegexLibReflector.PrintGeneratedRegexMethodsByClass(className);
        await Task.CompletedTask;
    }

    private static async Task HandleMethodCommand(string methodName, string? className)
    {
        Console.WriteLine($"=== Details for '{methodName}' Method ===\n");
        RegexLibReflector.PrintGeneratedRegexMethodByName(methodName, className);
        await Task.CompletedTask;
    }

    private static void HandleClassesCommand()
    {
        Console.WriteLine("=== Available Classes with Generated Regex Methods ===\n");

        var classes = RegexLibReflector.GetAvailableClassNames();
        var allMethods = RegexLibReflector.GetGeneratedRegexMethods();

        foreach (var className in classes)
        {
            var methodCount = allMethods.Count(m => m.DeclaringType?.Name == className);
            Console.WriteLine($"• {className} ({methodCount} methods)");
        }

        Console.WriteLine($"\nTotal: {classes.Count} classes with {allMethods.Count} generated regex methods");
    }
}
