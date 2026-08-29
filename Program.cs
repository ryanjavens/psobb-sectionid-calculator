using Spectre.Console.Cli;

internal class Program
{
    static int Main(string[] args)
    {
        CommandApp<CalculateSectionIdCommand> app = new();
        app.Configure(config => config.SetApplicationName("PSOBB Section ID Name Calculator"));

        return app.Run(args);
    }
}
