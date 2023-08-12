using MaxWorld.Web.Utilities;

namespace MaxWorld.UnitTest
{
    [TestClass]
    public class ObjectMapperTests
    {
        private class SourceModel
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string MappedField { get; set; }
        }

        private class DestinationModel
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Field { get; set; }
        }

        [TestMethod]
        public void MapTo_SamePropertyName_MapsCorrectly()
        {
            var source = new SourceModel { Name = "John", Age = 30 };
            var result = ObjectMapper.MapTo<DestinationModel>(source);

            Assert.AreEqual(source.Name, result.Name);
            Assert.AreEqual(source.Age, result.Age);
            Assert.IsNull(result.Field);
        }

        [TestMethod]
        public void MapTo_UsingSourceFindingRules_MapsCorrectly()
        {
            var source = new SourceModel { Name = "John", Age = 30, MappedField = "Value" };
            var result = ObjectMapper.MapTo<DestinationModel>(source, propName =>
            {
                return propName == "Field" ? "MappedField" : propName;
            });

            Assert.AreEqual(source.Name, result.Name);
            Assert.AreEqual(source.Age, result.Age);
            Assert.AreEqual(source.MappedField, result.Field);
        }
    }
}