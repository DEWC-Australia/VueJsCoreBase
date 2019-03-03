using Models.VeeValidation;
using Models.VeeValidation.Builders.Implementations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Model.VeeValidation.Builders
{
    public static class ValidationBuilderFactory
    {
        public static Dictionary<string, dynamic> GetBuilder(CustomAttributeData attr, string propName)
        {
            switch (attr.AttributeType.Name)
            {
                case "DataTypeAttribute":

                    var type = attr.ConstructorArguments.SingleOrDefault(a => a.ArgumentType.Name == "DataType");

                    if (type == null)
                        return null;

                    switch (type.Value)
                    {
                        case DataType.CreditCard:
                            return new CreditCardBuilder(attr, propName).propertyValidation;
                        case DataType.DateTime:
                            return null;
                        case DataType.EmailAddress:
                            return new EmailBuilder(attr, propName).propertyValidation;
                        case DataType.PhoneNumber:
                            return null;
                        case DataType.Url:
                            return new UrlBuilder(attr, propName).propertyValidation;
                         default:
                            return null;
                    }

                case "CreditCardAttribute":
                    return new CreditCardBuilder(attr, propName).propertyValidation;
                case "RegularExpressionAttribute":
                    return new RegexBuilder(attr, propName).propertyValidation;
                case "UrlAttribute":
                    return new UrlBuilder(attr, propName).propertyValidation;
                case "RequiredAttribute":
                    return new RequiredBuilder(attr, propName).propertyValidation;
                case "RangeAttribute":
                    return new RangeBuilder(attr, propName).propertyValidation;
                case "MaxLengthAttribute":
                    return new MaxLengthBuilder(attr, propName).propertyValidation;
                case "StringLengthAttribute":
                    return new StringLengthBuilder(attr, propName).propertyValidation;
                case "EmailAddressAttribute":
                    return new EmailBuilder(attr, propName).propertyValidation;
                case ValidationDefinitions.INTEGER_ATTRIBUTE:
                    return new IntegerBuilder(attr, propName).propertyValidation;
                case ValidationDefinitions.FLOAT_ATTRIBUTE:
                    return new FloatBuilder(attr, propName).propertyValidation;
                default:
                    return null;

            }
       
        }

    }
}
