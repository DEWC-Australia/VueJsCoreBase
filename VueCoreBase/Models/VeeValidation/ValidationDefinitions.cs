using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.VeeValidation
{
    public class ValidationDefinitions
    {
        public const string INTERGER_REGEX = @"([-+]?[0-9\b]+$)";
        public const string FLOAT_REGEX = "([+-]?([0-9]*[.])?[0-9]+)";
        public const string TIME_REGEX = "([01][0-9]|2[0-3]):([0-5][0-9])$";
        public const string DATE_REGEX = "(3[01]|[12][0-9]|0[1-9])\\/(1[0-2]|0[1-9])\\/[12][0-9]{3}$";

        public const int MAX_NVARCHAR = 2147483647;

        public const string ENABLE_ATTRIBUTE = "EnableAttribute";
        public const string INTEGER_ATTRIBUTE = "IntegerAttribute";
        public const string FLOAT_ATTRIBUTE = "FloatAttribute";
        public const string REGEX_ATTRIBUTE = "RegularExpressionAttribute";
        public const string URL_ATTRIBUTE = "UrlAttribute";
        public const string REQUIRED_ATTRIBUTE = "RequiredAttribute";
        public const string RANGE_ATTRIBUTE = "RangeAttribute";
        public const string MAX_LENGTH_ATTRIBUTE = "MaxLengthAttribute";
        public const string STRING_LENGTH_ATTRIBUTE = "StringLengthAttribute";
        public const string EMAIL_ATTRIBUTE = "EmailAddressAttribute";



        public class ValueDefinitions
        {
            public const string INT = "int";
            public const string STRING = "string";
            public const string BOOL = "bool";
            public const string FLOAT = "float";
            public const string DATE = "date";
            public const string TIME = "time";

        }
    }
}
