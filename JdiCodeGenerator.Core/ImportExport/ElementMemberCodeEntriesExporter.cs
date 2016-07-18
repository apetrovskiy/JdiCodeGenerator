namespace JdiCodeGenerator.Core.ImportExport
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Helpers;
    using ObjectModel.Abstract;

    public class ElementMemberCodeEntriesExporter
    {
        public void WriteToFile(IEnumerable<IPageMemberCodeEntry> codeEntries, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                codeEntries.ExportCodeEntriesToJson()
                    .ToList()
                    .ForEach(codeEntryString => writer.WriteLine(codeEntryString));
                writer.Flush();
                writer.Close();
            }
        }
    }
}