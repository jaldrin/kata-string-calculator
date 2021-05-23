namespace KataStringCalc
{
    public class StringCalc
    {
        public int Add(string numbers)
        {
            if (string.IsNullOrWhiteSpace(numbers)) return 0;

            return int.Parse(numbers);
        }
    }
}
