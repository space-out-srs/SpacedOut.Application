using System;

namespace SpacedOut.SharedKernel
{
    public static class SystemTime
    {
        [ThreadStatic]
        private static DateTime? _dateTimeUtc;

        public static DateTime UtcNow()
        {
            if (_dateTimeUtc.HasValue)
            {
                return _dateTimeUtc.Value;
            }

            return DateTime.UtcNow;
        }

        public static IDisposable LockSystemTime(DateTime dateTimeUtc)
        {
            if (_dateTimeUtc.HasValue) throw new InvalidOperationException("SystemTime is already locked");

            _dateTimeUtc = dateTimeUtc;

            return new LockedDateTime();
        }

        private class LockedDateTime : IDisposable
        {
            public void Dispose()
            {
                _dateTimeUtc = null;

                GC.SuppressFinalize(this);
            }
        }
    }
}