namespace JdiCodeGenerator.Web.ExampleRunner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core.ImportExport;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using Core.ObjectModel.Enums;
    using Helpers;
    using ObjectModel.Abstract;
    using ObjectModel.Plugins.BootstrapAndCompetitors;
    using ObjectModel.Plugins.JavaScript;
    using ObjectModel.Plugins.Plain;

    class Program
    {
        static bool fromUrl = true;
        const string PathToExamplePage = @"SharepointSample.txt";

        static void Main(string[] args)
        {
            //var list1 = new List<HtmlElementTypes>();
            //list1.AddRange(new [] { HtmlElementTypes.Html, HtmlElementTypes.A, HtmlElementTypes.Abbr, });


            var pageSource = string.Empty;
            var list = new List<string>
            {
                //// "file:///C:/1/bootstrap.html",
                //"http://localhost:1234/bootstrap/users",
                //"http://localhost:1234/bootstrap/users/user",
                //"http://localhost:1234/bootstrap",
                //// "http://localhost:1234/",


                // "http://www.creative-tim.com/product/buy/bundle", // "http://www.creative-tim.com/", // "http://presentation.creative-tim.com/",
                "http://www.crit-research.it/events/", // "http://www.crit-research.it/",
                //"http://www.blackbox.cool/",
                // "http://indicius.com/",
                // "http://www.spotify-thedrop.com/#/",
                //"https://trakt.tv/",
                //"http://www.jdcdesignstudio.com/",
                //"http://www.europarc-deutschland.de/marktplatz-natur/",
                //"https://www.playosmo.com/en/",
                // "http://www.anakin.co/en",

                //"http://shadowwarrior.com/",
                //"http://calhoun.ca/",
                //"https://onreplaytv.com/",
                //"http://www.empor.cc/",
                // "https://www.mongodb.com/cloud",
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
                // "http://localhost/1/page4.htm"
                //,
                //"http://yuntolovo-spb.ru/",
                //"http://yuntolovo-spb.ru/gallery/building-progress/2-ya-ochered/2016/may/"

            };

            pageSource = LoadPageFromFile();

            var listNotToDisplay = new[] { "html", "head", "body", "#comment", "#text", "meta", "h1", "h2", "h3", "h4", "h5", "h6", "small", "font", "script", "i", "br", "hr", "strong", "style", "title", "img", "noscript" };
            var folderForExportFiles = @"D:\333";
            if (null != args && args.Any() && !string.IsNullOrEmpty(args[0]))
                folderForExportFiles = args[0];
            if (!Directory.Exists(folderForExportFiles))
                Directory.CreateDirectory(folderForExportFiles);
            var loader = new AwesomiumPageLoader();
            var exporter = new ElementMemberCodeEntriesExporter();
            var importer = new ElementMemberCodeEntriesImporter();
            var fileNumber = 0;
            // 20160715
            // TODO: create common collection of code entries and units
            var wholeSiteCollection = new List<IPieceOfPackage>
            {
                // 20160715
                // TODO: create a site code unit
                CodeFile.NewSite("project name")
            };

            list.ForEach(url =>
            {
                Console.WriteLine("===============================================================================");
                Console.WriteLine("================{0}================", url);
                Console.WriteLine("===============================================================================");
                // 20160706
                // var applicableAnalyzers = new[] { typeof(Bootstrap3), typeof(PlainHtml5) };
                // var applicableAnalyzers = new[] { typeof(Bootstrap3), typeof(PlainHtml5), typeof(Jdi) };
                var applicableAnalyzers = new[] { typeof(Bootstrap3), typeof(PlainHtml5), typeof(JqueryBootstrapSelect), typeof(Jdi) };
                // var codeEntries = loader.GetCodeEntriesFromUrl<HtmlElementTypes>(url, listNotToDisplay);
                // 20160715
                // TODO: add code entries and code units to the existing collection
                /*
                var codeEntries = fromUrl
                    ? loader.GetCodeEntriesFromUrl<HtmlElementTypes>(url, listNotToDisplay, applicableAnalyzers)
                    : loader.GetCodeEntriesFromPageSource<HtmlElementTypes>(pageSource, listNotToDisplay, applicableAnalyzers);
                */
                wholeSiteCollection.AddRange(fromUrl
                    ? loader.GetCodeEntriesFromUrl(url, listNotToDisplay, applicableAnalyzers)
                    : loader.GetCodeEntriesFromPageSource<HtmlElementTypes>(pageSource, listNotToDisplay, applicableAnalyzers));

                // 20160715
                // var entries = codeEntries as IList<IPageMemberCodeEntry<HtmlElementTypes>> ?? codeEntries.ToList();
                var entries = wholeSiteCollection.ToList();
                using (var writer = new StreamWriter(folderForExportFiles + @"\" + (300 + fileNumber)))
                {
                    writer.WriteLine(@"// {0}", url);
                    entries.ToList().ForEach(elementDefinition =>
                    {
                        var codeEntryString = elementDefinition.GenerateCode(SupportedLanguages.Java);
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

                // 20160715
                var onlyPageMembers = entries
                    .Where(entry => PiecesOfCodeClasses.PageMember == entry.CodeClass)
                    .Cast<IPageMemberCodeEntry>()
                    .ToList();

                using (var tempWriter = new StreamWriter(folderForExportFiles + @"\" + (400 + fileNumber)))
                {
                    // this worked
                    // entries.ToList().ForEach(eltDef => tempWriter.WriteLine("try {{ elementFound = \"0\"; elementDisplayed = \"0\"; allElementsNumber++; WebElement element = driver.findElement(By.{0}(\"{1}\")); foundElementsNumber++; elementFound = \"1\"; if (element.isDisplayed()) {{ displayedElementsNumber++; elementDisplayed = \"1\"; }} }} catch (Exception e) {{ System.out.println(\"failed: {1}\"); }} writeData(fileWriter, \"{0}\", \"{1}\", elementFound, elementDisplayed);",
                    // 20160715
                    // TODO: work only with code entries
                    // entries.ToList().ForEach(eltDef => tempWriter.WriteLine("testLocator(By.{0}(\"{1}\"), \"{0}\", \"{1}\");",
                    onlyPageMembers
                        .ForEach(eltDef => tempWriter.WriteLine("testLocator(By.{0}(\"{1}\"), \"{0}\", \"{1}\");",
                        // static void testLocator(org.openqa.selenium.By by, String locatorName, String locatorPath) {
                        eltDef.Locators.Any() ? eltDef.Locators.First(loc => loc.IsBestChoice).SearchTypePreference.ToString() : "_",
                        eltDef.Locators.Any() ? eltDef.Locators.First(loc => loc.IsBestChoice).SearchString : "_"
                        ));
                    tempWriter.Flush();
                    tempWriter.Close();
                }

                // 20160715
                // exporter.WriteToFile(entries, folderForExportFiles + @"\" + (100 + ++fileNumber));
                exporter.WriteToFile(onlyPageMembers, folderForExportFiles + @"\" + (100 + ++fileNumber));
                var importedEntries = importer.LoadFromFile<HtmlElementTypes>(folderForExportFiles + @"\" + (100 + fileNumber));
                exporter.WriteToFile(importedEntries, folderForExportFiles + @"\" + (200 + fileNumber));

            });

            Console.WriteLine("Completed!");
            Console.ReadKey();
        }

        static string LoadPageFromFile()
        {
#if DEBUG
            var path = @"..\Debug\Data\" + PathToExamplePage;
#else
            var path = @"..\Release\Data\" + pathToExamplePage;
#endif
            var result = string.Empty;
            using (var reader = new StreamReader(path))
            {
                result = reader.ReadToEnd();
                reader.Close();
            }
            return result;
        }
    }
}
