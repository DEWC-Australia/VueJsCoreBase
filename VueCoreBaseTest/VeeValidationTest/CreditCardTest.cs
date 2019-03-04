using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Base;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VueCoreBaseTest.VeeValidation
{
    [TestClass]
    public class CreditCardTest
    {

        [TestMethod]
        public void CreditCard()
        {
            // setup
            ViewModelBase uut = new CreditCardTestClass();

            // VeeValidation Key
            var VeePropName = "testProperty";

            var VeeKey = "credit_card";
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
    public class CreditCardTestClass : ViewModelBase
    {
        [CreditCard]
        public string TestProperty { get; set; }

    }

}
