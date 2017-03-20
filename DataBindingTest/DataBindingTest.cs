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
            dataBinding.AddData<bool>("test.bool", true);
            Assert.IsTrue(dataBinding.ContainsNode("test.bool"));
            Assert.IsFalse(dataBinding.ContainsNode("test.bool.false"));
        }

        [Test()]
        public void TestCheckOfNodeBasedOnIdAndDepth()
        {
            dataBinding.AddData<bool>("test.bool.secondLayerOfDepth", true);
            Assert.IsTrue(dataBinding.ContainsNode("secondLayerOfDepth", 2));
        }

        [Test()]
        public void TestExtractionOfNode()
        {
            dataBinding.AddData<bool>("test.bool.true", true);
            dataBinding.AddData<bool>("test.bool.false");

            Assert.IsTrue(dataBinding.ExtractNode("test.bool.true") != null);
            Assert.IsTrue(dataBinding.ExtractNode("test.bool.false") != null);
        }

        [Test()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionIfAddingDataWithoutBranch()
        {
            dataBinding.AddData<bool>(string.Empty);
        }

        [Test()]
        public void TestGettingData()
        {
            dataBinding.AddData<bool>("test.bool.true", true);
            dataBinding.AddData<bool>("test.bool.false");

            Assert.IsTrue(dataBinding.GetData<bool>("test.bool.true").value);
            Assert.IsFalse(dataBinding.GetData<bool>("test.bool.false").value);
            Assert.IsNull(dataBinding.GetData<object>("test.nulldata"));
        }
    }
}
