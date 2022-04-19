using System.Reflection;

namespace CsvSerialization
{
    public static class CsvSerializer
    {
        private const string QuotationMark = "\"";
        public static string Serialize(object value)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));

            var type = value.GetType();
            PropertyInfo[]? propertyInfos = type.GetProperties();
            IEnumerable<string?>? propertyValues = propertyInfos
                .Select(pi => Quote(pi?.GetValue(value, null)?.ToString()));
            return string.Join(",", propertyValues);
        }

        private static string Quote(string? value) =>
            QuotationMark + value + QuotationMark;
    }
}