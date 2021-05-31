using System;
using System.Globalization;

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
            InterpolationAndLiteral();
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
    }
}
