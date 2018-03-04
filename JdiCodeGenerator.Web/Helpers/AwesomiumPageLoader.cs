namespace CodeGenerator.Web.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using Awesomium.Core;
	using Core.Helpers;
	using Core.ObjectModel;
	using Core.ObjectModel.Abstract;
	using Core.ObjectModel.Enums;
	using HtmlAgilityPack;
	using JdiConverters.ObjectModel.Enums;
	using Web;

	public class AwesomiumPageLoader : IPageLoader
    {
        HtmlNode _docNode;
        List<IPieceOfPackage> _pageCodeEntries;
        Guid _pageGuid;

        public AwesomiumPageLoader()
        {
            _pageCodeEntries = new List<IPieceOfPackage>();
        }




        #region classic with HTML Agility Pack
        //void CreateDocumentNodeByUrl(string url)
        //{
        //    var web = new HtmlWeb();
        //    _docNode = web.Load(url).DocumentNode;
        //}
        #endregion
        #region Awesomium
        void CreateDocumentNodeByUrl(string url)
        {
            var htmlAsString = GetPageFromAwesomium(url);
            CreateDocumentNodeFromSource(htmlAsString);
        }

        void CreateDocumentNodeFromSource(string pageSource)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(pageSource);
            _docNode = doc.DocumentNode;
        }

        string GetPageFromAwesomium(string url)
        {
            var html = string.Empty;
            var finishedLoading = false;

            using (var webView = WebCore.CreateWebView(1920, 10800))
            {
                webView.Source = new Uri(url);
                webView.LoadingFrameComplete += (s, e) =>
                {
                    if (!e.IsMainFrame)
                        return;
                    finishedLoading = true;

                    html = webView.HTML;

                };

                while (!finishedLoading)
                {
                    // Thread.Sleep(20);
                    Thread.Sleep(100);
                    WebCore.Update();
                }
                WebCore.Shutdown();
            }

            return html;
        }
        #endregion

        public IEnumerable<IPieceOfPackage> GetCodeEntriesFromUrl(string url, IEnumerable<string> excludeList, Type[] analyzers)
        {
			_pageCodeEntries.Add(CodeFile.NewPage(url.GenerateNameFromUrl().ToPascalCase()));
			_pageGuid = _pageCodeEntries[0].Id;

            CreateDocumentNodeByUrl(url);
            return GetCodeEntriesFromNode(_docNode, excludeList, analyzers);
        }

        public IEnumerable<IPieceOfPackage> GetCodeEntriesFromPageSource<T>(string pageSource, IEnumerable<string> excludeList, Type[] analyzers)
        {
            // 20160715
            // TODO: create a page unit from the title of the page and add it to the collection
            var titleNode = _docNode.Descendants().Any(node => "title" == node.OriginalName)
                ? _docNode.Descendants().First(node => "title" == node.OriginalName).InnerText
                : "page name".ToPascalCase();
            _pageCodeEntries.Add(CodeFile.NewPage(titleNode));
            _pageGuid = _pageCodeEntries[0].Id;

            CreateDocumentNodeFromSource(pageSource);
            return GetCodeEntriesFromNode(_docNode, excludeList, analyzers);
        }

        internal IEnumerable<IPageMemberCodeEntry> GetCodeEntriesFromNode(HtmlNode docNode, IEnumerable<string> excludeList, Type[] analyzers)
        {
            var convertor = new HtmlElementToElementMemberCodeEntryConvertor(_pageGuid);

            var rootNode = docNode.Descendants().FirstOrDefault(bodyNode => bodyNode.OriginalName.ToLower() == WebNames.ElementTypeBody);
            if (null == rootNode)
                return new List<IPageMemberCodeEntry>();

            _pageCodeEntries.AddRange(convertor.ConvertToCodeEntries(rootNode, analyzers));

            // 20160715
            // TODO: remote the setting of best choice, probably
            // 20160715
            // TODO: set distinguished names for page member entries and element entries separately
            // experimental
            return
                // codeEntries.Where(codeEntry => codeEntry.JdiMemberType != JdiElementTypes.Element)
                // _pageCodeEntries.Cast<PageMemberCodeEntry>().Where(codeEntry => codeEntry.JdiMemberType != JdiElementTypes.Element)
				_pageCodeEntries
					.Where(codeEntry => PageObjectParts.CodeOfMember == codeEntry.CodeClass)
					.Cast<PageMemberCodeEntry>()
					.Where(codeEntry => codeEntry.JdiMemberType != JdiElementTypes.Element)
                    // 20160718
                    // .SetBestChoice()
                    .SetDistinguishNamesForMembers();

            // return codeEntries.SetBestChoice().SetDistinguishNamesForMembers();
        }
    }
}