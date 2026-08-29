using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed partial class CalculateSectionIdCommand : Command<CalculateSectionIdCommand.Settings>
{
    internal sealed class Settings : CommandSettings
    {
        [CommandOption("-n|--name <NAME>")]
        [Description("The name you intend to use for your PSO character.")]
        public string? Name { get; set; }

        [CommandOption("-c|--class <CLASS>")]
        [Description("The character class you intend to use for your PSO character.")]
        public string? CharacterClass { get; set; }

        public override ValidationResult Validate() =>
            string.IsNullOrWhiteSpace(Name)
                ? ValidationResult.Error("Required option '--name' is missing.")
                : ValidationResult.Success();
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        AnsiConsole.AlternateScreen(() =>
        {
            var total = CalculateValueFromCharacterClass(settings.CharacterClass);
            total += CalculateValueFromName(settings.Name!);

            var sectionId = CalculateSectionIdFromTotal(total);
            AnsiConsole.MarkupLine($"Your section ID is: {ColorizeSectionId(sectionId)}");

            AnsiConsole.MarkupLine("[grey]Press any key to exit...[/]");
            Console.ReadKey(intercept: true);
        });

        return 0;
    }

    internal static int CalculateValueFromName(string name)
    {
        var total = 0;

        foreach(var currentChar in name)
        {
            if(!CharLookup.TryGetValue(currentChar, out var charValue))
            {
                throw new ArgumentException($"Invalid character {currentChar}");
            }

            total += charValue;
        }

        return total;
    }

    internal static int CalculateValueFromCharacterClass(string? characterClassInput = null)
    {
        if(!string.IsNullOrEmpty(characterClassInput)
            && CharacterClassLookup.TryGetValue(characterClassInput, out var selectedClass))
        {
            return selectedClass.Value;
        }

        var selectedClassName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Please select your desired [green]class[/]:")
                .PageSize(CharacterClassLookup.Count)
                .UseConverter(ColorizeClassName)
                .AddChoices(CharacterClassLookup.Keys));

        return CharacterClassLookup[selectedClassName].Value;
    }

    private static string ColorizeClassName(string className) =>
        $"[{CharacterClassLookup[className].Color}]{className}[/]";

    internal static string ColorizeSectionId(SectionId sectionId) =>
        $"[bold {sectionId.Color}]{sectionId.Name}[/]";

    internal static SectionId CalculateSectionIdFromTotal(int total) =>
        SectionIdLookup[total % 10];
}
