using MappingMadeEasy.Standard.Nuget;
using MappingMadeEasyTest.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomTestValues;
using System;
using System.Linq;
using MappingMadeEasy.Standard.Nuget.CustomExceptions;

namespace MappingMadeEasyTest
{
    [TestClass]
    public class MapShould
    {
        [TestMethod]
        public void ReturnNewModelWithPreviousModelValues()
        {
            var sut = new Mapper();
            var testModel = RandomValue.Object<SimpleTestModel>();
            var resultModel = sut.Map<SimpleTestModel, SimpleTestModelAlternative>(testModel);

            Assert.AreEqual(testModel.Age, resultModel.CurrentAge);
            Assert.AreEqual(testModel.Name, resultModel.PersonName);
            Assert.AreEqual(testModel.Salary, resultModel.CurrentSalary);
            CollectionAssert.AreEquivalent(testModel.RandomData, resultModel.RandomData);
        }

        [TestMethod]
        public void ReturnNewModelWithPreviousModelValuesWhenNewModelHasPropertiesWithoutAttributesAndStrictSetToFalse()
        {
            var sut = new Mapper();
            var testModel = RandomValue.Object<SimpleTestModel>();
            var resultModel = sut.Map<SimpleTestModel, TestModelWithoutAttribute>(testModel, false);

            Assert.AreEqual(testModel.Age, resultModel.CurrentAge);
            Assert.AreEqual(testModel.Name, resultModel.PersonName);
            CollectionAssert.AreEquivalent(testModel.RandomData, resultModel.RandomData);
        }

        [TestMethod]
        public void ThrowExceptionWhenSecondModelIsMissingPropertyAndStrictIsSetToTrue()
        {
            var correctExceptionThrown = false;
            var sut = new Mapper();
            var testModel = RandomValue.Object<SimpleTestModel>();
            try
            {
                sut.Map<SimpleTestModel, TestModelWithExtraAttribute>(testModel, true);
            }
            catch (MissingPropertyException)
            {
                correctExceptionThrown = true;
            }
            catch (Exception)
            {
                Assert.Fail("General exception was thrown");
            }

            Assert.IsTrue(correctExceptionThrown);
        }

        [TestMethod]
        public void ThrowExceptionWhenSecondModelIsHasExtraPropertyAndStrictIsSetToTrue()
        {
            var correctExceptionThrown = false;
            var sut = new Mapper();
            var testModel = RandomValue.Object<SimpleTestModel>();
            try
            {
                sut.Map<SimpleTestModel, TestModelWithoutAttribute>(testModel, true);
            }
            catch (MissingPropertyException)
            {
                correctExceptionThrown = true;
            }
            catch (Exception)
            {
                Assert.Fail("General exception was thrown");
            }

            Assert.IsTrue(correctExceptionThrown);
        }


        [TestMethod]
        public void ThrowExceptionWhenSecondModelIsHasPropertyWithDifferentNameAndStrictIsSetToTrue()
        {
            var correctExceptionThrown = false;
            var sut = new Mapper();
            var testModel = RandomValue.Object<SimpleTestModel>();
            try
            {
                sut.Map<SimpleTestModel, TestModelWithDifferentAttributeName>(testModel, true);
            }
            catch (PropertyMismatchException)
            {
                correctExceptionThrown = true;
            }
            catch (Exception)
            {
                Assert.Fail("General exception was thrown");
            }

            Assert.IsTrue(correctExceptionThrown);
        }

        [TestMethod]
        public void ReturnNewModelWithPreviousModelValuesWhenPreviousModelHasPropertiesWithoutAttributes()
        {
            var sut = new Mapper();
            var testModel = RandomValue.Object<TestModelWithoutAttribute>();
            var resultModel = sut.Map<TestModelWithoutAttribute, SimpleTestModel>(testModel, false);

            Assert.AreEqual(testModel.CurrentAge, resultModel.Age);
            Assert.AreEqual(testModel.PersonName, resultModel.Name);
            CollectionAssert.AreEquivalent(testModel.RandomData, resultModel.RandomData);
        }

