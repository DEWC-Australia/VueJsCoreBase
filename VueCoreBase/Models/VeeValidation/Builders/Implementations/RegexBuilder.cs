using System.Collections.Generic;
using System.Reflection;
using Model.VeeValidation;
using Model.VeeValidation.Builders;

namespace Models.VeeValidation.Builders.Implementations
{
    public class RegexBuilder : ValidationBuilderBase
    {
        public RegexBuilder(CustomAttributeData attr, string propName) : base(attr, propName)
        {

        }

        protected override Dictionary<string, dynamic> BuildValidation(CustomAttributeData attr, string propName)
        {
            var regexInput = "";
            return new Dictionary<string, dynamic>
            {
                   { "regex", regexInput }
            };
        }

    }
}

