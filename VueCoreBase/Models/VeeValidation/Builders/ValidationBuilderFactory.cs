using Models.VeeValidation;
using Models.VeeValidation.Builders.Implementations;
using System.Reflection;

namespace Model.VeeValidation.Builders
{
    public static class ValidationBuilderFactory
    {
        public static ValidationBuilderBase GetBuilder(CustomAttributeData attr, string propName)
        {

            if (attr.AttributeType.Name == "RegularExpressionAttribute")
            {
                return new RegexBuilder(attr, propName);
            }
            else if (attr.AttributeType.Name == "UrlAttribute")
            {
                return new UrlBuilder(attr, propName);
            }
            else if (attr.AttributeType.Name == "RequiredAttribute")
            {
                return new RequiredBuilder(attr, propName);
            }
            else if (attr.AttributeType.Name == "RangeAttribute")
            {
                return new RangeBuilder(attr, propName);
            }
            else if (attr.AttributeType.Name == "MaxLengthAttribute")
            {
                return new MaxLengthBuilder(attr, propName);
            }
            else if (attr.AttributeType.Name == "StringLengthAttribute")
            {
                return new StringLengthBuilder(attr, propName);
            }
            else if (attr.AttributeType.Name == "EmailAddressAttribute")
            {
                return new EmailBuilder(attr, propName);
            }
            else if (attr.AttributeType.Name == ValidationDefinitions.INTEGER_ATTRIBUTE)
            {
                return new IntegerBuilder(attr, propName);
            }
            else if (attr.AttributeType.Name == ValidationDefinitions.FLOAT_ATTRIBUTE)
            {
                return new FloatBuilder(attr, propName);
            }

            return null;

        }

    }
}
