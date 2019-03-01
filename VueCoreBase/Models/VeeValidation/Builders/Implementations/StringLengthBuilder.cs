using System.Collections.Generic;
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
            var max = 5;
            return new Dictionary<string, dynamic>
            {
                    { "max", max}
            };
        }

    }
}
