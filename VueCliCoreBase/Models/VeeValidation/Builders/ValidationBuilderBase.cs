using Models.VeeValidation;
using System.Collections.Generic;
using System.Reflection;

namespace Model.VeeValidation.Builders
{
    public abstract class ValidationBuilderBase
    {
        public Dictionary<string, dynamic> propertyValidation { get; set; }
        public ValidationBuilderBase(CustomAttributeData attr, string propName)
        {
            propertyValidation = BuildValidation(attr, propName);
        }

        protected abstract Dictionary<string, dynamic> BuildValidation(CustomAttributeData attr, string propName);
    }
}
