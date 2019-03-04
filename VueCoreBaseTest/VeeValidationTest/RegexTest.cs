using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VueCoreBaseTest.VeeValidation
{
    [TestClass]
    public class RegexTest
    {

        [TestMethod]
        public void Regex()
        {
            // setup
            ViewModelBase uut = new RegexTestClass();

            // VeeValidation Key
            var VeePropName = "testProperty";

            var VeeKey = "regex";
            var VeeValue = "^([0-9]+)$";

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
    public class RegexTestClass : ViewModelBase
    {
        [RegularExpression("^([0-9]+)$")]
        public string TestProperty { get; set; }
    }

}
