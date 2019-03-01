using System.Collections.Generic;
using System.Reflection;
using Model.VeeValidation.Builders;

namespace Models.VeeValidation.Builders.Implementations
{
    public class RangeBuilder : ValidationBuilderBase
    {

        public RangeBuilder(CustomAttributeData attr, string propName) : base(attr, propName)
        {

        }

        protected override Dictionary<string, dynamic> BuildValidation(CustomAttributeData attr, string propName)
        {
            var min = 0;
            var max = 2;
            return new Dictionary<string, dynamic>
            {
                    { "lookup",$"{min},{max}"}
            };
        }

    }
}
