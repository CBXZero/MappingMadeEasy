﻿using System.Collections.Generic;
using MappingMadeEasy.Standard.Nuget.CustomDataAttributes;

namespace MappingMadeEasyTest.TestModels
{
    [IsModel]
    public class TestModelWithChildModelListAlternative
    {
        [MapToName("Name")]
        public string Name { get; set; }
        [MapToName("Age")]
        public int Age { get; set; }
        [MapToName("RandomData")]
        public List<string> RandomData { get; set; }
        [MapToName("TestModel")]
        public List<SimpleTestModelAlternative> TestModels { get; set; }
    }
}