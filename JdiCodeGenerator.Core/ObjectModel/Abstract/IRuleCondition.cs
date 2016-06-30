namespace JdiCodeGenerator.Core.ObjectModel.Abstract
{
    using System.Collections.Generic;

    public interface IRuleCondition
    {
        NodeRelationships Relationship { get; set; }
        Markers Marker { get; set; }
        List<string> MarkerValues { get; set; }
    }
}