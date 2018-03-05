namespace CodeGenerator.Core.ObjectModel.Abstract.Rules
{
    using System.Collections.Generic;
    using Enums;

    public interface IRuleCondition
    {
        NodeRelationships NodeRelationship { get; set; }
        MarkerAttributes MarkerAttribute { get; set; }
        List<string> MarkerValues { get; set; }
    }
}