        [TestMethod]
        public void ReturnNewModelWithPreviousModelValuesWhenBothModelHasAChildModel()
        {
            var sut = new Mapper();
            var testModel = RandomValue.Object<TestModelWithChildModel>();
            var resultModel = sut.Map<TestModelWithChildModel, TestModelWithChildModelAlternative>(testModel);

            Assert.AreEqual(testModel.Age, resultModel.Age);
            Assert.AreEqual(testModel.Name, resultModel.Name);
            CollectionAssert.AreEquivalent(testModel.RandomData, resultModel.RandomData);

            Assert.AreEqual(testModel.TestModel.Age, resultModel.TestModel.CurrentAge);
            Assert.AreEqual(testModel.TestModel.Name, resultModel.TestModel.PersonName);
            Assert.AreEqual(testModel.TestModel.Salary, resultModel.TestModel.CurrentSalary);
            CollectionAssert.AreEquivalent(testModel.TestModel.RandomData, resultModel.TestModel.RandomData);
        }

        [TestMethod]
        public void ReturnNewModelWithPreviousModelValuesWhenBothModelHasAChildModelWithoutIsModelAnnotation()
        {
            var sut = new Mapper();
            var testModel = RandomValue.Object<TestModelWithChildModelNoAnnotation>();
            var resultModel = sut.Map<TestModelWithChildModelNoAnnotation, TestModelWithChildModelAlternativeNoAnnotation>(testModel);

            Assert.AreEqual(testModel.Age, resultModel.Age);
            Assert.AreEqual(testModel.Name, resultModel.Name);
            CollectionAssert.AreEquivalent(testModel.RandomData, resultModel.RandomData);

            Assert.AreEqual(testModel.TestModel.Age, resultModel.TestModel.CurrentAge);
            Assert.AreEqual(testModel.TestModel.Name, resultModel.TestModel.PersonName);
            Assert.AreEqual(testModel.TestModel.Salary, resultModel.TestModel.CurrentSalary);
            CollectionAssert.AreEquivalent(testModel.TestModel.RandomData, resultModel.TestModel.RandomData);
        }

        [TestMethod]
        public void ReturnNewModelWithPreviousModelValuesWhenBothHasAStruct()
        {
            var sut = new Mapper();
            var testModel = RandomValue.Object<SimpleTestModelWithSystemStruct>();
            var resultModel = sut.Map<SimpleTestModelWithSystemStruct, SimpleTestModelWithSystemStructAlternate>(testModel);

            Assert.AreEqual(testModel.Age, resultModel.CurrentAge);
            Assert.AreEqual(testModel.Name, resultModel.PersonName);
            Assert.AreEqual(testModel.Id, resultModel.PrimaryId);
            CollectionAssert.AreEquivalent(testModel.RandomData, resultModel.RandomData);
        }

        [TestMethod]
        public void ThrowExceptionIfBothModelsAreNull()
        {
            var sut = new Mapper();
            SimpleTestModel testModel = null;
            var exceptionThrown = false;

            try
            {
                sut.Map<SimpleTestModel, SimpleTestModelAlternative>(testModel);
            }
            catch (ArgumentNullException e)
            {
                exceptionThrown = true;
            }
            catch (Exception)
            {
                Assert.Fail("General exception thrown instead of null exception");
            }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void CorrectlyMapNullValues()
        {
            var sut = new Mapper();
            var testModel = RandomValue.Object<TestModelWithChildModel>();
            testModel.RandomData = null;
            testModel.TestModel = null;

            var resultModel = sut.Map<TestModelWithChildModel, TestModelWithChildModelAlternative>(testModel);

            Assert.AreEqual(testModel.Age, resultModel.Age);
            Assert.AreEqual(testModel.Name, resultModel.Name);
            Assert.IsNull(resultModel.RandomData);
            Assert.IsNull(resultModel.TestModel);
        }

        [TestMethod]
        public void ReturnNewModelWithPreviousModelValuesWhenBothModelHasAListOfChildModels()
        {
            var sut = new Mapper();
            var testModel = RandomValue.Object<TestModelWithChildModelList>();
            var resultModel = sut.Map<TestModelWithChildModelList, TestModelWithChildModelListAlternative>(testModel);

            Assert.AreEqual(testModel.Age, resultModel.Age);
            Assert.AreEqual(testModel.Name, resultModel.Name);
            CollectionAssert.AreEquivalent(testModel.RandomData, resultModel.RandomData);
            Assert.AreEqual(testModel.TestModels.Count, resultModel.TestModels.Count);

            foreach (var model in testModel.TestModels)
            {
                var match = resultModel.TestModels.FirstOrDefault(m => m.PersonName == model.Name);

                Assert.IsNotNull(match);
                Assert.AreEqual(model.Age, match.CurrentAge);
                Assert.AreEqual(model.Salary, match.CurrentSalary);
                CollectionAssert.AreEquivalent(model.RandomData, match.RandomData);
            }
        }
    }
}
