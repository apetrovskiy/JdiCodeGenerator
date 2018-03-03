namespace JdiCodeGenerator.Web.Helpers
{
	using System.Text.RegularExpressions;

	public static class NameGenerationExtensions
	{
		public static string GenerateNameFromUrl(this string url)
		{
			if (string.IsNullOrEmpty(url)) return string.Empty;
			var pattern = @"(?<=https?\:\/\/[^\/]+\/).*(?=\.[^\.]+\r)";
			return Regex.Match(url, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline).Value;
		}
	}
}