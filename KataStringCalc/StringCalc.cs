using System.Linq;

namespace KataStringCalc
{
    public class StringCalc
    {
        public int Add(string numbers)
        {
            if (string.IsNullOrWhiteSpace(numbers)) return 0;

            var result = numbers.Split(',').Select(n => int.Parse(n)).Sum();

            return result;
        }
    }
}
