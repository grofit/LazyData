using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace LazyData.Tests.Extensions
{
    public class AssertExtensions : Assert
    {
        public static void AreEqual(object expected, object actual)
        {
            var expectedStr = JsonConvert.SerializeObject(expected);
            var actualStr = JsonConvert.SerializeObject(actual);
            Equal(expectedStr, actualStr);
        }
        
        public static void AreEqual(JObject expected, JObject actual)
        {
            var expectedStr = expected.ToString();
            var actualStr = actual.ToString();
            Equal(expectedStr, actualStr);
        }
        
        public static void IsRuntimeType<T>(Type type)
        { Equal(typeof(T), type); }
    }
}