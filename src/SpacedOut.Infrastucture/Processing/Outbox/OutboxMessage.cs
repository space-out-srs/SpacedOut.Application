using SpacedOut.SharedKernel;
using System;

namespace SpacedOut.Infrastucture.Processing.Outbox
{
    public class OutboxMessage : BaseEntity
    {
        public DateTime OccurredOnUtc { get; private set; }
        public DateTime ProcessOnUtc { get; private set; }
        public DateTime? ProcessedOnUtc { get; private set; } = null;
        public int FailedAttempts { get; private set; } = 0;
        public string Key { get; private set; } = null!;
        public string Data { get; private set; } = null!;

        private OutboxMessage() { }
        public OutboxMessage(string type, string key, DateTime processOnUtc)
        {
            OccurredOnUtc = SystemTime.UtcNow();
            ProcessOnUtc = processOnUtc;
            Key = type;
            Data = key;
        }

        public void MarkProcessed()
        {
            ProcessedOnUtc = SystemTime.UtcNow();
        }

        public void MarkFailed()
        {
            FailedAttempts += 1;
            ProcessOnUtc = SystemTime
                .UtcNow()
                .AddHours(1);
        }
    }
}