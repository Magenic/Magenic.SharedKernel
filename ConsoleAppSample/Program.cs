using System;

using Magenic.SharedKernel;

namespace ConsoleAppSample
{
    internal static class Program
    {
        private static void ClearSpace()
        {
            Util.Repeat(() => Console.WriteLine(), 10);
        }

        private static void Main(string[] args)
        {
            string stars = StringEx.CreateString('*', 128);
            int min = 16;
            int max = 64;
            int len = 16;

            ClearSpace();
            ClearSpace();

            Console.WriteLine(stars);

            using (SecureRandom sr = SecureRandom.Create())
            {
                Console.WriteLine($"Random int: {sr.Next()}.");
                Console.WriteLine($"Random int with max {max}: {sr.Next(max)}.");
                Console.WriteLine(
                    $"Random int with min {min} and max {max}: {sr.Next(min, max)}.");
                Console.WriteLine($"Random bool: {sr.NextBool()}.");
                Console.WriteLine($"Random byte: {sr.NextByte()}.");
                Console.WriteLine($"Random char: {sr.NextChar()}.");
                Console.WriteLine($"Random decimal: {sr.NextDecimal()}.");
                Console.WriteLine($"Random double: {sr.NextDouble()}.");
                Console.WriteLine($"Random long: {sr.NextLong()}.");
                Console.WriteLine($"Random short: {sr.NextShort()}.");
                Console.WriteLine(
                    $"Random alphanumeric string of length {len}: {sr.NextString(16)}.");
            }

            Console.WriteLine("Hit Enter to exit console application.");
            Console.WriteLine(stars);
            ClearSpace();
            Console.ReadLine();
        }
    }
}