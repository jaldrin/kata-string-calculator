using System;
using System.Collections.Generic;
using System.Linq;

namespace KataStringCalc
{
    public class StringCalc
    {
        public List<char> delimiters = new() { ',', '\n' };

        public int Add(string numbers)
        {
            if (string.IsNullOrWhiteSpace(numbers)) return 0;

            var numberString = numbers;
            if (numberString.StartsWith("//"))
            {
                var delimiterString = numberString.Split('\n')
                                                  .First();
                numberString = numberString.Substring(delimiterString.Length + 1);

                var delimiter = delimiterString.Split('/')
                                               .Skip(2)
                                               .First();
                delimiters.Add(char.Parse(delimiter));
            }

            RejectNegativeNumbers(numberString);

            var result = numberString.Split(delimiters.ToArray())
                                     .Select(n => int.Parse(n))
                                     .Where(n => n <= 1000)
                                     .Sum();

            return result;
        }

        private void RejectNegativeNumbers(string numberString)
        {
            var negativeNumbers = numberString.Split(delimiters.ToArray())
                                              .Select(n => int.Parse(n))
                                              .Where(n => n < 0);
            if (negativeNumbers.Any())
            {
                var numberList = string.Join(',', negativeNumbers);
                throw new ArgumentException($"Negatives not allowed: {numberList}");
            }
        }
    }
}
