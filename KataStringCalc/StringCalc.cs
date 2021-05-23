using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace KataStringCalc
{
    public class StringCalc
    {
        /// <summary>
        /// See https://regex101.com/r/hzYaRl/1 for explanation of pattern
        /// </summary>
        private const string regexPattern = @"\[(.+?)\]";
        public List<string> delimiters = new() { ",", "\n" };

        public int Add(string numbers)
        {
            if (string.IsNullOrWhiteSpace(numbers)) return 0;

            var numberString = ParseDelimiters(numbers);

            RejectNegativeNumbers(numberString);

            var result = numberString.Split(delimiters.ToArray(), StringSplitOptions.None)
                                     .Select(n => int.Parse(n))
                                     .Where(n => n <= 1000)
                                     .Sum();

            return result;
        }

        private string ParseDelimiters(string numbers)
        {
            var numberString = numbers;
            if (numberString.StartsWith("//"))
            {
                var delimiterString = numberString.Split('\n')
                                                  .First();
                numberString = numberString[(delimiterString.Length + 1)..];

                var matches = Regex.Matches(delimiterString, regexPattern);
                if (matches.Any())
                {
                    foreach (var match in matches.ToList())
                    {
                        var delimiter = match.Groups[1].Value;
                        delimiters.Add(delimiter);
                    }
                }
                else
                {
                    var delimiter = delimiterString.Split('/')
                                                   .Skip(2)
                                                   .First();
                    delimiters.Add(delimiter);
                }
            }

            return numberString;
        }

        private void RejectNegativeNumbers(string numberString)
        {
            var negativeNumbers = numberString.Split(delimiters.ToArray(), StringSplitOptions.None)
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
