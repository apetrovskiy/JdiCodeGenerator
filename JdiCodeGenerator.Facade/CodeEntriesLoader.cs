namespace CodeGenerator.Facade
{
	using System.Collections.Generic;
	using Core.ObjectModel.Abstract;

	public class CodeEntriesLoader
    {
        public IEnumerable<IPageMemberCodeEntry> LoadWeb(string[] addresses)
        {
            return Load(addresses);
        }

        //public IEnumerable<IPageMemberCodeEntry<WinElementTypes>> LoadWinUi(string pathToApp)
        //{
        //    return Load<WinElementTypes>(pathToApp);
        //}

        IEnumerable<IPageMemberCodeEntry> Load(string[] addresses)
        {
            var result = new List<IPageMemberCodeEntry>();

            return result;
        }
    }
}