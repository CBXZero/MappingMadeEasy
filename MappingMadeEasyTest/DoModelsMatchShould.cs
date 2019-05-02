using MappingMadeEasy.Standard.Nuget;
using MappingMadeEasy.Standard.Nuget.ModelHelper;
using MappingMadeEasyTest.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomTestValues;

namespace MappingMadeEasyTest
{
    [TestClass]
    public class DoModelsMatchShould
    {
        [TestMethod]
        public void ReturnTrueWhenModelsMatch()
        {
            var mapper = new Mapper();
            var sut = new ModelHelper();

            var modelToTest1 = RandomValue.Object<SimpleTestModel>();
            var modelToTest2 = mapper.Map<SimpleTestModel, SimpleTestModelAlternative>(modelToTest1);

            var result = sut.DoModelsMatch(modelToTest1, modelToTest2);

            Assert.IsTrue(result.IsEquivalent);
        }

        [TestMethod]
        public void ReturnTrueWhenNumberOfPropertiesDoMatchAndPropertiesMustMatchIsSetToTrue()
        {
            var mapper = new Mapper();
            var sut = new ModelHelper();

            var modelToTest1 = RandomValue.Object<SimpleTestModel>();
            var modelToTest2 = mapper.Map<SimpleTestModel, SimpleTestModelAlternative>(modelToTest1);

            var result = sut.DoModelsMatch(modelToTest1, modelToTest2, true);

            Assert.IsTrue(result.IsEquivalent);;
        }

        [TestMethod]
        public void ReturnFalseWhenNumberOfPropertiesDoNotMatchAndStrictIsSetToTrue()
        {
            var mapper = new Mapper();
            var sut = new ModelHelper();

            var modelToTest1 = RandomValue.Object<SimpleTestModel>();
            var modelToTest2 = mapper.Map<SimpleTestModel, SimpleTestModelWithSystemStruct>(modelToTest1, false);

            var result = sut.DoModelsMatch(modelToTest1, modelToTest2, true);

            Assert.IsFalse(result.IsEquivalent);
        }

        [TestMethod]
        public void ReturnFalseWhenPropertyNamesDoNotMatchAndStrictToTrue()
        {
            var mapper = new Mapper();
            var sut = new ModelHelper();

            var modelToTest1 = RandomValue.Object<SimpleTestModel>();
            var modelToTest2 = mapper.Map<SimpleTestModel, TestModelWithDifferentAttributeName>(modelToTest1, false);

            var result = sut.DoModelsMatch(modelToTest1, modelToTest2, true);

            Assert.IsFalse(result.IsEquivalent);
        }

        [TestMethod]
        public void ReturnFalseWhenNumberOfModelsDoNotMatch()
        {
            var mapper = new Mapper();
            var sut = new ModelHelper();

            var modelToTest1 = RandomValue.Object<SimpleTestModel>();
            var modelToTest2 = mapper.Map<SimpleTestModel, SimpleTestModelAlternative>(modelToTest1);
            modelToTest2.PersonName = "IAmWrongName";

            var result = sut.DoModelsMatch(modelToTest1, modelToTest2);

            Assert.IsFalse(result.IsEquivalent);
            Assert.IsTrue(result.ResultMessage.Contains(modelToTest1.Name) && result.ResultMessage.Contains(modelToTest2.PersonName));
            Assert.IsTrue(result.ResultMessage.Contains("Name") && result.ResultMessage.Contains("PersonName"));
        }

        [TestMethod]
        public void ReturnFalseWhenModelsListDoNotMatch()
        {
            var mapper = new Mapper();
            var sut = new ModelHelper();

            var modelToTest1 = RandomValue.Object<SimpleTestModel>();
            var modelToTest2 = mapper.Map<SimpleTestModel, SimpleTestModelAlternative>(modelToTest1);
            modelToTest2.RandomData[0] = "IAmWrongValue";

            var result = sut.DoModelsMatch(modelToTest2, modelToTest1);

            Assert.IsFalse(result.IsEquivalent);
            Assert.IsTrue(result.ResultMessage.Contains("IAmWrongValue"));
            Assert.IsTrue(result.ResultMessage.Contains("RandomData"));
        }

