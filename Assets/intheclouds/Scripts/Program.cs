using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeInterviewPractice
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            Console.WriteLine(ReturnEvenNumbers(numbers));
            
            var names = new List<string> { "alice", "bob", "charlie" };
            Console.WriteLine(ConvertStringsToUppercase(names));
            

        }

        // Find even numbers in a list
        public static string ReturnEvenNumbers(List<int> numbers)
        {
            var evenNums = numbers.Where(x => x % 2 == 0);
            string output = "{" + string.Join(", ", evenNums) + "}";
            return output;
        }

        public static string ConvertStringsToUppercase(List<string> names)
        {
            var uppercaseNames = names.Select(x => x.ToUpper());
            string output = string.Join(", ", uppercaseNames);
            return output;
        }
    }
}