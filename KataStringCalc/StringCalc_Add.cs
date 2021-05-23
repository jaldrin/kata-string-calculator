using Shouldly;
using System;
using Xunit;

namespace KataStringCalc
{
    public class StringCalc_Add
    {
        private readonly StringCalc target = new();

        [Fact]
        public void EmptyStringShouldReturnZero()
        {
            var actual = target.Add("");
            actual.ShouldBe(0);
        }

        [Theory]
        [InlineData("1,\n", "Input string was not in a correct format.")]
        public void StringMustBeProperlyFormatted(string stringValue, string expected)
        {
            var ex = Assert.ThrowsAny<Exception>(() => target.Add(stringValue));
            ex.Message.ShouldBe(expected);
            ex.ShouldBeOfType<FormatException>();
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("2", 2)]
        public void NumbersCanHandleASingleNumber(string stringValue, int expected)
        {
            var actual = target.Add(stringValue);
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData("1,2", 3)]
        [InlineData("2,3,4", 9)]
        public void NumbersCanHandleMultipleNumbers(string stringValue, int expected)
        {
            var actual = target.Add(stringValue);
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData("1\n2,3", 6)]
        [InlineData("1,2\n3", 6)]
        public void NumbersCanHandleNewLines(string stringValue, int expected)
        {
            var actual = target.Add(stringValue);
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData("//;\n1;2", 3)]
        [InlineData("//;\n1;2\n3,4", 10)]
        public void NumbersCanHandleDifferentDelimiter(string stringValue, int expected)
        {
            var actual = target.Add(stringValue);
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData("-1,2", "Negatives not allowed: -1")]
        [InlineData("2,-4,3,-5", "Negatives not allowed: -4,-5")]
        public void NegativeNumbersThrowException(string stringValue, string expected)
        {
            var ex = Assert.Throws<ArgumentException>(() => target.Add(stringValue));
            ex.Message.ShouldBe(expected);
        }

        [Theory]
        [InlineData("1001,2", 2)]
        [InlineData("1000,2", 1002)]    // Edge case
        public void NumbersGreaterThan1000ShouldBeIgnored(string stringValue, int expected)
        {
            var actual = target.Add(stringValue);
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData("//[|||]\n1|||2|||3", 6)]
        [InlineData("//[|||]\n1|||2,3\n4", 10)]     // Mixed edge case
        public void DelimitersCanBeAnyLength(string stringValue, int expected)
        {
            var actual = target.Add(stringValue);
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData("//[|][%]\n1|2%3", 6)]
        [InlineData("//[|][%]\n1|2%3\n4,5", 15)]     // Mixed edge case
        public void AllowMultipleDelimiters(string stringValue, int expected)
        {
            var actual = target.Add(stringValue);
            actual.ShouldBe(expected);
        }
    }
}
