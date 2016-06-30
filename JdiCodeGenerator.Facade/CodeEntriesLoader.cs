namespace JdiCodeGenerator.Facade
{
    using System.Collections.Generic;
    using Core.ObjectModel.Abstract;
    using Web.ObjectModel.Abstract;

    public class CodeEntriesLoader
    {
        public IEnumerable<ICodeEntry<HtmlElementTypes>> LoadWeb(string[] addresses)
        {
            return Load<HtmlElementTypes>(addresses);
        }

        //public IEnumerable<ICodeEntry<WinElementTypes>> LoadWinUi(string pathToApp)
        //{
        //    return Load<WinElementTypes>(pathToApp);
        //}

        IEnumerable<ICodeEntry<T>> Load<T>(string[] addresses)
        {
            var result = new List<ICodeEntry<T>>();

            return result;
        }
    }
}