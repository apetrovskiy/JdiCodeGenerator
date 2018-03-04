namespace CodeGenerator.Tests.Helpers
{
	using System.Collections.Generic;
	using System.Linq;
	using Core.Helpers;
	using Core.ObjectModel;
	using Core.ObjectModel.Abstract;
	using Core.ObjectModel.Enums;
	using JdiConverters.ObjectModel.Enums;
	using Xunit;

	public class SettingNamesTests
    {
        List<IPageMemberCodeEntry> _codeEntries;

        public SettingNamesTests()
        {
            _codeEntries = null;
        }

        [Theory]
        [InlineData(new[] { "a", "b", "c" }, "button", new[] { "buttonA", "buttonB", "buttonC" })]
        [InlineData(new[] { "a", "a", "b", "c" }, "button", new[] { "buttonA", "buttonA1", "buttonB", "buttonC" })]
        [InlineData(new[] { "a", "a", "b", "b", "c" }, "button", new[] { "buttonA", "buttonA1", "buttonB", "buttonB1", "buttonC" })]
        [InlineData(new[] { "a", "a", "a", "b", "c" }, "button", new[] { "buttonA", "buttonA1", "buttonA2", "buttonB", "buttonC" })]
        [InlineData(new[] { "a", "b", "c", "a", "c" }, "button", new[] { "buttonA", "buttonB", "buttonC", "buttonA1", "buttonC1" })]
        [InlineData(new[] { "a", "b", "c", "a", "c" }, "combobox", new[] { "comboBoxA", "comboBoxB", "comboBoxC", "comboBoxA1", "comboBoxC1" })]
		[Trait("Category", "SettingNames")]
        public void SetsDistinctNames(string[] originalSequence, string memberTypeName, string[] expectedSequence)
        {
            GivenCodeEntries(memberTypeName, originalSequence);
            WhenCalculatingNames();
            ThenTheResultIs(expectedSequence);
        }

        void GivenCodeEntries(string memberTypeName, string[] originalSequence)
        {
            _codeEntries = new List<IPageMemberCodeEntry>();
            originalSequence.ToList().ForEach(item => _codeEntries.Add(new PageMemberCodeEntry { MemberName = item, MemberType = memberTypeName, JdiMemberType = GetJdiElementType(memberTypeName), Locators = new List<LocatorDefinition> { new LocatorDefinition { IsBestChoice = true, SearchString = item, Attribute = FindTypes.FindBy, SearchTypePreference = SearchTypePreferences.id } } }));
            for (int i = 0; i < originalSequence.Length; i++)
                _codeEntries[i].MemberName = originalSequence[i];
        }

	    JdiElementTypes GetJdiElementType(string memberTypeName)
	    {
		    switch (memberTypeName.ToUpper())
		    {
				case "BUTTON":
					return JdiElementTypes.Button;
				case "COMBOBOX":
					return JdiElementTypes.ComboBox;
				default:
					return JdiElementTypes.Button;
		    }
	    }

        void WhenCalculatingNames()
        {
            _codeEntries.SetDistinguishNamesForMembers();
        }

        void ThenTheResultIs(string[] expectedSequence)
        {
            var expectedNames = expectedSequence.ToList();
            var actualNames = _codeEntries.Select(entry1 => entry1.MemberName).ToList();
            Assert.Equal(expectedNames, actualNames);
        }
    }
}