using System;
using System.Collections.Generic;
using LazyData.Mappings;
using LazyData.Tests.Models;
using Assert = LazyData.Tests.Extensions.AssertExtensions;

namespace LazyData.Tests.Helpers
{
    public static class TypeAssertionHelper
    {
        public static void AssertComplexModel(IList<Mapping> mappings)
        {
            Assert.Equal(9, mappings.Count);

            var testValueMapping = mappings[0] as PropertyMapping;
            Assert.IsRuntimeType<string>(testValueMapping.Type);
            Assert.Equal("TestValue", testValueMapping.LocalName);
            Assert.Empty(testValueMapping.MetaData);

            var nestedValueMapping = mappings[1] as NestedMapping;
            Assert.IsRuntimeType<B>(nestedValueMapping.Type);
            Assert.Equal("NestedValue", nestedValueMapping.LocalName);
            Assert.False(nestedValueMapping.IsDynamicType);
            Assert.Empty(nestedValueMapping.MetaData);
            AssertBModel(nestedValueMapping.InternalMappings);

            var nestedArrayMapping = mappings[2] as CollectionMapping;
            Assert.IsRuntimeType<B[]>(nestedArrayMapping.Type);
            Assert.Equal("NestedArray", nestedArrayMapping.LocalName);
            Assert.False(nestedArrayMapping.IsElementDynamicType);
            Assert.Empty(nestedArrayMapping.MetaData);
            AssertBModel(nestedArrayMapping.InternalMappings);

            var stuffMapping = mappings[3] as CollectionMapping;
            Assert.IsRuntimeType<IList<string>>(stuffMapping.Type);
            Assert.Equal("Stuff", stuffMapping.LocalName);
            Assert.False(stuffMapping.IsElementDynamicType);
            Assert.Empty(stuffMapping.MetaData);
            Assert.IsRuntimeType<string>(stuffMapping.CollectionType);
            Assert.Equal(0, stuffMapping.InternalMappings.Count);
            
            var customListMapping = mappings[4] as CollectionMapping;
            Assert.IsRuntimeType<CustomList>(customListMapping.Type);
            Assert.Equal("CustomList", customListMapping.LocalName);
            Assert.False(customListMapping.IsElementDynamicType);
            Assert.Empty(customListMapping.MetaData);
            Assert.IsRuntimeType<int>(customListMapping.CollectionType);
            Assert.Equal(0, customListMapping.InternalMappings.Count);

            var allTypesMapping = mappings[5] as NestedMapping;
            Assert.IsRuntimeType<CommonTypesModel>(allTypesMapping.Type);
            Assert.Equal("AllTypes", allTypesMapping.LocalName);
            Assert.False(allTypesMapping.IsDynamicType);
            Assert.Empty(allTypesMapping.MetaData);
            AssertDModel(allTypesMapping.InternalMappings);

            var simpleDictionaryMapping = mappings[6] as DictionaryMapping;
            Assert.IsRuntimeType<IDictionary<string, string>>(simpleDictionaryMapping.Type);
            Assert.Equal("SimpleDictionary", simpleDictionaryMapping.LocalName);
            Assert.False(simpleDictionaryMapping.IsKeyDynamicType);
            Assert.False(simpleDictionaryMapping.IsValueDynamicType);
            Assert.Empty(simpleDictionaryMapping.MetaData);
            Assert.IsRuntimeType<string>(simpleDictionaryMapping.KeyType);
            Assert.IsRuntimeType<string>(simpleDictionaryMapping.ValueType);
            Assert.Equal(0, simpleDictionaryMapping.KeyMappings.Count);
            Assert.Equal(0, simpleDictionaryMapping.ValueMappings.Count);
            
            var complexDictionaryMapping = mappings[7] as DictionaryMapping;
            Assert.IsRuntimeType<IDictionary<E, C>>(complexDictionaryMapping.Type);
            Assert.Equal("ComplexDictionary", complexDictionaryMapping.LocalName);
            Assert.False(complexDictionaryMapping.IsKeyDynamicType);
            Assert.False(complexDictionaryMapping.IsValueDynamicType);
            Assert.Empty(complexDictionaryMapping.MetaData);
            Assert.IsRuntimeType<E>(complexDictionaryMapping.KeyType);
            Assert.IsRuntimeType<C>(complexDictionaryMapping.ValueType);
            Assert.Equal(1, complexDictionaryMapping.KeyMappings.Count);
            Assert.Equal(1, complexDictionaryMapping.ValueMappings.Count);
            AssertEModel(complexDictionaryMapping.KeyMappings);
            AssertCModel(complexDictionaryMapping.ValueMappings);
            
            var customDictionaryMapping = mappings[8] as DictionaryMapping;
            Assert.IsRuntimeType<CustomDictionary>(customDictionaryMapping.Type);
            Assert.Equal("CustomDictionary", customDictionaryMapping.LocalName);
            Assert.False(customDictionaryMapping.IsKeyDynamicType);
            Assert.False(customDictionaryMapping.IsValueDynamicType);
            Assert.Empty(customDictionaryMapping.MetaData);
            Assert.IsRuntimeType<int>(customDictionaryMapping.KeyType);
            Assert.IsRuntimeType<int>(customDictionaryMapping.ValueType);
            Assert.Equal(0, customDictionaryMapping.KeyMappings.Count);
            Assert.Equal(0, customDictionaryMapping.ValueMappings.Count);
        }

