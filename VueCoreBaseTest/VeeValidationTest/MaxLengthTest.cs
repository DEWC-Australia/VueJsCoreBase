using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VueCoreBaseTest.VeeValidation
{
    [TestClass]
    public class MaxLengthTest
    {

        [TestMethod]
        public void MaxLength()
        {
            // setup
            ViewModelBase uut = new MaxLengthTestClass();

            // VeeValidation Key
            var VeePropName = "testProperty";

            var VeeKey = "max";
            var VeeValue = 100;

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
    public class MaxLengthTestClass : ViewModelBase
    {
        [MaxLength(100)]
        public string TestProperty { get; set; }
    }

}
