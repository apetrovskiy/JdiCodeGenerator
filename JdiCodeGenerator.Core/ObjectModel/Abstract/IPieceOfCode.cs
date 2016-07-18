﻿namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System;
    using Enums;

    public interface IPieceOfCode
    {
        Guid Id { get; set; }
        Guid ParentId { get; set; }
        string GenerateCode(SupportedLanguages language);
        PiecesOfCodeClasses CodeClass { get; set; }
    }
}