        public static void AssertBModel(IList<Mapping> mappings)
        {
            Assert.Equal(3, mappings.Count);

            var stringValueMapping = mappings[0] as PropertyMapping;
            Assert.NotNull(stringValueMapping);
            Assert.IsRuntimeType<string>(stringValueMapping.Type);
            Assert.Equal("StringValue", stringValueMapping.LocalName);

            var intValueMapping = mappings[1] as PropertyMapping;
            Assert.NotNull(intValueMapping);
            Assert.IsRuntimeType<int>(intValueMapping.Type);
            Assert.Equal("IntValue", intValueMapping.LocalName);

            var nestedArrayMapping = mappings[2] as CollectionMapping;
            Assert.NotNull(nestedArrayMapping);
            Assert.IsRuntimeType<C[]>(nestedArrayMapping.Type);
            Assert.Equal("NestedArray", nestedArrayMapping.LocalName);

            AssertCModel(nestedArrayMapping.InternalMappings);
        }

        public static void AssertCModel(IList<Mapping> mappings)
        {
            Assert.Equal(1, mappings.Count);

            var floatValueMapping = mappings[0] as PropertyMapping;
            Assert.NotNull(floatValueMapping);
            Assert.IsRuntimeType<float>(floatValueMapping.Type);
            Assert.Equal("FloatValue", floatValueMapping.LocalName);
        }

        public static void AssertDModel(IList<Mapping> mappings)
        {
            Assert.Equal(8, mappings.Count);

            var byteValueMapping = mappings[0] as PropertyMapping;
            Assert.NotNull(byteValueMapping);
            Assert.IsRuntimeType<byte>(byteValueMapping.Type);
            Assert.Equal("ByteValue", byteValueMapping.LocalName);

            var shortValueMapping = mappings[1] as PropertyMapping;
            Assert.NotNull(shortValueMapping);
            Assert.IsRuntimeType<short>(shortValueMapping.Type);
            Assert.Equal("ShortValue", shortValueMapping.LocalName);

            var intValueMapping = mappings[2] as PropertyMapping;
            Assert.NotNull(intValueMapping);
            Assert.IsRuntimeType<int>(intValueMapping.Type);
            Assert.Equal("IntValue", intValueMapping.LocalName);

            var longValueMapping = mappings[3] as PropertyMapping;
            Assert.NotNull(longValueMapping);
            Assert.IsRuntimeType<long>(longValueMapping.Type);
            Assert.Equal("LongValue", longValueMapping.LocalName);

            var guidValueMapping = mappings[4] as PropertyMapping;
            Assert.NotNull(guidValueMapping);
            Assert.IsRuntimeType<Guid>(guidValueMapping.Type);
            Assert.Equal("GuidValue", guidValueMapping.LocalName);

            var dateTimeValueMapping = mappings[5] as PropertyMapping;
            Assert.NotNull(dateTimeValueMapping);
            Assert.IsRuntimeType<DateTime>(dateTimeValueMapping.Type);
            Assert.Equal("DateTimeValue", dateTimeValueMapping.LocalName);

            var timespanValueMapping = mappings[6] as PropertyMapping;
            Assert.NotNull(timespanValueMapping);
            Assert.IsRuntimeType<TimeSpan>(timespanValueMapping.Type);
            Assert.Equal("TimeSpanValue", timespanValueMapping.LocalName);

            var someTypeMapping = mappings[7] as PropertyMapping;
            Assert.NotNull(someTypeMapping);
            Assert.IsRuntimeType<SomeTypes>(someTypeMapping.Type);
            Assert.Equal("SomeType", someTypeMapping.LocalName);
        }

