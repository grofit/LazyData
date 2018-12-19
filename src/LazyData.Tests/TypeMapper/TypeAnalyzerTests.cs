using System;
using System.Collections;
using System.Collections.Generic;
using LazyData.Mappings.Types;
using LazyData.Tests.Models;
using Xunit;

namespace LazyData.Tests.TypeMapper
{   
    public class TypeAnalyzerTests
    {
        [Theory]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(IList<>), true)]
        [InlineData(typeof(ICollection<>), true)]
        [InlineData(typeof(IEnumerable<>), true)]
        [InlineData(typeof(IDictionary<,>), true)]
        [InlineData(typeof(IEnumerable), false)]
        [InlineData(typeof(List<>), true)]
        [InlineData(typeof(CustomList), false)]
        public void should_correctly_identify_generic_collection(Type collectionType, bool shouldMatch)
        {
            var typeAnalyzer = new TypeAnalyzer();
            var isGenericCollection = typeAnalyzer.IsGenericCollection(collectionType);
            
            Assert.Equal(isGenericCollection, shouldMatch);
        }
        
        [Theory]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(IList<>), true)]
        [InlineData(typeof(ICollection<>), true)]
        [InlineData(typeof(IEnumerable<>), false)]
        [InlineData(typeof(IDictionary<,>), true)]
        [InlineData(typeof(IEnumerable), false)]
        [InlineData(typeof(List<>), true)]
        [InlineData(typeof(CustomList), true)]
        public void should_correctly_identify_implemented_generic_collection(Type collectionType, bool shouldMatch)
        {
            var typeAnalyzer = new TypeAnalyzer();
            var isGenericCollection = typeAnalyzer.HasImplementedGenericCollection(collectionType);
            
            Assert.Equal(isGenericCollection, shouldMatch);
        }
        
        [Theory]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(IList<>), false)]
        [InlineData(typeof(ICollection<>), false)]
        [InlineData(typeof(IEnumerable<>), false)]
        [InlineData(typeof(IEnumerable), false)]
        [InlineData(typeof(IDictionary<,>), true)]
        [InlineData(typeof(Dictionary<,>), true)]
        [InlineData(typeof(CustomDictionary), false)]
        public void should_correctly_identify_generic_dictionary(Type collectionType, bool shouldMatch)
        {
            var typeAnalyzer = new TypeAnalyzer();
            var isGenericCollection = typeAnalyzer.IsGenericDictionary(collectionType);
            
            Assert.Equal(isGenericCollection, shouldMatch);
        }
        
        [Theory]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(IList<>), false)]
        [InlineData(typeof(ICollection<>), false)]
        [InlineData(typeof(IEnumerable<>), false)]
        [InlineData(typeof(IEnumerable), false)]
        [InlineData(typeof(IDictionary<,>), false)]
        [InlineData(typeof(Dictionary<,>), true)]
        [InlineData(typeof(CustomDictionary), true)]
        public void should_correctly_identify_implemented_generic_dictionary(Type collectionType, bool shouldMatch)
        {
            var typeAnalyzer = new TypeAnalyzer();
            var isGenericCollection = typeAnalyzer.HasImplementedGenericDictionary(collectionType);
            
            Assert.Equal(isGenericCollection, shouldMatch);
        }
    }
}