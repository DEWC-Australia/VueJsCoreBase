using Model.VeeValidation.Builders;
using System.Collections.Generic;
using System.Reflection;

namespace Models.VeeValidation.Builders.Implementations
{
    public class UrlBuilder: ValidationBuilderBase
    {
        public UrlBuilder(CustomAttributeData attr, string propName) : base(attr, propName)
        {

        }

        protected override Dictionary<string, dynamic> BuildValidation(CustomAttributeData attr, string propName)
        {
            return new Dictionary<string, dynamic>
            {
                    { "url", new Dictionary<string,bool>(){
                            { "require_protocol", true}
                        }
                    }
            };
        }
    }
}
