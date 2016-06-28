namespace JdiCodeGenerator.ExampleRunner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core.ImportExport;
    using Core.ObjectModel.Abstract;
    using JdiCodeGenerator.Web.Helpers;

    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<string>
            {
                //// "file:///C:/1/bootstrap.html",
                //"http://localhost:1234/bootstrap/users",
                //"http://localhost:1234/bootstrap/users/user",
                //"http://localhost:1234/bootstrap",
                //// "http://localhost:1234/",


                //"http://presentation.creative-tim.com/",
                //// "http://www.crit-research.it/",
                //"http://www.blackbox.cool/",
                // "http://indicius.com/",
                //"http://www.spotify-thedrop.com/#/",
                //"https://trakt.tv/",
                //"http://www.jdcdesignstudio.com/",
                //"http://www.europarc-deutschland.de/marktplatz-natur/",
                //"https://www.playosmo.com/en/",
                // "http://www.anakin.co/en",

                //"http://shadowwarrior.com/",
                //"http://calhoun.ca/",
                //"https://onreplaytv.com/",
                //"http://www.empor.cc/",
                "https://www.mongodb.com/cloud",
                //"http://threedaysgrace.com/",
                //"https://www.nasa.gov/",
                //"https://overv.io/",
                //"http://liveramp.com/",
                // "https://www.aceandtate.com/",
                // "http://www.washington.edu/",
                // "http://www.fifa.com/",
                //"http://www.placemeter.com/",
                //"http://www.littlehj.com/",
                //"http://thefounderspledge.org/",

                //// "http://www.folchstudio.com/",
                //"https://maple.com/",
                //"http://www.racefurniture.com.au/",
                //"http://www1.nyc.gov/html/onenyc/",
                //"http://goranfactory.com/",

                //"http://ronagam.com/",
                //"http://www.clusta.com/",
                //"http://thespaces.com/",
                //"http://www.creanet.es/",
                //"https://www.youworkforthem.com/",
                //"http://colofts.ca/",
                //"http://visualsoldiers.com/",
                //"http://scottieandrussell.co.uk/",
                //"http://themewich.com/struck/",
                //"http://olafurarnalds.com/",

                //"http://www.vibrantcomposites.com/",
                //"https://modsquad.com/",
                //"https://www.fromparcel.com/",
                //"http://www.winshipwealth.com/",
                //"http://magnet.co/",
                //"https://webtask.io/",
                //"http://spikenode.com/",
                //"http://guizion.com/",
                //"https://www.glaz-displayschutz.de/"

                //,
                //"http://localhost/1/page4.htm"
                //,
                //"http://yuntolovo-spb.ru/",
                //"http://yuntolovo-spb.ru/gallery/building-progress/2-ya-ochered/2016/may/"

            };
            // var listNotToDisplay = new[] { "html", "head", "body", "#comment", "#text", "div", "meta", "p", "h1", "h2", "h3", "h4", "h5", "h6", "small", "font", "script", "i", "br", "hr", "strong", "style", "title", "li", "ul", "img", "span", "noscript" };
            var listNotToDisplay = new[] { "html", "head", "body", "#comment", "#text", "meta", "h1", "h2", "h3", "h4", "h5", "h6", "small", "font", "script", "i", "br", "hr", "strong", "style", "title", "img", "noscript" };
            // const string FolderForExportFiles = ".";
            var folderForExportFiles = @"D:\333";
            if (null != args && args.Any() && !string.IsNullOrEmpty(args[0]))
                folderForExportFiles = args[0];
            if (!Directory.Exists(folderForExportFiles))
                Directory.CreateDirectory(folderForExportFiles);
            var loader = new PageLoader();
            var exporter = new CodeEntriesExporter();
            var importer = new CodeEntriesImporter();
            var fileNumber = 0;
            list.ForEach(url =>
            {
                Console.WriteLine("===============================================================================");
                Console.WriteLine("================{0}================", url);
                Console.WriteLine("===============================================================================");
                var codeEntries = loader.GetCodeEntries(url, listNotToDisplay);
                var entries = codeEntries as IList<ICodeEntry> ?? codeEntries.ToList();
                using (var writer = new StreamWriter(folderForExportFiles + @"\" + (300 + fileNumber)))
                {
                    writer.WriteLine(@"// {0}", url);
                    entries.ToList().ForEach(elementDefinition =>
                    {
                        var codeEntryString = elementDefinition.GenerateCodeForEntry(SupportedLanguages.Java);
                        Console.WriteLine(codeEntryString);
                        writer.WriteLine(codeEntryString);

                    });
                    writer.Flush();
                    writer.Close();
                }

                /*
        // driver.findElement(By.xpath())
        // driver.findElement(By.className())
        // driver.findElement(By.cssSelector())
        // driver.findElement(By.id())
        // driver.findElement(By.linkText())
        // driver.findElement(By.name())
        // driver.findElement(By.partialLinkText())
        // driver.findElement(By.tagName())
                */
                using (var tempWriter = new StreamWriter(folderForExportFiles + @"\" + (400 + fileNumber)))
                {
                    // tempWriter.WriteLine("tag;css;name;id;class");
                    // displayedElementsNumber++;
                    // entries.ToList().ForEach(eltDef => tempWriter.WriteLine("try {{ allElementsNumber++; WebElement element = driver.findElement(By.{0}(\"{1}\")); foundElementsNumber++; if (element.isDisplayed()) displayedElementsNumber++; }} catch (Exception e) {{ System.out.println(\"failed: {1}\"); }}",
                    // this worked
                    // entries.ToList().ForEach(eltDef => tempWriter.WriteLine("try {{ elementFound = \"0\"; elementDisplayed = \"0\"; allElementsNumber++; WebElement element = driver.findElement(By.{0}(\"{1}\")); foundElementsNumber++; elementFound = \"1\"; if (element.isDisplayed()) {{ displayedElementsNumber++; elementDisplayed = \"1\"; }} }} catch (Exception e) {{ System.out.println(\"failed: {1}\"); }} writeData(fileWriter, \"{0}\", \"{1}\", elementFound, elementDisplayed);",
                    entries.ToList().ForEach(eltDef => tempWriter.WriteLine("testLocator(By.{0}(\"{1}\"), \"{0}\", \"{1}\");",
                        // static void testLocator(org.openqa.selenium.By by, String locatorName, String locatorPath) {
                        eltDef.Locators.Any() ? eltDef.Locators.First(loc => loc.IsBestChoice).SearchTypePreference.ToString() : "_",
                        eltDef.Locators.Any() ? eltDef.Locators.First(loc => loc.IsBestChoice).SearchString : "_"
                        ));
                    tempWriter.Flush();
                    tempWriter.Close();
                }
                /*
                using (var tempWriter = new StreamWriter(folderForExportFiles + @"\" + (400 + fileNumber)))
                {
                    tempWriter.WriteLine("tag;css;name;id;class");
                    entries.ToList().ForEach(eltDef => tempWriter.WriteLine("{0};{1};{2};{3};{4}",
                        eltDef.HtmlMemberType.ToString().ToLower(),
                        eltDef.Locators.Any(l => l.SearchTypePreference == SearchTypePreferences.css) ? eltDef.Locators.FirstOrDefault(l => l.SearchTypePreference == SearchTypePreferences.css).SearchString : string.Empty,
                        eltDef.Locators.Any(l => l.SearchTypePreference == SearchTypePreferences.name) ? eltDef.Locators.FirstOrDefault(l => l.SearchTypePreference == SearchTypePreferences.name).SearchString : string.Empty,
                        eltDef.Locators.Any(l => l.SearchTypePreference == SearchTypePreferences.id) ? eltDef.Locators.FirstOrDefault(l => l.SearchTypePreference == SearchTypePreferences.id).SearchString : string.Empty,
                        eltDef.Locators.Any(l => l.SearchTypePreference == SearchTypePreferences.className) ? eltDef.Locators.FirstOrDefault(l => l.SearchTypePreference == SearchTypePreferences.className).SearchString : string.Empty
                        ));
                    tempWriter.Flush();
                    tempWriter.Close();
                }
                */

                exporter.WriteToFile(entries, folderForExportFiles + @"\" + (100 + ++fileNumber));
                var importedEntries = importer.LoadFromFile(folderForExportFiles + @"\" + (100 + fileNumber));
                exporter.WriteToFile(importedEntries, folderForExportFiles + @"\" + (200 + fileNumber));

            });

            Console.WriteLine("Completed!");
            Console.ReadKey();
        }
    }
}
