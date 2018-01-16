using LazyData.Attributes;

namespace LazyData.Tests.Models
{
    public class B
    {
        [PersistData]
        public string StringValue { get; set; }

        [PersistData]
        public int  IntValue { get; set; }

        [PersistData]
        public C[] NestedArray { get; set; }
    }
}