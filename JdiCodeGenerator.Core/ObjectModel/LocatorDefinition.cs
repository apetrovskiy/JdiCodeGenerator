namespace JdiCodeGenerator.Core.ObjectModel
{
    using Enums;

    public class LocatorDefinition
    {
        public bool IsBestChoice { get; set; }
        public bool IsUnique { get; set; }
        public FindTypes Attribute { get; set; }
        public SearchTypePreferences SearchTypePreference { get; set; }
        public string SearchString { get; set; }
    }
}