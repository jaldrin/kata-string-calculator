using Shouldly;
using Xunit;

namespace KataStringCalc
{
    public class StringCalc_Add
    {
        [Fact]
        public void EmptyStringShouldReturnZero()
        {
            var target = new StringCalc();
            var actual = target.Add("");
            actual.ShouldBe(0);
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("2", 2)]
        public void NumbersCanHandleASingleNumber(string stringValue, int expected)
        {
            var target = new StringCalc();
            var actual = target.Add(stringValue);
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData("1,2", 3)]
        [InlineData("2,3,4", 9)]
        public void NumbersCanHandleMultipleNumbers(string stringValue, int expected)
        {
            var target = new StringCalc();
            var actual = target.Add(stringValue);
            actual.ShouldBe(expected);
        }
    }
}
