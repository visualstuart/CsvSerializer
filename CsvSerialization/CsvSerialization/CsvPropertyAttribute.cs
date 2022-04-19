namespace CsvSerialization
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CsvPropertyAttribute : Attribute
    {
        public int Order { get; set; }
    }
}