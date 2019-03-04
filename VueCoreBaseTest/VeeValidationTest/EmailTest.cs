using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VueCoreBaseTest.VeeValidation
{
    [TestClass]
    public class EmailTest
    {
        [TestMethod]
        public void Email()
        {
            // setup
            ViewModelBase uut = new EmailTestClass();

            // VeeValidation Key
            var VeePropName = "testProperty";

            var VeeKey = "email";
            var VeeValue = true;

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
    public class EmailTestClass: ViewModelBase
    {
        [EmailAddress]
        public string TestProperty { get; set; }
    }

}
