namespace JdiCodeGenerator.Core.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;

	public static class NameGenerationExtensions
	{
		static readonly Regex UppercaseTheNextCharacter = new Regex("[^0-9a-zA-Z]+(?<letter>[a-z])", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
		static readonly Regex JustRemoveWrongCharacter = new Regex("[^0-9a-zA-Z]+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
		static readonly Regex TheResourcePartOfUrl = new Regex(@"(?<=https?\:\/\/[^\/]+\/).*(?=\.[^\.]+\r)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		static readonly List<char> WrongCharactersSmallList = new List<char>
		{
			// ' ',
			'-',
			'.',
			// '/',
			'#',
			',',
			// '\\',
			'*',
			':',
			'@',
			// '(',
			// ')',
			// '[',
			// ']',
			'?',
			'=',
			'&',
			'+',
			'%',
			// '\r',
			// '\n'
			';'
		};

		public static string ToPascalCase(this string wronglyFormattedString)
		{
			if (String.IsNullOrEmpty(wronglyFormattedString))
				return ElementMemberCodeEntriesExtensions.NoName;

			var firstCharacter = (int)wronglyFormattedString[0];
			wronglyFormattedString = ((char)firstCharacter).ToString().ToUpper() + wronglyFormattedString.Substring(1);

			wronglyFormattedString = UppercaseTheNextCharacter.Replace(wronglyFormattedString, m => m.ToString().ToUpper());
			wronglyFormattedString = JustRemoveWrongCharacter.Replace(wronglyFormattedString, String.Empty);

			if (String.IsNullOrEmpty(wronglyFormattedString))
				return ElementMemberCodeEntriesExtensions.NoName;

			return wronglyFormattedString;
		}

		public static string CleanUpFromWrongCharacters(this string originalString)
		{
			return string.IsNullOrEmpty(originalString) ? string.Empty : JustRemoveWrongCharacter.Replace(originalString, string.Empty);
		}

		public static string GenerateNameFromUrl(this string url)
		{
			return string.IsNullOrEmpty(url) ? string.Empty : TheResourcePartOfUrl.Match(url).Value;
		}
	}
}