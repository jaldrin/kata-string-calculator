using Shouldly;
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
    }
}
