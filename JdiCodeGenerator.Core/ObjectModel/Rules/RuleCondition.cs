namespace    CodeGenerator.Core.ObjectModel.Rules
{
                using    System.Collections.Generic;
                using    Abstract.Rules;
                using    Enums;

                public    class    RuleCondition    :    IRuleCondition
                {
                                public    MarkerAttributes    MarkerAttribute    {    get;    set;    }
                                public    List<string>    MarkerValues    {    get;    set;    }
                                public    NodeRelationships    NodeRelationship    {    get;    set;    }

                                public    RuleCondition()
                                {
                                                MarkerValues    =    new    List<string>();
                                }
                }
}
