using Extensions.CamelCase;
using Model.VeeValidation.Builders;
using Models.VeeValidation;
using System.Collections.Generic;
using System.Linq;

namespace Models.ViewModels
{
    public abstract class ViewModelBase
    {

        public Dictionary<string, ClassProperty> Validations { get
            {
                return BuildValidations();
                /*
                    new Dictionary<string, ClassProperty>
                {
                    {
                        "login",
                        new ClassProperty
                        {
                            displayName = "Login",
                            value = null,
                            validations = new Dictionary<string, dynamic>
                            {
                                {"required", true }
                            }

                        }
                    },
                    {
                        "password",
                        new ClassProperty
                        {
                            displayName = "Password",
                            value = null,
                            validations = new Dictionary<string, dynamic>
                            {
                                {"required", true }
                            }

                        }
                    }
                };
            */
            }
        }

        public Dictionary<string, ClassProperty> BuildValidations()
        {
            var retVal = new Dictionary<string, ClassProperty>();

            var type = this.GetType();
           
            var props = type.GetProperties();

            foreach(var prop in props.Where(a => a.Name != "Validations").ToList())
            {
                var propValidations = new Dictionary<string, dynamic>();
                var propDisplayName = prop.Name;

                foreach (var attr in prop.CustomAttributes.ToList())
                {
                    var testDisplayAttr = attr.AttributeType.Name.Equals("DisplayAttribute");
                    var hasName = attr.NamedArguments.Any(a => a.MemberName == "Name");

                    if (testDisplayAttr && hasName)
                    {
                        propDisplayName = attr.NamedArguments.SingleOrDefault(a => a.MemberName == "Name").TypedValue.Value.ToString();
                            continue;
                    }else if (testDisplayAttr)              
                        continue;
                    

                    var validations = ValidationBuilderFactory.GetBuilder(attr, prop.Name);

                    if (validations == null)
                        continue;

                    foreach(var val in validations)
                    {
                        if (propValidations.ContainsKey(val.Key))
                            continue;

                        propValidations.Add(val.Key, val.Value);
                    }

                   
                }

                retVal.Add(prop.Name.ToCamelCase(), new ClassProperty {
                    displayName = propDisplayName,
                    value = null,
                    validations = propValidations
                });

            }

            return retVal;

        }

    }
}
