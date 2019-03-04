using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Base;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VueCoreBaseTest.VeeValidation
{
    [TestClass]
    public class MinLengthTest
    {

        [TestMethod]
        public void MinLength()
        {
            // setup
            ViewModelBase uut = new MinLengthTestClass();

            // VeeValidation Key
            var VeePropName = "testProperty";

            var VeeKey = "min";
            var VeeValue = 10;

            // operation
            var result = uut.Validations;

            // assert

            var getProperty = result.SingleOrDefault(a => a.Key == VeePropName);
            var getRequired = result[VeePropName].validations.SingleOrDefault(a => a.Key == VeeKey);

            Assert.AreEqual(VeePropName, getProperty.Key);
            Assert.AreEqual(VeeKey, getRequired.Key);
            Assert.AreEqual(VeeValue, getRequired.Value);

        }
    }
    public class MinLengthTestClass : ViewModelBase
    {
        [MinLength(10)]
        public string TestProperty { get; set; }
    }

}
