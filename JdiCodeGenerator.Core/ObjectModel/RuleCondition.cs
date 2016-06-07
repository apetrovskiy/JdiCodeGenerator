﻿namespace JdiCodeGenerator.Core.ObjectModel
{
    using System.Collections.Generic;
    using Abstract;

    public class RuleCondition : IRuleCondition
    {
        public Markers Marker { get; set; }
        public List<string> MarkerValues { get; set; }
        public NodeRelationships Relationship { get; set; }

        public RuleCondition()
        {
            MarkerValues = new List<string>();
        }
    }
}
