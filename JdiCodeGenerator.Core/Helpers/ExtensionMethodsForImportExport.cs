namespace JdiCodeGenerator.Core.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using ObjectModel;
    using ObjectModel.Abstract;

    public static class ExtensionMethodsForImportExport
    {
        public static IEnumerable<string> ExportCodeEntriesToJson<T>(this IEnumerable<ICodeEntry<T>> codeEntries)
        {
            var entries = codeEntries as ICodeEntry<T>[] ?? codeEntries.ToArray();
            if (null == codeEntries || !entries.Any())
                return new List<string>();

            return entries.ToList().Select(JsonConvert.SerializeObject);
        }

        public static IEnumerable<ICodeEntry<T>> ImportCodeEntriesFromJson<T>(this IEnumerable<string> serializedCodeEntries)
        {
            var entries = serializedCodeEntries as string[] ?? serializedCodeEntries.ToArray();
            if(null == serializedCodeEntries || !entries.Any())
                return new List<ICodeEntry<T>>();

            //var settings = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Include,
            //    MissingMemberHandling = MissingMemberHandling.Ignore
            //};
            return entries.ToList().Select(JsonConvert.DeserializeObject<CodeEntry<T>>);
        }
    }
}