using System.Linq;

namespace KataStringCalc
{
    public class StringCalc
    {
        public char[] delimiters = { ',', '\n' };

        public int Add(string numbers)
        {
            if (string.IsNullOrWhiteSpace(numbers)) return 0;

            var result = numbers.Split(delimiters)
                                .Select(n => int.Parse(n))
                                .Sum();

            return result;
        }
    }
}
