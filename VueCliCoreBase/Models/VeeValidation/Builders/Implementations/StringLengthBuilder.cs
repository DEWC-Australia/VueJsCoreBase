using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Model.VeeValidation.Builders;

namespace Models.VeeValidation.Builders.Implementations
{
    public class StringLengthBuilder : ValidationBuilderBase
    {
        public StringLengthBuilder(CustomAttributeData attr, string propName) : base(attr, propName)
        {

        }

        protected override Dictionary<string, dynamic> BuildValidation(CustomAttributeData attr, string propName)
        {
            var max = attr.ConstructorArguments.Where(a => a.ArgumentType.Name == "Int32");

            var min = attr.NamedArguments.Where(a => a.MemberName == "MinimumLength");

            var result = new Dictionary<string, dynamic>();

            if (max.Count() == 1)
            {
                result.Add("max", max.First().Value);

            }

            if (min.Count() == 1)
            {
                result.Add("min", min.First().TypedValue.Value);
            }

            return result;

        }

    }
}
