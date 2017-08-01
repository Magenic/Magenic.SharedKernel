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

            Util.Repeat(() => ClearSpace(), 2);

            Console.WriteLine(stars);

            Flow.Using(
                SecureRandom.Create(),
                sr => Seq.List(
                    $"Random int: {sr.Next()}.",
                    $"Random int with max {max}: {sr.Next(max)}.",
                    $"Random int with min {min} and max {max}: {sr.Next(min, max)}.",
                    $"Random bool: {sr.NextBool()}.",
                    $"Random byte: {sr.NextByte()}.",
                    $"Random char: {sr.NextChar()}.",
                    $"Random decimal: {sr.NextDecimal()}.",
                    $"Random double: {sr.NextDouble()}.",
                    $"Random long: {sr.NextLong()}.",
                    $"Random short: {sr.NextShort()}.",
                    $"Random alphanumeric string of length {len}: {sr.NextString(16)}.")
                        .Apply(s => Console.WriteLine(s)));

            Console.WriteLine(Seq.List(
                "Hit Enter to exit console application.",
                stars).JoinWithNewLine());

            ClearSpace();

            Console.ReadLine();
        }
    }
}