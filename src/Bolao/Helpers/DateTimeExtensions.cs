using System;
using System.Linq;

namespace Bolao.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime ToBrasiliaTime(this DateTime dateTime)
        {
            // Ensure the input DateTime is treated as UTC to convert it properly.
            // If it is Unspecified, SpecifyKind as UTC. If Local, convert to UTC first.
            var utcDateTime = dateTime.Kind == DateTimeKind.Utc 
                ? dateTime 
                : (dateTime.Kind == DateTimeKind.Local 
                    ? dateTime.ToUniversalTime() 
                    : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));

            try
            {
                // Windows uses "E. South America Standard Time", Linux/Mac (Render) uses "America/Sao_Paulo"
                var tz = TimeZoneInfo.GetSystemTimeZones()
                    .FirstOrDefault(t => t.Id == "America/Sao_Paulo" || t.Id == "E. South America Standard Time");

                if (tz != null)
                {
                    return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, tz);
                }
            }
            catch
            {
                // Fallback inside catch block
            }

            // Fallback to static GMT-3 offset if system timezone database is unavailable
            return utcDateTime.AddHours(-3);
        }
    }
}
