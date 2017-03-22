using DataBinding;
using NUnit.Framework;

namespace DataBindingTest
{
    [TestFixture()]
    public class BindingComponentTest
    {
        private DataBindingService dataBinding;
        private SampleBindingComponent bindingComponent;

        [SetUp()]
        public void SetUp()
        {
            dataBinding = new DataBindingService();
            bindingComponent = new SampleBindingComponent();
        }

        [Test()]
        public void TestReceivedDefaultDataValue()
        {
            dataBinding.AddData<bool>("bool.false", true);
            dataBinding.AddData<string>("string.notEmpty", "string");

            dataBinding.Bind<bool>("bool.false", bindingComponent);
            dataBinding.Bind<string>("string.notEmpty", bindingComponent);

            Assert.IsTrue(bindingComponent.booleanValue);
            Assert.AreEqual(bindingComponent.stringValue, "string");
        }

        [Test()]
        public void TestReceiveChangedDataValue()
        {
            dataBinding.AddData<bool>("bool.false", true);
            dataBinding.AddData<string>("string.notEmpty", "string");

            dataBinding.Bind<bool>("bool.false", bindingComponent);
            dataBinding.Bind<string>("string.notEmpty", bindingComponent);

            Assert.IsTrue(bindingComponent.booleanValue);
            Assert.IsNotEmpty(bindingComponent.stringValue);

            dataBinding.GetData<bool>("bool.false").value = false;
            dataBinding.GetData<string>("string.notEmpty").value = string.Empty;

            Assert.IsFalse(bindingComponent.booleanValue);
            Assert.IsEmpty(bindingComponent.stringValue);
        }
    }

    public class SampleBindingComponent : BindingComponent<bool>, BindingComponent<string>
    {
        public bool booleanValue;
        public string stringValue;

        public void OnValueChanged(string branch, bool value)
        {
            booleanValue = value;
        }

        public void OnValueChanged(string branch, string value)
        {
            stringValue = value;
        }
    }
}