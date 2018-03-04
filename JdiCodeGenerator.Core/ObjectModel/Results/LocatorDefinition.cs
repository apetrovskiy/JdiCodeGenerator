namespace CodeGenerator.Core.ObjectModel.Results
{
	using Enums;

	public class LocatorDefinition
    {
        public bool IsBestChoice { get; set; }
        public bool IsUnique { get; set; }
        public FindAnnotationTypes Attribute { get; set; }
        public ElementSearchTypePreferences ElementSearchTypePreference { get; set; }
        public string SearchString { get; set; }
    }
}