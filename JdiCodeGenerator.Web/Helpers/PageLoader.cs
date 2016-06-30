namespace JdiCodeGenerator.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Awesomium.Core;
    using HtmlAgilityPack;
    using Core;
    using Core.Helpers;
    using Core.ObjectModel.Abstract;
    using ObjectModel.Abstract;

    //using CefSharp;
    //using CefSharp.OffScreen;

    public class PageLoader
    {
        HtmlNode _docNode;
        //static ChromiumWebBrowser _browser;

        //public PageLoader()
        //{
        //    _browser = null;
        //}

#region this version is for CefSharp
        //void LoadPage(string url)
        //{
        //    var settings = new CefSettings();
        //    // Disable GPU in WPF and Offscreen examples until #1634 has been resolved
        //    settings.CefCommandLineArgs.Add("disable-gpu", "1");

        //    //Perform dependency check to make sure all relevant resources are in our output directory.
        //    Cef.Initialize(settings, shutdownOnProcessExit: true, performDependencyCheck: true);

        //    // Create the offscreen Chromium _browser.
        //    _browser = new ChromiumWebBrowser(url);

        //    // An event that is fired when the first page is finished loading.
        //    // This returns to us from another thread.
        //    // _browser.LoadingStateChanged += BrowserLoadingStateChanged;

        //    // We have to wait for something, otherwise the process will exit too soon.
        //    // Console.ReadKey();
        //    Thread.Sleep(2000);

        //    Thread.Sleep(2000);

        //    //while (!_browser.IsBrowserInitialized) // && _browser.IsLoading)
        //    //{
        //    //    Thread.Sleep(100);
        //    //}
        //    _browser.Stop();

        //    var htmlAsString = GetSourceOfThePage().Result;
        //    // _browser.Stop();

        //    // Clean up Chromium objects.  You need to call this in your application otherwise
        //    // you will get a crash when closing.
        //    Cef.Shutdown();

        //    // var htmlAsString = string.Empty; // GetPageFromAwesomium(url);
        //    var doc = new HtmlDocument();
        //    doc.LoadHtml(htmlAsString);
        //    _docNode = doc.DocumentNode;
        //}

        //async Task<string> GetSourceOfThePage()
        //{
        //    return await _browser.GetSourceAsync();
        //}

        /*
        private static void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            // Check to see if loading is complete - this event is called twice, one when loading starts
            // second time when it's finished
            // (rather than an iframe within the main frame).
            if (!e.IsLoading)
            {
                // Remove the load event handler, because we only want one snapshot of the initial page.
                _browser.LoadingStateChanged -= BrowserLoadingStateChanged;

                var scriptTask = _browser.EvaluateScriptAsync("document.getElementById('lst-ib').value = 'CefSharp Was Here!'");

                scriptTask.ContinueWith(t =>
                {
                    //Give the _browser a little time to render
                    Thread.Sleep(500);
                    // Wait for the screenshot to be taken.
                    var task = _browser.ScreenshotAsync();
                    task.ContinueWith(x =>
                    {
                        // Make a file to save it to (e.g. C:\Users\jan\Desktop\CefSharp screenshot.png)
                        var screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CefSharp screenshot.png");

                        Console.WriteLine();
                        Console.WriteLine("Screenshot ready. Saving to {0}", screenshotPath);

                        // Save the Bitmap to the path.
                        // The image type is auto-detected via the ".png" extension.
                        task.Result.Save(screenshotPath);

                        // We no longer need the Bitmap.
                        // Dispose it to avoid keeping the memory alive.  Especially important in 32-bit applications.
                        task.Result.Dispose();

                        Console.WriteLine("Screenshot saved.  Launching your default image viewer...");

                        // Tell Windows to launch the saved image.
                        Process.Start(screenshotPath);

                        Console.WriteLine("Image viewer launched.  Press any key to exit.");
                    });
                });
            }
        }
        */
        #endregion
        #region classic with HTML Agility Pack
        //void LoadPage(string url)
        //{
        //    var web = new HtmlWeb();
        //    _docNode = web.Load(url).DocumentNode;
        //}
        #endregion
        #region Awesomium
        void LoadPage(string url)
        {
            var htmlAsString = GetPageFromAwesomium(url);
            //var web = new HtmlWeb();
            //_docNode = web.Load(url).DocumentNode;
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlAsString);
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

        public IEnumerable<ICodeEntry<T>> GetCodeEntries<T>(string url, IEnumerable<string> excludeList)
        {
            LoadPage(url);
            return GetCodeEntriesFromNode<T>(_docNode, excludeList);
        }

        internal IEnumerable<ICodeEntry<T>> GetCodeEntriesFromNode<T>(HtmlNode docNode, IEnumerable<string> excludeList)
        {
            var convertor = new HtmlElementToCodeEntryConvertor();

            var rootNode = docNode.Descendants().FirstOrDefault(bodyNode => bodyNode.OriginalName.ToLower() == WebNames.ElementTypeBody);
            if (null == rootNode)
                return new List<ICodeEntry<T>>();

            var codeEntries = convertor.ConvertToCodeEntries(rootNode);

            // experimental
            return
                codeEntries.Where(codeEntry => codeEntry.JdiMemberType != JdiElementTypes.Element)
                    .SetBestChoice()
                    .SetDistinguishNamesForMembers()
                    .Cast<ICodeEntry<T>>();

            // return codeEntries.SetBestChoice().SetDistinguishNamesForMembers();
        }
    }
}