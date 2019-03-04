using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VueCoreBaseTest.VeeValidation
{
    [TestClass]
    public class RangeTest
    {

        [TestMethod]
        public void Range()
        {
            // setup
            ViewModelBase uut = new RangeTestClass();

            // VeeValidation Key
            var VeePropName = "integerProperty";
            var VeePropName2 = "doubleProperty";

            var VeeKey = "between";
            var VeeValue = new List<double> { 0, 10 };

            var intKey = "numeric";
            var intValue = true;

            var doubleKey = "decimal";
            var doubleValue = 50;



            // operation
            var result = uut.Validations;

            // assert

            var getProperty = result.SingleOrDefault(a => a.Key == VeePropName);
            var getProperty2 = result.SingleOrDefault(a => a.Key == VeePropName2);
            var getRequired = result[VeePropName].validations.SingleOrDefault(a => a.Key == VeeKey);
            var intVal = result[VeePropName].validations.SingleOrDefault(a => a.Key == intKey);

            var getRequired2 = result[VeePropName2].validations.SingleOrDefault(a => a.Key == VeeKey);
            var doubleVal = result[VeePropName2].validations.SingleOrDefault(a => a.Key == doubleKey);

            var value = (List<double>)getRequired.Value;
            var value2 = (List<double>)getRequired2.Value;

            Assert.AreEqual(VeePropName, getProperty.Key);
            Assert.AreEqual(VeeKey, getRequired.Key);
            Assert.AreEqual(VeeValue.ElementAt(0), value.ElementAt(0));
            Assert.AreEqual(VeeValue.ElementAt(1), value.ElementAt(1));

            Assert.AreEqual(intVal.Key, intKey);
            Assert.AreEqual(intVal.Value, intValue);



            Assert.AreEqual(VeePropName2, getProperty2.Key);
            Assert.AreEqual(VeeKey, getRequired2.Key);
            Assert.AreEqual(VeeValue.ElementAt(0), value2.ElementAt(0));
            Assert.AreEqual(VeeValue.ElementAt(1), value2.ElementAt(1));

            Assert.AreEqual(doubleVal.Key, doubleKey);
            Assert.AreEqual(doubleVal.Value, doubleValue);

        }
    }
    public class RangeTestClass : ViewModelBase
    {
        [Range(0,10)]
        public int IntegerProperty { get; set; }
        [Range(0.0, 10.0)]
        public double DoubleProperty { get; set; }
    }

}
