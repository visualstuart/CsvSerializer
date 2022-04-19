using System;
using Xunit;

namespace CsvSerialization.Tests
{
    public class CsvSerializerTests
    {
        public class Poco1
        {
            public string? Value { get; set; }
        }
        
        [Fact]
        public void When_serializing_a_Poco1_instance_then_the_result_is_correct()
        {
            string value = Guid.NewGuid().ToString();

            Poco1 poco = new Poco1 { Value = value };
            string csv = CsvSerializer.Serialize(poco);
            Assert.Equal($"\"{value}\"", csv);
        }

        public class Poco2
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
        }

        [Fact]
        public void When_serializing_a_Poco2_instance_then_the_result_is_correct()
        {
            string firstName = "Paul";
            string lastName = "Erdos";

            Poco2 poco = new Poco2 { FirstName = firstName, LastName = lastName };
            string csv = CsvSerializer.Serialize(poco);
            Assert.Equal($"\"{firstName}\",\"{lastName}\"", csv);
        }
    }
}