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
    }
}
