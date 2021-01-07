using SpacedOut.SharedKernal.Interfaces;
using System;
using System.Threading.Tasks;

namespace SpacedOut.Infrastucture.Processing.Outbox
{
    internal abstract class BaseOutboxMessageHandler
    {
        private readonly IRepository _repository;

        public BaseOutboxMessageHandler(IRepository repository)
        {
            _repository = repository;
        }

        public abstract string Key { get; }
        public abstract Task Process(string data);

        public async Task Queue(string data, DateTime? processOnUtc = null)
        {
            var outboxMessage = new OutboxMessage(
                Key,
                data,
                processOnUtc ?? SharpTime.UtcNow
            );

            await _repository.AddAsync(outboxMessage);
        }
    }
}