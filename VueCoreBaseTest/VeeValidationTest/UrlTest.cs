using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VueCoreBaseTest.VeeValidation
{
    [TestClass]
    public class UrlTest
    {

        [TestMethod]
        public void Url()
        {
            // setup
            ViewModelBase uut = new UrlTestClass();

            // VeeValidation Key
            var VeePropName = "testProperty";

            var VeeKey = "url";
            var VeeValue = new Dictionary<string, bool>(){
                            { "require_protocol", true}
                        };

            // operation
            var result = uut.Validations;

            // assert

            var getProperty = result.SingleOrDefault(a => a.Key == VeePropName);
            var getRequired = result[VeePropName].validations.SingleOrDefault(a => a.Key == VeeKey);
            var value = (Dictionary<string, bool>)getRequired.Value;

            Assert.AreEqual(VeePropName, getProperty.Key);
            Assert.AreEqual(VeeKey, getRequired.Key);
            Assert.AreEqual(VeeValue.First().Value, value.First().Value);

        }
    }
    public class UrlTestClass : ViewModelBase
    {
        [Url]
        public string TestProperty { get; set; }
    }

}
