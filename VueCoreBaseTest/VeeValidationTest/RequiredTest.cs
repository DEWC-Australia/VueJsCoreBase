using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VueCoreBaseTest.VeeValidation
{
    [TestClass]
    public class RequiredTest
    {
        [TestMethod]
        public void Required()
        {
            // setup
            ViewModelBase uut = new RequiredTestClass();

            // VeeValidation Key
            var VeePropName = "testProperty";
            var VeePropDisplayName = "TestProperty";

            var VeeKey = "required";
            var VeeValue = true;

            // operation
            var result = uut.Validations;

            // assert

            var getProperty = result.SingleOrDefault(a => a.Key == VeePropName);
            var getRequired = result[VeePropName].validations.SingleOrDefault(a => a.Key == VeeKey);


            Assert.AreEqual(VeePropName, getProperty.Key);
            Assert.AreEqual(VeePropDisplayName, result[VeePropName].displayName);
 

            Assert.AreEqual(VeeKey, getRequired.Key);
            Assert.AreEqual(VeeValue, getRequired.Value);

        }

        [TestMethod]
        public void RequiredDisplayName()
        {
            // setup
            ViewModelBase uut = new RequiredDisplayNameTestClass();

            // VeeValidation Key
            var VeePropName = "testProperty";
            var VeePropDisplayName = "Test Property";

            var VeeKey = "required";
            var VeeValue = true;

            // operation
            var result = uut.Validations;

            // assert

            var getProperty = result.SingleOrDefault(a => a.Key == VeePropName);
            var getRequired = result[VeePropName].validations.SingleOrDefault(a => a.Key == VeeKey);


            Assert.AreEqual(VeePropName, getProperty.Key);
            Assert.AreEqual(VeePropDisplayName, result[VeePropName].displayName);


            Assert.AreEqual(VeeKey, getRequired.Key);
            Assert.AreEqual(VeeValue, getRequired.Value);

        }


    }

    public class RequiredTestClass: ViewModelBase
    {
        [Required]
        public string TestProperty { get; set; }
    }

    public class RequiredDisplayNameTestClass : ViewModelBase
    {
        [Required]
        [Display(Name = "Test Property")]
        public string TestProperty { get; set; }
    }


}
