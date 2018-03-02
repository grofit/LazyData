using System;
using LazyData.Attributes;

namespace LazyData.Tests.Models
{
    public class CommonTypesModel
    {
        [PersistData] public byte ByteValue { get; set; }
        [PersistData] public short ShortValue { get; set; }
        [PersistData] public int IntValue { get; set; }
        [PersistData] public long LongValue { get; set; }
        [PersistData] public Guid GuidValue { get; set; }
        [PersistData] public DateTime DateTimeValue { get; set; }
        [PersistData] public TimeSpan TimeSpanValue { get; set; }
        [PersistData] public SomeTypes SomeType { get; set; }
    }
}