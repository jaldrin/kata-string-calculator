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
    }
}
