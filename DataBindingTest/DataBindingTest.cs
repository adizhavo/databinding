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
            dataBinding.AddDataNode<bool>("test.bool");
            Assert.IsTrue(dataBinding.ContainsNode("test.bool"));
        }

        [Test()]
        public void TestAdditionOfMultipleData()
        {
            dataBinding.AddDataNode<bool>("test.bool");
            dataBinding.AddDataNode<string>("test.string");

            Assert.IsTrue(dataBinding.ContainsNode("test.bool") && dataBinding.ContainsNode("test.string"));
        }

        [Test()]
        public void TestAdditionOfSingleDefaultData()
        {
            dataBinding.AddDataNode<bool>("test.bool", true);
            Assert.IsTrue(dataBinding.ContainsNode("test.bool"));
            Assert.IsFalse(dataBinding.ContainsNode("test.bool.false"));
        }

        [Test()]
        public void TestCheckOfNodeBasedOnIdAndDepth()
        {
            dataBinding.AddDataNode<bool>("test.bool.thirdLayerOfDepth", true);
            Assert.IsTrue(dataBinding.ContainsNode("thirdLayerOfDepth", 2));
        }

        [Test()]
        public void TestExtractionOfNode()
        {
            dataBinding.AddDataNode<bool>("test.bool.true", true);
            dataBinding.AddDataNode<bool>("test.bool.false");

            Assert.IsTrue(dataBinding.ExtractNode("test.bool.true") != null);
            Assert.IsTrue(dataBinding.ExtractNode("test.bool.false") != null);
        }

        [Test()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionIfAddingDataWithoutBranch()
        {
            dataBinding.AddDataNode<bool>(string.Empty);
        }

        [Test()]
        public void TestGettingData()
        {
            dataBinding.AddDataNode<bool>("test.bool.true", true);
            dataBinding.AddDataNode<bool>("test.bool.false");

            Assert.IsTrue(dataBinding.GetData<bool>("test.bool.true").value);
            Assert.IsFalse(dataBinding.GetData<bool>("test.bool.false").value);
            Assert.IsNull(dataBinding.GetData<object>("test.nulldata"));
        }
    }
}