        public static void AssertEModel(IList<Mapping> mappings)
        {
            Assert.Equal(1, mappings.Count);

            var intValueMapping = mappings[0] as PropertyMapping;
            Assert.NotNull(intValueMapping);
            Assert.IsRuntimeType<int>(intValueMapping.Type);
            Assert.Equal("IntValue", intValueMapping.LocalName);
        }

        public static void AssertDynamicModel(IList<Mapping> mappings)
        {
            Assert.Equal(6, mappings.Count);

            var dynamicNestedPropertyMapping = mappings[0] as NestedMapping;
            Assert.NotNull(dynamicNestedPropertyMapping);
            Assert.IsRuntimeType<object>(dynamicNestedPropertyMapping.Type);
            Assert.True(dynamicNestedPropertyMapping.IsDynamicType);
            Assert.Empty(dynamicNestedPropertyMapping.InternalMappings);
            Assert.Equal("DynamicNestedProperty", dynamicNestedPropertyMapping.LocalName);

            var dynamicPrimitivePropertyMapping = mappings[1] as NestedMapping;
            Assert.NotNull(dynamicPrimitivePropertyMapping);
            Assert.IsRuntimeType<object>(dynamicPrimitivePropertyMapping.Type);
            Assert.True(dynamicPrimitivePropertyMapping.IsDynamicType);
            Assert.Empty(dynamicPrimitivePropertyMapping.InternalMappings);
            Assert.Equal("DynamicPrimitiveProperty", dynamicPrimitivePropertyMapping.LocalName);

            var dynamicListMapping = mappings[2] as CollectionMapping;
            Assert.NotNull(dynamicListMapping);
            Assert.IsRuntimeType<IList<object>>(dynamicListMapping.Type);
            Assert.IsRuntimeType<object>(dynamicListMapping.CollectionType);
            Assert.True(dynamicListMapping.IsElementDynamicType);
            Assert.Empty(dynamicListMapping.InternalMappings);
            Assert.Equal("DynamicList", dynamicListMapping.LocalName);
            
            var dynamicArrayMapping = mappings[3] as CollectionMapping;
            Assert.NotNull(dynamicArrayMapping);
            Assert.IsRuntimeType<object[]>(dynamicArrayMapping.Type);
            Assert.IsRuntimeType<object>(dynamicArrayMapping.CollectionType);
            Assert.True(dynamicArrayMapping.IsElementDynamicType);
            Assert.Empty(dynamicArrayMapping.InternalMappings);
            Assert.Equal("DynamicArray", dynamicArrayMapping.LocalName);
            
            var dynamicEnumerableMapping = mappings[4] as CollectionMapping;
            Assert.NotNull(dynamicEnumerableMapping);
            Assert.IsRuntimeType<IEnumerable<object>>(dynamicEnumerableMapping.Type);
            Assert.IsRuntimeType<object>(dynamicEnumerableMapping.CollectionType);
            Assert.True(dynamicEnumerableMapping.IsElementDynamicType);
            Assert.Empty(dynamicEnumerableMapping.InternalMappings);
            Assert.Equal("DynamicEnumerable", dynamicEnumerableMapping.LocalName);

            var dynamicDictionaryMapping = mappings[5] as DictionaryMapping;
            Assert.NotNull(dynamicDictionaryMapping);
            Assert.IsRuntimeType<IDictionary<object, object>>(dynamicDictionaryMapping.Type);
            Assert.IsRuntimeType<object>(dynamicDictionaryMapping.KeyType);
            Assert.IsRuntimeType<object>(dynamicDictionaryMapping.ValueType);
            Assert.True(dynamicDictionaryMapping.IsKeyDynamicType);
            Assert.True(dynamicDictionaryMapping.IsValueDynamicType);
            Assert.Empty(dynamicDictionaryMapping.KeyMappings);
            Assert.Empty(dynamicDictionaryMapping.ValueMappings);
            Assert.Equal("DynamicDictionary", dynamicDictionaryMapping.LocalName);
        }
    }
}