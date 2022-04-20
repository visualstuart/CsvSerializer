using System;
using System.Diagnostics;
using Xunit;

namespace CsvSerialization.Tests
{
    public class CsvSerializerScaleTests
    {
        public class PocoWithManyProperties
        {
            public bool BoolProperty1 { get; set; }
            public int IntProperty1 { get; set; }
            public decimal DecimalProperty1 { get; set; }
            public string StringProperty1 { get; set; }
            public DateTime DateProperty1 { get; set; }

            public bool BoolProperty2 { get; set; }
            public int IntProperty2 { get; set; }
            public decimal DecimalProperty2 { get; set; }
            public string StringProperty2 { get; set; }
            public DateTime DateProperty2 { get; set; }

            public bool BoolProperty3 { get; set; }
            public int IntProperty3 { get; set; }
            public decimal DecimalProperty3 { get; set; }
            public string StringProperty3 { get; set; }
            public DateTime DateProperty3 { get; set; }

            public bool BoolProperty4 { get; set; }
            public int IntProperty4 { get; set; }
            public decimal DecimalProperty4 { get; set; }
            public string StringProperty4 { get; set; }
            public DateTime DateProperty4 { get; set; }

            public bool BoolProperty5 { get; set; }
            public int IntProperty5 { get; set; }
            public decimal DecimalProperty5 { get; set; }
            public string StringProperty5 { get; set; }
            public DateTime DateProperty5 { get; set; }

            public bool BoolProperty6 { get; set; }
            public int IntProperty6 { get; set; }
            public decimal DecimalProperty6 { get; set; }
            public string StringProperty6 { get; set; }
            public DateTime DateProperty6 { get; set; }

            public bool BoolProperty7 { get; set; }
            public int IntProperty7 { get; set; }
            public decimal DecimalProperty7 { get; set; }
            public string StringProperty7 { get; set; }
            public DateTime DateProperty7 { get; set; }

            public bool BoolProperty8 { get; set; }
            public int IntProperty8 { get; set; }
            public decimal DecimalProperty8 { get; set; }
            public string StringProperty8 { get; set; }
            public DateTime DateProperty8 { get; set; }

            public bool BoolProperty9 { get; set; }
            public int IntProperty9 { get; set; }
            public decimal DecimalProperty9 { get; set; }
            public string StringProperty9 { get; set; }
            public DateTime DateProperty9 { get; set; }

            public bool BoolProperty10 { get; set; }
            public int IntProperty10 { get; set; }
            public decimal DecimalProperty10 { get; set; }
            public string StringProperty10 { get; set; }
            public DateTime DateProperty10 { get; set; }

            public bool BoolProperty11 { get; set; }
            public int IntProperty11 { get; set; }
            public decimal DecimalProperty11 { get; set; }
            public string StringProperty11 { get; set; }
            public DateTime DateProperty11 { get; set; }

            public bool BoolProperty12 { get; set; }
            public int IntProperty12 { get; set; }
            public decimal DecimalProperty12 { get; set; }
            public string StringProperty12 { get; set; }
            public DateTime DateProperty12 { get; set; }

            public bool BoolProperty13 { get; set; }
            public int IntProperty13 { get; set; }
            public decimal DecimalProperty13 { get; set; }
            public string StringProperty13 { get; set; }
            public DateTime DateProperty13 { get; set; }

            public bool BoolProperty14 { get; set; }
            public int IntProperty14 { get; set; }
            public decimal DecimalProperty14 { get; set; }
            public string StringProperty14 { get; set; }
            public DateTime DateProperty14 { get; set; }

            public bool BoolProperty15 { get; set; }
            public int IntProperty15 { get; set; }
            public decimal DecimalProperty15 { get; set; }
            public string StringProperty15 { get; set; }
            public DateTime DateProperty15 { get; set; }

            public bool BoolProperty16 { get; set; }
            public int IntProperty16 { get; set; }
            public decimal DecimalProperty16 { get; set; }
            public string StringProperty16 { get; set; }
            public DateTime DateProperty16 { get; set; }

            public bool BoolProperty17 { get; set; }
            public int IntProperty17 { get; set; }
            public decimal DecimalProperty17 { get; set; }
            public string StringProperty17 { get; set; }
            public DateTime DateProperty17 { get; set; }

            public bool BoolProperty18 { get; set; }
            public int IntProperty18 { get; set; }
            public decimal DecimalProperty18 { get; set; }
            public string StringProperty18 { get; set; }
            public DateTime DateProperty18 { get; set; }

            public bool BoolProperty19 { get; set; }
            public int IntProperty19 { get; set; }
            public decimal DecimalProperty19 { get; set; }
            public string StringProperty19 { get; set; }
            public DateTime DateProperty19 { get; set; }

            public bool BoolProperty20 { get; set; }
            public int IntProperty20 { get; set; }
            public decimal DecimalProperty20 { get; set; }
            public string StringProperty20 { get; set; }
            public DateTime DateProperty20 { get; set; }
        }

        /// <summary>
        /// Simplistic performance testing executing the test runner.
        /// Serialized 1,000,000 records of 100 properties of mixed types in 32 seconds.
        /// </summary>
        //[Fact]
        public void When_serializing_a_PocoWithCustomThresholdPercentage_instance_with_percentage_in_no_range_then_the_result_is_correct()
        {
            var poco = new PocoWithManyProperties();
            var propertyInfos = poco.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType == typeof(bool))
                {
                    propertyInfo.SetValue(poco, true);
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    propertyInfo.SetValue(poco, 1);
                }
                else if (propertyInfo.PropertyType == typeof(decimal))
                {
                    propertyInfo.SetValue(poco, 1.7m);
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(poco, "Acer");
                }
                else if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    propertyInfo.SetValue(poco, new DateTime(2022, 4, 20));
                }
                else
                {
                    throw new InvalidOperationException($"Unexpected property type in {nameof(poco)}.");
                }
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                _ = CsvSerializer.Serialize(poco);
            }

            stopwatch.Stop();
            Assert.False(true, stopwatch.Elapsed.ToString());
        }
    }
}
