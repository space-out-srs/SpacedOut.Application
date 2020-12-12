using SpacedOut.SharedKernal.Interfaces;
using System;

namespace SpacedOut.Infrastucture
{
    public class DateService : IDateService
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}