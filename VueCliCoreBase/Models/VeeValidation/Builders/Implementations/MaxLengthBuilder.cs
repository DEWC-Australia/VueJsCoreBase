using Model.VeeValidation.Builders;
using System.Collections.Generic;
using System.Linq;
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
            var number = attr.ConstructorArguments.FirstOrDefault(a => a.ArgumentType.Name == "Int32");
            if (number == null)
                return new Dictionary<string, dynamic>();
            
            return new Dictionary<string, dynamic>
            {
                    { "max", number.Value }
            };
        }
    }
}
