using Model.VeeValidation.Builders;
using System.Collections.Generic;
using System.Reflection;

namespace Models.VeeValidation.Builders.Implementations
{
    public class MaxLengthBuilder: ValidationBuilderBase
    {
        public MaxLengthBuilder(CustomAttributeData attr, string propName) : base(attr, propName)
        {

        }

        protected override Dictionary<string, dynamic> BuildValidation(CustomAttributeData attr, string propName)
        {
            int max = 10;
            return new Dictionary<string, dynamic>
            {
                    { "max", max }
            };
        }
    }
}
