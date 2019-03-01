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
                return new Dictionary<string, ClassProperty>
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
            }
        }

        public Dictionary<string, ClassProperty> BuildValidations()
        {
            var retVal = new Dictionary<string, ClassProperty>();

            var type = this.GetType();
           
            var props = type.GetProperties();

            foreach(var prop in props.ToList())
            {
                var propValidations = new Dictionary<string, dynamic>();

                foreach(var attr in prop.CustomAttributes)
                {
                    var validation = ValidationBuilderFactory.GetBuilder(attr, prop.Name);
                    if (validation == null)
                        continue;

                    validation.propertyValidation
                        .Select(a => propValidations.Add(a.Key,a.Value));
                    
                }

                retVal.Add(prop.Name.ToLowerInvariant(), new ClassProperty {
                    displayName = prop.Name,
                    value = null,
                    validations = propValidations
                });

            }

            return retVal;

        }
    }
}
