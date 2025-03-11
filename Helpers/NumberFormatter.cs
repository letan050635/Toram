using System;
using System.Globalization;

namespace ToramFillCalculator.Helpers
{
    public static class NumberFormatter
    {
        public static string FormatNumber(int value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }

        public static int ParseNumber(string text)
        {
            if (int.TryParse(text, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int value))
            {
                return value;
            }
            return 0;
        }
    }
}