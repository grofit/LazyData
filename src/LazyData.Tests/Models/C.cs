using LazyData.Attributes;

namespace LazyData.Tests.Models
{
    [Persist]
    public class C
    {
        [PersistData]
        public float FloatValue { get; set; }
    }
}