using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VueCoreBaseTest.VeeValidation
{
    [TestClass]
    public class ConfirmTest
    {

        [TestMethod]
        public void Confirm()
        {
            // setup
            ViewModelBase uut = new ConfirmTestClass();

            // VeeValidation Key
            var VeePropName = "testProperty";

            var VeeKey = "confirmed";
            var VeeValue = "Password";

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
    public class ConfirmTestClass : ViewModelBase
    {
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string TestProperty { get; set; }
    }

}
