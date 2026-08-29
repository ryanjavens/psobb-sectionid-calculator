using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed partial class CalculateSectionIdCommand : Command<CalculateSectionIdCommand.Settings>
{
    private const string NameValidationMessage = "Character names must be between 1 and 10 characters.";

    private static bool IsValidNameLength(string name) => name.Length is >= 1 and <= 10;

    internal sealed class Settings : CommandSettings
    {
        [CommandOption("-n|--name <NAME>")]
        [Description("The name you intend to use for your PSO character.")]
        public string? Name { get; set; }

        [CommandOption("-c|--class <CLASS>")]
        [Description("The character class you intend to use for your PSO character.")]
        public string? CharacterClass { get; set; }

        public override ValidationResult Validate() =>
            Name is not null && !IsValidNameLength(Name)
                ? ValidationResult.Error(NameValidationMessage)
                : ValidationResult.Success();
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        AnsiConsole.AlternateScreen(() =>
        {
            var characterClass = ResolveCharacterClass(settings.CharacterClass);
            var name = ResolveName(settings.Name);

            var total = characterClass.Value + CalculateValueFromName(name);
            var sectionId = CalculateSectionIdFromTotal(total);

            AnsiConsole.Clear();
            AnsiConsole.MarkupLine($"Class: {ColorizeClassName(characterClass.Name)}");
            AnsiConsole.MarkupLine($"Name: {name}");
            AnsiConsole.MarkupLine($"Your section ID is: {ColorizeSectionId(sectionId)}");

            AnsiConsole.MarkupLine("[grey]Press any key to exit...[/]");
            Console.ReadKey(intercept: true);
        });

        return 0;
    }

    internal static int CalculateValueFromName(string? name = null)
    {
        name = ResolveName(name);

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

    internal static int CalculateValueFromCharacterClass(string? characterClassInput = null) =>
        ResolveCharacterClass(characterClassInput).Value;

    private static string ResolveName(string? name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            name = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter your desired [green]name[/]:")
                    .Validate(input => IsValidNameLength(input)
                        ? ValidationResult.Success()
                        : ValidationResult.Error($"[red]{NameValidationMessage}[/]")));
        }

        return name;
    }

    private static CharacterClass ResolveCharacterClass(string? characterClassInput)
    {
        if(!string.IsNullOrEmpty(characterClassInput)
            && CharacterClassLookup.TryGetValue(characterClassInput, out var selectedClass))
        {
            return selectedClass;
        }

        var selectedClassName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Please select your desired [green]class[/]:")
                .PageSize(CharacterClassLookup.Count)
                .UseConverter(ColorizeClassName)
                .AddChoices(CharacterClassLookup.Keys));

        return CharacterClassLookup[selectedClassName];
    }

    private static string ColorizeClassName(string className) =>
        $"[{CharacterClassLookup[className].Color}]{className}[/]";

    internal static string ColorizeSectionId(SectionId sectionId) =>
        $"[bold {sectionId.Color}]{sectionId.Name}[/]";

    internal static SectionId CalculateSectionIdFromTotal(int total) =>
        SectionIdLookup[total % 10];
}
