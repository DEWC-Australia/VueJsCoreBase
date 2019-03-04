using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Model.VeeValidation.Builders;

namespace Models.VeeValidation.Builders.Implementations
{
    public class RangeBuilder : ValidationBuilderBase
    {

        public RangeBuilder(CustomAttributeData attr, string propName) : base(attr, propName)
        {

        }

        protected override Dictionary<string, dynamic> BuildValidation(CustomAttributeData attr, string propName)
        {
            var inputs = attr.ConstructorArguments.Where(a => a.ArgumentType.Name == "Double");

            bool isdouble = true;

            if (inputs.Count() < 2)
            {
                inputs = attr.ConstructorArguments.Where(a => a.ArgumentType.Name == "Int32");
                isdouble = false;
            }


            if (inputs.Count() < 2)
                return new Dictionary<string, dynamic>();


            double min = double.Parse(inputs.ElementAt(0).Value.ToString());
            double max = double.Parse(inputs.ElementAt(1).Value.ToString());

            if (isdouble)
            {
                return new Dictionary<string, dynamic>
                {
                    { "between",new List<double> { min, max }},
                    { "decimal", 50 }
                };
            }

            return new Dictionary<string, dynamic>
            {
                    { "between",new List<double> { min, max }},
                    { "numeric", true }
            };
        }

    }
}
