namespace CodeGenerator.Web.Helpers
{
	using System;
	using System.Collections.Generic;
	using Core.ObjectModel.Abstract.Results;

	public interface IPageLoader
	{
		IEnumerable<IPieceOfPackage> GetCodeEntriesFromUrl(string url, IEnumerable<string> excludeList, Type[] analyzers)
			// public IEnumerable<IPieceOfCode> GetCodeEntriesFromUrl<T>(string url, IEnumerable<string> excludeList, Type[] analyzers)
			;

		IEnumerable<IPieceOfPackage> GetCodeEntriesFromPageSource<T>(string pageSource, IEnumerable<string> excludeList, Type[] analyzers)
			// public IEnumerable<IPieceOfCode> GetCodeEntriesFromPageSource<T>(string pageSource, IEnumerable<string> excludeList, Type[] analyzers)
			;
	}
}