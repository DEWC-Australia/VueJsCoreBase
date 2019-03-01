using System.Collections.Generic;
using System.Reflection;
using Model.VeeValidation;
using Model.VeeValidation.Builders;


namespace Models.VeeValidation.Builders.Implementations
{
    public class FloatBuilder: ValidationBuilderBase
    {
        public FloatBuilder(CustomAttributeData attr, string propName) : base(attr, propName)
        {

        }

        protected override Dictionary<string, dynamic> BuildValidation(CustomAttributeData attr, string propName)
        {
            return new Dictionary<string, dynamic>
            {
                    { "decimal", 50 }
            };
        }
    }
}
