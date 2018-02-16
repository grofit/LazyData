namespace LazyData.Tests.Models
{
    public class CyclicA
    {
        public CyclicB References { get; set; }
    }

    public class CyclicB
    {
        public CyclicA References { get; set; }
    }
}