namespace JdiCodeGenerator.Facade
{
    using System.Collections.Generic;
    using Core.ObjectModel.Abstract;
    using Web.ObjectModel.Abstract;

    public class CodeEntriesLoader
    {
        public IEnumerable<IPageMemberCodeEntry<HtmlElementTypes>> LoadWeb(string[] addresses)
        {
            return Load<HtmlElementTypes>(addresses);
        }

        //public IEnumerable<IPageMemberCodeEntry<WinElementTypes>> LoadWinUi(string pathToApp)
        //{
        //    return Load<WinElementTypes>(pathToApp);
        //}

        IEnumerable<IPageMemberCodeEntry<T>> Load<T>(string[] addresses)
        {
            var result = new List<IPageMemberCodeEntry<T>>();

            return result;
        }
    }
}