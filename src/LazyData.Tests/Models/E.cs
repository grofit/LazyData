using LazyData.Attributes;

namespace LazyData.Tests.Models
{
    [Persist]
    public class E
    {
        [PersistData]
        public int IntValue { get; set; }
    }
}