namespace LazyData.Serialization
{
    public interface ISerializer
    {
        DataObject Serialize(object data, bool persistType = false);
    }
}