        [TestMethod]
        public void ReturnFalseWhenFirstModelHaveNoMappablePropertiesAndStrictIfFalse()
        {
            var mapper = new Mapper();
            var sut = new ModelHelper();

            var modelToTest1 = RandomValue.Object<SimpleTestModelWithNoMappableProperties>();
            var modelToTest2 = mapper.Map<SimpleTestModelWithNoMappableProperties, SimpleTestModelAlternative>(modelToTest1, false);

            var result = sut.DoModelsMatch(modelToTest1, modelToTest2);

            Assert.IsFalse(result.IsEquivalent);
            Assert.IsTrue(result.ResultMessage.ToLower().Contains("property names on models do not match"));
        }

        [TestMethod]
        public void ReturnFalseWhenSecondModelHaveNoMappablePropertiesAndStrictIfFalse()
        {
            var mapper = new Mapper();
            var sut = new ModelHelper();

            var modelToTest1 = RandomValue.Object<SimpleTestModel>();
            var modelToTest2 = mapper.Map<SimpleTestModel, SimpleTestModelAlternativeWithNoMappableProperties>(modelToTest1, false);

            var result = sut.DoModelsMatch(modelToTest1, modelToTest2);

            Assert.IsFalse(result.IsEquivalent);
            Assert.IsTrue(result.ResultMessage.ToLower().Contains("property names on models do not match"));
        }

        [TestMethod]
        public void ReturnTrueWhenModelsWithChildModelMatch()
        {
            var mapper = new Mapper();
            var sut = new ModelHelper();

            var modelToTest1 = RandomValue.Object<TestModelWithChildModel>();
            var modelToTest2 = mapper.Map<TestModelWithChildModel, TestModelWithChildModelAlternative>(modelToTest1);

            var result = sut.DoModelsMatch(modelToTest1, modelToTest2);

            Assert.IsTrue(result.IsEquivalent);;
        }

        [TestMethod]
        public void ReturnFalseWhenModelsWithChildModelMatch()
        {
            var mapper = new Mapper();
            var sut = new ModelHelper();

            var modelToTest1 = RandomValue.Object<TestModelWithChildModel>();
            var modelToTest2 = mapper.Map<TestModelWithChildModel, TestModelWithChildModelAlternative>(modelToTest1);
            modelToTest2.TestModel.PersonName = "IAmWrongValue";

            var result = sut.DoModelsMatch(modelToTest1, modelToTest2);

            Assert.IsFalse(result.IsEquivalent);
        }

        [TestMethod]
        public void ReturnTrueWhenModelsWithListOfChildMatch()
        {
            var mapper = new Mapper();
            var sut = new ModelHelper();

            var modelToTest1 = RandomValue.Object<TestModelWithChildModelList>();
            var modelToTest2 = mapper.Map<TestModelWithChildModelList, TestModelWithChildModelListAlternative>(modelToTest1);

            var result = sut.DoModelsMatch(modelToTest1, modelToTest2);

            Assert.IsTrue(result.IsEquivalent);;
        }

        [TestMethod]
        public void ReturnFalseWhenModelsWithListOfChildMatch()
        {
            var mapper = new Mapper();
            var sut = new ModelHelper();

            var modelToTest1 = RandomValue.Object<TestModelWithChildModelList>();
            var modelToTest2 = mapper.Map<TestModelWithChildModelList, TestModelWithChildModelListAlternative>(modelToTest1);
            modelToTest2.TestModels[0].PersonName = "IAmWrongValue";

            var result = sut.DoModelsMatch(modelToTest1, modelToTest2);

            Assert.IsFalse(result.IsEquivalent);
        }
    }
}