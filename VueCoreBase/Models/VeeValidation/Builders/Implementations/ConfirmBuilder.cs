using Model.VeeValidation.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Models.VeeValidation.Builders.Implementations
{
    public class ConfirmBuilder : ValidationBuilderBase
    {
        public ConfirmBuilder(CustomAttributeData attr, string propName) : base(attr, propName)
        {

        }

        protected override Dictionary<string, dynamic> BuildValidation(CustomAttributeData attr, string propName)
        {
            var name = attr.ConstructorArguments.FirstOrDefault(a => a.ArgumentType.Name == "String");
            if(name != null)
            {
                return new Dictionary<string, dynamic>
                {
                    { "confirmed", name.Value.ToString() }
                };
            }

            return new Dictionary<string, dynamic>();
            
        }
    }
}
