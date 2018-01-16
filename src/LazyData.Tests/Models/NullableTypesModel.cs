using LazyData.Attributes;

namespace LazyData.Tests.Models
{
    [Persist]
    public class NullableTypesModel
    {
        [PersistData]
        public int? NullableInt { get; set; }

        [PersistData]
        public float? NullableFloat { get; set; }
    }
}