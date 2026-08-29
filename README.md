# PSO Section ID Calculator

[![.NET](https://github.com/ryanjavens/psobb-sectionid-calculator/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ryanjavens/psobb-sectionid-calculator/actions/workflows/dotnet.yml)

A small .NET CLI tool that calculates your Phantasy Star Online Blue Burst Section ID from a character name and class, using the game's known hash values for each character and class. Console output is powered by [Spectre.Console](https://github.com/spectreconsole/spectre.console).

## Usage

```bash
dotnet run --name YourCharacterName --class hucast
```

If `--name` or `--class` is omitted, you'll be prompted for it interactively — class selection is presented as a color-coded menu you can pick from with the arrow keys.

The result is displayed on an alternate screen, with the class and section ID color-coded to match their in-game colors. Press any key to exit.
