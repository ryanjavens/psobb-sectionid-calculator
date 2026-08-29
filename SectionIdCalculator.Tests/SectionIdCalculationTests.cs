namespace SectionIdCalculator.Tests;

public class SectionIdCalculationTests
{
    [Theory]
    [InlineData("Skysinge", "ramarl", "Greenill")]
    [InlineData("SwitchForc", "fonewm", "Skyly")]
    [InlineData("PSO Lover", "ramar", "Skyly")]
    [InlineData("PSO Lover", "fonewearl", "Oran")]
    public void CalculatesExpectedSectionId(string name, string characterClass, string expectedSectionId)
    {
        var total = CalculateSectionIdCommand.CalculateValueFromCharacterClass(characterClass) + CalculateSectionIdCommand.CalculateValueFromName(name);
        var sectionId = CalculateSectionIdCommand.CalculateSectionIdFromTotal(total);

        Assert.Equal(expectedSectionId, sectionId.Name);
    }

    [Theory]
    [InlineData("RAMARL")]
    [InlineData("RaMaRl")]
    [InlineData("ramarL")]
    public void CharacterClassLookupIsCaseInsensitive(string characterClassInput)
    {
        var value = CalculateSectionIdCommand.CalculateValueFromCharacterClass(characterClassInput);
        var expectedValue = CalculateSectionIdCommand.CalculateValueFromCharacterClass("ramarl");

        Assert.Equal(expectedValue, value);
    }
}
