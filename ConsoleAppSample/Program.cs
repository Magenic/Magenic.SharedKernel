using System;
using Magenic.SharedKernel;

namespace ConsoleAppSample
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using (SecureRandom sr = SecureRandom.Create())
            {
                Console.WriteLine($"Random number: {sr.Next()}");
            }
        }
    }
}