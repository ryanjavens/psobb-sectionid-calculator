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
        var total = Program.CalculateValueFromCharacterClass(characterClass) + Program.CalculateValueFromName(name);
        var sectionId = Program.CalculateSectionIdFromTotal(total);

        Assert.Equal(expectedSectionId, sectionId);
    }

    [Theory]
    [InlineData("RAMARL")]
    [InlineData("RaMaRl")]
    [InlineData("ramarL")]
    public void CharacterClassLookupIsCaseInsensitive(string characterClassInput)
    {
        var value = Program.CalculateValueFromCharacterClass(characterClassInput);
        var expectedValue = Program.CalculateValueFromCharacterClass("ramarl");

        Assert.Equal(expectedValue, value);
    }
}
