using System.Collections.Generic;
using System.Linq;
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
            var input = attr.ConstructorArguments.FirstOrDefault(a => a.ArgumentType.Name == "String");
            if (input == null)
                return new Dictionary<string, dynamic>();

            return new Dictionary<string, dynamic>
            {
                   { "regex", input.Value }
            };
        }

    }
}

