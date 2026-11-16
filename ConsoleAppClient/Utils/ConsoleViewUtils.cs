using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ConsoleClient.Utils
{
    internal static class ConsoleViewUtils
    {
        public static int SafeInputInt()
        {
            string rawInput = System.Console.ReadLine() ?? string.Empty;

            while (!int.TryParse(rawInput, out int input))
            {
                System.Console.WriteLine("Not a valid number, try again:");
                rawInput = System.Console.ReadLine() ?? string.Empty;
            }

            return int.Parse(rawInput);
        }

        public static double SafeInputDouble()
        {
            string rawInput = System.Console.ReadLine() ?? string.Empty;

            while (!double.TryParse(rawInput.Replace(',', '.'),
                       NumberStyles.Any,
                       CultureInfo.InvariantCulture,
                       out double input))
            {
                System.Console.WriteLine("Not a valid decimal number, try again:");
                rawInput = System.Console.ReadLine() ?? string.Empty;
            }

            return double.Parse(rawInput.Replace(',', '.'), CultureInfo.InvariantCulture);
        }

        public static DateTime SafeInputDate()
        {
            string rawInput = System.Console.ReadLine() ?? string.Empty;
            string[] formats = { "dd.MM.yyyy.", "dd.MM.yyyy", "yyyy-MM-dd" };

            while (!DateTime.TryParseExact(rawInput,
                       formats,
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime date))
            {
                System.Console.WriteLine("Not a valid date. Use dd.MM.yyyy. or yyyy-MM-dd:");
                rawInput = System.Console.ReadLine() ?? string.Empty;
            }

            return DateTime.ParseExact(rawInput, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static List<int> ParseIntList(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<int>();

            return input
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .ToList();
        }
    }
}
