using System;
using System.Globalization;

namespace DeployImporter
{
    public static class Extensions
    {
        private const string DeployTimeFormat = "M/d/yyyy H:mm:ss";

        public static decimal? TryParseDecimal(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            decimal parsed;
            return Decimal.TryParse(value, out parsed)
                       ? parsed
                       : (decimal?)null;
        }

        public static int? TryParseInt(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            int parsed;
            return Int32.TryParse(value, out parsed)
                       ? parsed
                       : (int?) null;
        }

        public static DateTime? TryParseDateTime(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            
            DateTime parsed;
            return DateTime.TryParseExact(value, DeployTimeFormat, CultureInfo.CurrentCulture,
                                          DateTimeStyles.AssumeLocal | DateTimeStyles.AdjustToUniversal, out parsed)
                       ? parsed
                       : (DateTime?)null;
        }
    }
}