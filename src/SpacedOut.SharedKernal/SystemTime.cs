using System;

namespace SpacedOut.SharedKernel
{
    public static class SystemTime
    {
        private static DateTime? _dateTimeUtc;

        public static DateTime UtcNow()
        {
            if (_dateTimeUtc.HasValue)
            {
                return _dateTimeUtc.Value;
            }

            return DateTime.UtcNow;
        }
    }
}