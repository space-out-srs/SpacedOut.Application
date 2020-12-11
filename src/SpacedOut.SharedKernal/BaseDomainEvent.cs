using System;

namespace SpacedOut.SharedKernel
{
    public abstract class BaseDomainEvent
    {
        public DateTime OccuredOnUtc { get; protected set; } = DateTime.UtcNow;
    }
}