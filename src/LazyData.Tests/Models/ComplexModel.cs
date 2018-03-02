using System.Collections.Generic;
using LazyData.Attributes;

namespace LazyData.Tests.Models
{
    [Persist]
    public class ComplexModel
    {
        [PersistData]
        public string TestValue { get; set; }

        public int NonPersisted { get; set; }

        [PersistData]
        public B NestedValue { get; set; }

        [PersistData]
        public B[] NestedArray { get; set; }
        
        [PersistData]
        public IList<string> Stuff { get; set; }

        [PersistData]
        public CommonTypesModel AllTypes { get; set; }

        [PersistData]
        public IDictionary<string, string> SimpleDictionary { get; set; }

        [PersistData]
        public IDictionary<E, C> ComplexDictionary { get; set; }

        public ComplexModel()
        {
            Stuff = new List<string>();
            SimpleDictionary = new Dictionary<string, string>();
            ComplexDictionary = new Dictionary<E, C>();
        }
    }
}
