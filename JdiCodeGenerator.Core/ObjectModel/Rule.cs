﻿namespace JdiCodeGenerator.Core.ObjectModel
{
    using System.Collections.Generic;
    using Abstract;

    public class Rule<T> : IRule<T>
    {
        public IEnumerable<IRuleCondition> OrConditions { get; set; }
        public IEnumerable<IRuleCondition> AndConditions { get; set; }
        public string Description { get; set; }
        public Dictionary<string, IRule<T>> InternalElements { get; set; }
        public List<SourceElementTypeCollection<T>> SourceTypes { get; set; }
        public JdiElementTypes TargetType { get; set; }

        public Rule()
        {
            SourceTypes = new List<SourceElementTypeCollection<T>>();
            OrConditions = new List<IRuleCondition>();
            AndConditions = new List<IRuleCondition>();
            InternalElements = new Dictionary<string, IRule<T>>();
            TargetType = JdiElementTypes.Element;
        }
    }
}
