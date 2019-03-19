using Models.VeeValidation;
using Models.VeeValidation.Builders.Implementations;
using System;
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

                case "CompareAttribute":
                    return new ConfirmBuilder(attr, propName).propertyValidation;
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
                case "MinLengthAttribute":
                    return new MinLengthBuilder(attr, propName).propertyValidation;
                case "StringLengthAttribute":
                    return new StringLengthBuilder(attr, propName).propertyValidation;
                case "EmailAddressAttribute":
                    return new EmailBuilder(attr, propName).propertyValidation;
                default:
                    return null;

            }
       
        }

    }
}
