namespace CodeGenerator.Core.ObjectModel.Abstract.Results
{
    using System;
    using Enums;

    public interface IPieceOfPackage
    {
        Guid Id { get; set; }
        Guid ParentId { get; set; }
        string GenerateCode(SupportedTargetLanguages targetLanguage);
        PageObjectParts CodeClass { get; set; }
    }
}