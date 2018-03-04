namespace CodeGenerator.Core.Helpers
{
	using System.Collections.Generic;
	using System.Linq;
	using Newtonsoft.Json;
	using ObjectModel;
	using ObjectModel.Abstract;

	public static class ImportExportExtensions
    {
        public static IEnumerable<string> ExportCodeEntriesToJson(this IEnumerable<IPageMemberCodeEntry> codeEntries)
        {
            var entries = codeEntries as IPageMemberCodeEntry[] ?? codeEntries.ToArray();
            if (null == codeEntries || !entries.Any())
                return new List<string>();

            return entries.ToList().Select(JsonConvert.SerializeObject);
        }

        public static IEnumerable<IPageMemberCodeEntry> ImportCodeEntriesFromJson<T>(this IEnumerable<string> serializedCodeEntries)
        {
            var entries = serializedCodeEntries as string[] ?? serializedCodeEntries.ToArray();
            if(null == serializedCodeEntries || !entries.Any())
                return new List<IPageMemberCodeEntry>();

            //var settings = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Include,
            //    MissingMemberHandling = MissingMemberHandling.Ignore
            //};
            return entries.ToList().Select(JsonConvert.DeserializeObject<PageMemberCodeEntry>);
        }
    }
}