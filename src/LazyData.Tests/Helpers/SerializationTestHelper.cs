using System;
using System.Collections.Generic;
using System.Numerics;
using LazyData.Tests.Models;
using Newtonsoft.Json.Linq;
using Assert = LazyData.Tests.Extensions.AssertExtensions;

namespace LazyData.Tests.Helpers
{
    public static class SerializationTestHelper
    {
        public static ComplexModel GeneratePopulatedModel()
        {
            var a = new ComplexModel();
            a.TestValue = "WOW";
            a.NonPersisted = 100;
            a.Stuff.Add("woop");
            a.Stuff.Add("poow");

            a.NestedValue = new B
            {
                IntValue = 0,
                StringValue = "Hello",
                NestedArray = new[] { new C { FloatValue = 2.43f } }
            };

            a.NestedArray = new B[2];
            a.NestedArray[0] = new B
            {
                IntValue = 20,
                StringValue = "There",
                NestedArray = new[] { new C { FloatValue = 3.5f } }
            };

            a.NestedArray[1] = new B
            {
                IntValue = 30,
                StringValue = "Sir",
                NestedArray = new[]
                {
                     new C { FloatValue = 4.1f },
                     new C { FloatValue = 5.2f }
                }
            };

            a.AllTypes = new CommonTypesModel
            {
                ByteValue = byte.MaxValue,
                ShortValue = short.MaxValue,
                IntValue = int.MaxValue,
                LongValue = long.MaxValue,
                GuidValue = Guid.NewGuid(),
                DateTimeValue = DateTime.MaxValue,
                TimeSpanValue = TimeSpan.FromMilliseconds(10),
                SomeType = SomeTypes.Known
            };

            a.SimpleDictionary.Add("key1", "some-value");
            a.SimpleDictionary.Add("key2", "some-other-value");

            a.ComplexDictionary.Add(new E { IntValue = 10 }, new C { FloatValue = 32.2f });

            return a;
        }

        public static ComplexModel GenerateNulledModel()
        {
            var a = new ComplexModel();
            a.TestValue = null;
            a.NonPersisted = 0;
            a.Stuff = null;
            a.NestedValue = null;
            a.NestedArray = new B[2];
            a.NestedArray[0] = new B
            {
                IntValue = 0,
                StringValue = null,
                NestedArray = null
            };
            a.NestedArray[1] = null;
            a.AllTypes = null;
            a.SimpleDictionary.Add("key1", null);
            a.ComplexDictionary = null;

            return a;
        }

        public static NullableTypesModel GenerateNulledNullableModel()
        {
            var model = new NullableTypesModel();
            model.NullableFloat = null;
            model.NullableInt = null;
            return model;
        }

        public static NullableTypesModel GeneratePopulatedNullableModel()
        {
            var model = new NullableTypesModel();
            model.NullableFloat = 10.0f;
            model.NullableInt = 22;
            return model;
        }

        public static DynamicTypesModel GeneratePopulatedDynamicTypesModel()
        {
            var model = new DynamicTypesModel();
            model.DynamicNestedProperty = new E { IntValue = 10 };
            model.DynamicPrimitiveProperty = 12;

            model.DynamicList = new List<object>();
            model.DynamicList.Add(new E { IntValue = 22 });
            model.DynamicList.Add(new C { FloatValue = 25 });
            model.DynamicList.Add(20);

            model.DynamicArray = new object[]
            {
                new E { IntValue = 12 },
                new C { FloatValue = 54.2f }
            };

            model.DynamicEnumerable = new object[]
            {
                new E { IntValue = 45 },
                new C { FloatValue = 22.7f }
            };
            
            model.DynamicDictionary = new Dictionary<object, object>();
            model.DynamicDictionary.Add("key1", 62);
            model.DynamicDictionary.Add(new E{IntValue = 99}, 54);
            model.DynamicDictionary.Add(1, new C {FloatValue = 51.0f});
            return model;
        }

        public static NumericsTypesModel GenerateNumericsModel()
        {
            var model = new NumericsTypesModel();
            model.NullableVector2Value = model.Vector2Value = Vector2.One;
            model.NullableVector3Value = model.Vector3Value = Vector3.One;
            model.NullableVector4Value = model.Vector4Value = Vector4.One;
            model.NullableQuaternionValue = model.QuaternionValue = new Quaternion(1.0f, 1.0f, 1.0f, 1.0f);
            return model;
        }

        public static DynamicTypesModel GenerateNulledDynamicTypesModel()
        {
            var model = new DynamicTypesModel();
            model.DynamicNestedProperty = null;
            model.DynamicPrimitiveProperty = null;

            model.DynamicList = new List<object>();
            model.DynamicList.Add(new E() { IntValue = 22 });
            model.DynamicList.Add(null);
            model.DynamicList.Add(20);

            model.DynamicDictionary = new Dictionary<object, object>();
            model.DynamicDictionary.Add("key1", null);
            model.DynamicDictionary.Add(new E { IntValue = 99 }, null);
            model.DynamicDictionary.Add(1, null);
            return model;
        }

        public static void AssertPopulatedData(ComplexModel expected, ComplexModel actual)
        {
            var expectedObject = JObject.FromObject(expected);
            expectedObject.Property("NonPersisted").Remove();

            var actualObject = JObject.FromObject(actual);
            actualObject.Property("NonPersisted").Remove();
            
            Assert.AreEqual(expectedObject, actualObject);
        }

        public static void AssertNulledData(ComplexModel expected, ComplexModel actual)
        { Assert.AreEqual(actual, expected); }

        public static void AssertPopulatedDynamicTypesData(DynamicTypesModel expected, DynamicTypesModel actual)
        { Assert.AreEqual(actual, expected); }

        public static void AsserNulledDynamicTypesData(DynamicTypesModel expected, DynamicTypesModel actual)
        { Assert.AreEqual(actual, expected); }

        public static void AssertNullableModelData(NullableTypesModel expected, NullableTypesModel actual)
        { Assert.AreEqual(actual, expected); }
    }
}