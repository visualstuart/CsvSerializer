using System.Reflection;

namespace CsvSerialization
{
    public static class CsvSerializer
    {
        private const string QuotationMark = "\"";
        public static string Serialize(object value)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));

            IEnumerable<string?>? propertyValues = value
                .GetType()
                .GetProperties()
                .OrderBy(pi => (pi?.GetCustomAttribute<CsvPropertyAttribute>()?.Order))
                .Select(pi => RenderProperty(pi.GetValue(value, null)));

            return CommaSeparated(propertyValues);
        }

        /// <summary>
        /// Render unto Caesar...
        /// </summary>
        /// <param name="property">The property to render.</param>
        private static string RenderProperty(object? property)
        {
            // It isn't possible to statically express an enum type in C#,
            //  so this condition must be evaluated at runtime.
            if (property?.GetType().IsEnum ?? false)
            {
                return property.ToString() ?? string.Empty;
            }

            // switch on static types
            return property switch
            {
                bool b => b.ToString(),
                int i => i.ToString(),
                decimal d => d.ToString(),
                string s => Quoted(s),
                DateTime dt => Quoted(dt.ToString("yyyyMMdd")),
                _ => Quoted(property?.ToString()),
            };
        }

        private static string CommaSeparated(IEnumerable<string?> propertyValues) =>
            string.Join(",", propertyValues);

        private static string Quoted(string? value) =>
            QuotationMark + value + QuotationMark;
    }
}