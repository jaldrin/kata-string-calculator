using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace StringDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            //StringConversion();
            //StringAsArray();
            //EscapeString();
            //AppendingStrings();
            //InterpolationAndLiteral();
            //StringBuilderDemo();
            WorkingWithArrays();
        }

        private static void StringConversion()
        {
            string testString = "tHis iS tHe FBI Calling!";
            TextInfo currentTextInfo = CultureInfo.CurrentCulture.TextInfo;
            TextInfo englishTextInfo = new CultureInfo("en-US", false).TextInfo;
            string result;

            result = testString.ToLower();
            Console.WriteLine(result);

            result = testString.ToUpper();
            Console.WriteLine(result);

            result = currentTextInfo.ToTitleCase(testString);
            Console.WriteLine(result);

            result = englishTextInfo.ToTitleCase(testString);
            Console.WriteLine(result);
        }

        private static void StringAsArray()
        {
            string testString = "John";

            for (int i = 0; i < testString.Length; i++)
            {
                Console.WriteLine(testString[i]);
            }
        }

        private static void EscapeString()
        {
            string results;

            results = "This is my \"test\" solution";
            Console.WriteLine(results);

            results = "C:\\Demos\\Test.txt";
            Console.WriteLine(results);

            results = @"C:\Demos\Test.txt";
            Console.WriteLine(results);
        }

        private static void AppendingStrings()
        {
            string firstName = "John";
            string lastName = "Aldrin";
            string results;

            results = firstName + ", my name is " + firstName + " " + lastName;
            Console.WriteLine(results);

            results = string.Format("{0}, my name is {0} {1}", firstName, lastName);
            Console.WriteLine(results);

            Console.WriteLine("{0}, my name is {0} {1}", firstName, lastName);

            results = $"{firstName}, my name is {firstName} {lastName}";
            Console.WriteLine(results);
        }

        private static void InterpolationAndLiteral()
        {
            string testString = "John Aldrin";
            string results = $@"{"\""}C:\Demos\{testString}\Test.txt{"\""}";

            Console.WriteLine(results);
        }

        private static void StringBuilderDemo()
        {
            ////    String Concatination
            Stopwatch regularStopwatch = new();
            regularStopwatch.Start();

            string test = "";
            for (int i = 0; i < 100_000; i++)
            {
                test += i;
            }

            regularStopwatch.Stop();
            Console.WriteLine($"Regular Stopwatch: {regularStopwatch.ElapsedMilliseconds}ms.");

            ////    String Concatination
            Stopwatch builderStopwatch = new();
            builderStopwatch.Start();

            StringBuilder sb = new();
            for (int i = 0; i < 100_000; i++)
            {
                sb.Append(i);
            }

            builderStopwatch.Stop();
            Console.WriteLine($"Builder Stopwatch: {builderStopwatch.ElapsedMilliseconds}ms.");

            var test2 = sb.ToString();
            Console.WriteLine($"String Compare: {test == test2}");
        }

        private static void WorkingWithArrays()
        {
            int[] ages = new int[] { 6, 7, 8, 3, 5 };
            string results;

            results = string.Concat(ages);
            Console.WriteLine($"Concat: {results}");

            results = string.Join(",", ages);
            Console.WriteLine($"Join (CSV): {results}");

            Console.WriteLine();

            string testString = "Jon,Tim,Mary,Sue,Bob,Jane";
            string[] resultsArray = testString.Split(',');

            Array.ForEach(resultsArray, x => Console.WriteLine(x));
            Console.WriteLine();

            testString = "Jon, Tim, Mary, Sue, Bob, Jane";
            resultsArray = testString.Split(", ");

            Array.ForEach(resultsArray, x => Console.WriteLine(x));
        }
    }
}
