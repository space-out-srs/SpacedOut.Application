using SpacedOut.SharedKernal.Interfaces;
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
        public OutboxMessage(IDateService dateService, string type, string key, DateTime processOnUtc)
        {
            OccurredOnUtc = dateService.GetUtcNow();
            ProcessOnUtc = processOnUtc;
            Key = type;
            Data = key;
        }

        public void MarkProcessed(IDateService dateService)
        {
            ProcessedOnUtc = dateService.GetUtcNow();
        }

        public void MarkFailed(IDateService dateService)
        {
            FailedAttempts += 1;
            ProcessOnUtc = dateService
                .GetUtcNow()
                .AddHours(1);
        }
    }
}