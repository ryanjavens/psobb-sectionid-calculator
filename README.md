# PSO Section ID Calculator

A small .NET CLI tool that calculates your Phantasy Star Online Blue Burst Section ID from a character name and class, using the game's known hash values for each character and class.

## Usage

```bash
dotnet run --name YourCharacterName --class hucast
```

If `--class` is omitted, you'll be prompted to enter one interactively.

## TODO

- Add [SpectraConsole](https://github.com/spectreconsole/spectre.console) support for nicer console output.
