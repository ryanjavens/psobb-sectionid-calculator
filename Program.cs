using System.CommandLine;
using System.CommandLine.Parsing;

partial class Program
{
    static void Main(string[] args)
    {
        Option<string> name = new("--name", "-n")
        {
            Description = "The name you intend to use for your PSO character.",
        };

        Option<string> characterClass = new("--class", "-c") {
           Description = "The character class you intend to use for your PSO character."
        };

        RootCommand rootCommand = new("PSOBB Section ID Name Calculator");
        rootCommand.Options.Add(name);
        rootCommand.Options.Add(characterClass);

        ParseResult parseResult = rootCommand.Parse(args);
        if(parseResult.Errors.Count == 0 
            && parseResult.GetValue(name) is string parsedName)
        {
            int total;
            if (parseResult.GetValue(characterClass) is string parsedCharacterClass)
            {
                total = CalculateValueFromCharacterClass(parsedCharacterClass);
            }
            else
            {
                total = CalculateValueFromCharacterClass();
            }

            total += CalculateValueFromName(parsedName);

            var sectionId = CalculateSectionIdFromTotal(total);
            Console.WriteLine($"Your section ID is: {sectionId}");
        }

        foreach(ParseError parseError in parseResult.Errors)
        {
            Console.Error.WriteLine(parseError.Message);
        }
    }

    internal static int CalculateValueFromName(string name)
    {
        var total = 0;

        for(var i = 0; i < name.Length; i++)
        {
            var currentChar = name[i];
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
        var isValid = false;
        var selectedClassValue = 0;

        if(!string.IsNullOrEmpty(characterClassInput))
        {
            if(CharacterClassLookup.TryGetValue(characterClassInput.ToLowerInvariant(), out selectedClassValue))
            {
                isValid = true;
            }
        }

        while(!isValid)
        {
            Console.WriteLine("Please input your desired class");
            var newInput = Console.ReadLine();

            if(string.IsNullOrEmpty(newInput)) continue;

            if(!CharacterClassLookup.TryGetValue(newInput.ToLowerInvariant(), out selectedClassValue)) continue;

            return selectedClassValue;
        }

        return selectedClassValue;
    }

    internal static string CalculateSectionIdFromTotal(int total) =>
        (total % 10) switch
        {
            0 => "Viridia",
            1 => "Greenill",
            2 => "Skyly",
            3 => "Bluefull",
            4 => "Purplenum",
            5 => "Pinkal",
            6 => "Redria",
            7 => "Oran",
            8 => "Yellowboze",
            9 => "Whitill",
            _ => "Something went wrong",
        };
}

