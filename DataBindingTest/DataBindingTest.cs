using System;
using DataBinding;
using NUnit.Framework;

namespace DataBindingTest
{
    [TestFixture()]
    public class DataBindingTest
    {
        private DataBindingService dataBinding;

        [SetUp]
        public void SetUp()
        {
            dataBinding = new DataBindingService();
        }

        [Test()]
        public void TestAdditionOfSingleData()
        {
            dataBinding.AddData<bool>("test.bool");
            Assert.IsTrue(dataBinding.ContainsNode("test.bool"));
        }

        [Test()]
        public void TestAdditionOfMultipleData()
        {
            dataBinding.AddData<bool>("test.bool");
            dataBinding.AddData<string>("test.string");

            Assert.IsTrue(dataBinding.ContainsNode("test.bool") && dataBinding.ContainsNode("test.string"));
        }

        [Test()]
        public void TestAdditionOfSingleDefaultData()
        {
            dataBinding.AddData<bool>("test.bool.true", true);
            Assert.IsTrue(dataBinding.ContainsNode("test.bool.true"));
            Assert.IsFalse(dataBinding.ContainsNode("test.bool.false"));
        }

        [Test()]
        public void TestExtractionOfNode()
        {
            dataBinding.AddData<bool>("test.bool.true", true);
            dataBinding.AddData<bool>("test.bool.false");

            Assert.IsNotNull(dataBinding.ExtractNode("test.bool.true"));
            Assert.IsNotNull(dataBinding.ExtractNode("test.bool.false"));
        }

        [Test()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionIfAddingDataWithoutBranch()
        {
            dataBinding.AddData<bool>(string.Empty, false);
        }

        [Test()]
        public void TestGettingData()
        {
            dataBinding.AddData<bool>("test.bool.true", true);
            dataBinding.AddData<bool>("test.bool.false", false);

            Assert.IsTrue(dataBinding.GetData<bool>("test.bool.true").value);
            Assert.IsFalse(dataBinding.GetData<bool>("test.bool.false").value);
            Assert.IsNull(dataBinding.GetData<object>("test.nulldata"));
        }
    }
}
