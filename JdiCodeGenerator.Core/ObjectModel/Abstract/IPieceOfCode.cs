namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System;
    using Enums;

    public interface IPieceOfCode
    {
        Guid Id { get; set; }
        string GenerateCodeForEntry(SupportedLanguages language);
    }
}