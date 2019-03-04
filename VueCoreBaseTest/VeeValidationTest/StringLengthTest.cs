using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VueCoreBaseTest.VeeValidation
{
    [TestClass]
    public class StringLengthTest
    {
        [TestMethod]
        public void StringLength()
        {
            // setup
            ViewModelBase uut = new StringLengthTestClass();

            // VeeValidation Key
            var VeePropName = "testProperty";

            var VeeKey = "max";
            var VeeValue = 250;

            // operation
            var result = uut.Validations;

            // assert

            var getProperty = result.SingleOrDefault(a => a.Key == VeePropName);
            var getRequired = result[VeePropName].validations.SingleOrDefault(a => a.Key == VeeKey);
            var getMin = result[VeePropName].validations.SingleOrDefault(a => a.Key == "min");


            Assert.AreEqual(VeePropName, getProperty.Key);
            Assert.AreEqual(VeeKey, getRequired.Key);
            Assert.AreEqual(VeeValue, getRequired.Value);

            Assert.AreEqual("min", getMin.Key);
            Assert.AreEqual(3, getMin.Value);

        }

        public class StringLengthTestClass : ViewModelBase
        {
            [StringLength(250, ErrorMessage = "{0} max {1} characters long.", MinimumLength = 3)]
            public string TestProperty { get; set; }
        }
    }
}
