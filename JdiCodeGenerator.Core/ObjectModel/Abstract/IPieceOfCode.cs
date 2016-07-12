namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System;

    public interface IPieceOfCode
    {
        Guid Id { get; set; }
        string GenerateCodeForEntry(SupportedLanguages language);
        Guid DependsOn { get; set; }
    }
}