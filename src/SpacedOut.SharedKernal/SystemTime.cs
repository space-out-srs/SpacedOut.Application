using System;

namespace SpacedOut.SharedKernel
{
    public static class SystemTime
    {

        public static readonly Func<DateTime> UtcNow = () => DateTime.UtcNow;
    }
}