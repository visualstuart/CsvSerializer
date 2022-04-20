using System;
using Xunit;

namespace CsvSerialization.Tests
{
    public class CsvSerializerTests
    {
        public class PocoWithOneString
        {
            public string? Value { get; set; }
        }
        
        [Fact]
        public void When_serializing_a_PocoWithOneString_instance_then_the_result_is_correct()
        {
            string value = Guid.NewGuid().ToString();

            var poco = new PocoWithOneString { Value = value };
            string csv = CsvSerializer.Serialize(poco);
            Assert.Equal($"\"{value}\"", csv);
        }

        public class PocoWithTwoStrings
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
        }

        [Fact]
        public void When_serializing_a_PocoWithTwoStrings_instance_then_the_result_is_correct()
        {
            string firstName = "Paul";
            string lastName = "Erdos";

            var poco = 
                new PocoWithTwoStrings 
                { 
                    FirstName = firstName, 
                    LastName = lastName 
                };
            string csv = CsvSerializer.Serialize(poco);
            Assert.Equal($"\"{firstName}\",\"{lastName}\"", csv);
        }

        public class PocoWithOrdering
        {
            [CsvProperty(Order = 2)]
            public string? FirstName { get; set; }

            [CsvProperty(Order = 1)]
            public string? LastName { get; set; }
        }

        [Fact]
        public void When_serializing_a_PocoWithOrdering_instance_then_the_result_is_correct()
        {
            string firstName = "Paul";
            string lastName = "Erdos";

            var poco = new PocoWithOrdering { FirstName = firstName, LastName = lastName };
            string csv = CsvSerializer.Serialize(poco);
            Assert.Equal($"\"{lastName}\",\"{firstName}\"", csv);
        }


        public class PocoWithInt
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        [Fact]
        public void When_serializing_a_PocoWithInt_instance_then_the_result_is_correct()
        {
            string name = "Hilbert";
            int id = 37;

            var poco = new PocoWithInt { Id = id, Name = name };
            string csv = CsvSerializer.Serialize(poco);
            Assert.Equal($"{id},\"{name}\"", csv);
        }

        public class PocoWithDate
        {
            public string? Name { get; set; }
            public DateTime ArrivalDate { get; set; }
        }

        [Fact]
        public void When_serializing_a_PocoWithDate_instance_then_the_result_is_correct()
        {
            string name = "Clarance";
            int year = 2017;
            int month = 7;
            int day = 2;
            string expectedDate = $"{year}{month:d2}{day:d2}";

            var poco = 
                new PocoWithDate 
                { 
                    Name = name, 
                    ArrivalDate = new DateTime(year, month, day) 
                };
            string csv = CsvSerializer.Serialize(poco);
            Assert.Equal($"\"{name}\",\"{expectedDate}\"", csv);
        }

        public class PocoWithBool
        {
            public string? Description { get; set; }
            public bool Available { get; set; }
        }

        [Fact]
        public void When_serializing_a_PocoWithBool_instance_then_the_result_is_correct()
        {
            string description = "Graham Crackers";
            bool available = false;

            var poco =
                new PocoWithBool
                {
                    Description = description,
                    Available = available
                };
            string csv = CsvSerializer.Serialize(poco);
            Assert.Equal($"\"{description}\",{available}", csv);
        }

        public enum YesNoNA
        {
            Yes,
            No,
            NA
        }

        public class PocoWithEnum
        {
            public string? Name { get; set; }
            public YesNoNA Eligible { get; set; }
        }

        [Fact]
        public void When_serializing_a_PocoWithEnum_instance_then_the_result_is_correct()
        {
            string name = "Pickles";
            YesNoNA eligible = YesNoNA.NA;

            var poco =
                new PocoWithEnum
                {
                    Name = name,
                    Eligible = eligible
                };
            string csv = CsvSerializer.Serialize(poco);
            Assert.Equal($"\"{name}\",{eligible}", csv);
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Thresholded range of values

        public class DecimalRange
        {
            private readonly decimal start;
            private readonly decimal end;
            private readonly string description;

            public DecimalRange(decimal start, decimal end, string description)
            {
                this.start = start;
                this.end = end;
                this.description = description;
            }

            public bool IsInRange(decimal value) =>
                value >= start && value < end;

            public override string ToString() => description;
        }

        public class CustomThresholdPercentage
        {
            private readonly decimal numerator;
            private readonly decimal denominator;

            private readonly DecimalRange[] ranges =
            {
                new DecimalRange(0m, 0.5m, "<50%"),
                // note that 50 to 60% is not in a range
                new DecimalRange(0.6m, 1m, "60% - 100%"),
                new DecimalRange(1m, decimal.MaxValue, ">100%")
            };

            public CustomThresholdPercentage(decimal numerator, decimal denominator)
            {
                if (denominator == 0)
                {
                    throw new InvalidOperationException("Error: division by zero.");
                }

                this.numerator = numerator;
                this.denominator = denominator;
            }

            public override string ToString()
            {
                decimal ratio = numerator / denominator;
                foreach (var range in ranges)
                {
                    if (range.IsInRange(ratio))
                    {
                        return range.ToString();
                    }
                }
                return (100*ratio).ToString("N0") + "%";
            }
        }

        public class PocoWithCustomThresholdPercentage
        {
            public decimal Numerator { get; set; }
            public decimal Denominator { get; set; }
            public CustomThresholdPercentage? CustomThresholdPercentage { get; set; }
        }

        [Fact]
        public void When_serializing_a_PocoWithCustomThresholdPercentage_instance_with_percentage_in_a_range_then_the_result_is_correct()
        {
            decimal numerator = 5;
            decimal denominator = 50;

            var poco =
                new PocoWithCustomThresholdPercentage
                {
                    Numerator = numerator,
                    Denominator = denominator,
                    CustomThresholdPercentage = 
                        new CustomThresholdPercentage(numerator, denominator)
                };
            string csv = CsvSerializer.Serialize(poco);
            Assert.Equal($"{numerator},{denominator},\"<50%\"", csv);
        }

        [Fact]
        public void When_serializing_a_PocoWithCustomThresholdPercentage_instance_with_percentage_in_no_range_then_the_result_is_correct()
        {
            decimal numerator = 26;
            decimal denominator = 50;

            var poco =
                new PocoWithCustomThresholdPercentage
                {
                    Numerator = numerator,
                    Denominator = denominator,
                    CustomThresholdPercentage =
                        new CustomThresholdPercentage(numerator, denominator)
                };
            string csv = CsvSerializer.Serialize(poco);
            Assert.Equal($"{numerator},{denominator},\"52%\"", csv);
        }
    }
}