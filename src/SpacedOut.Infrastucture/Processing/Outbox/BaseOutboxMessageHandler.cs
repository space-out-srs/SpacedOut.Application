using SpacedOut.SharedKernal.Interfaces;
using System;
using System.Threading.Tasks;

namespace SpacedOut.Infrastucture.Processing.Outbox
{
    internal abstract class BaseOutboxMessageHandler
    {
        private readonly IDateService _dateService;
        private readonly IRepository _repository;

        public BaseOutboxMessageHandler(IDateService dateService, IRepository repository)
        {
            _dateService = dateService;
            _repository = repository;
        }

        public abstract string Key { get; }
        public abstract Task Process(string data);

        public async Task Queue(string data, DateTime? processOnUtc = null)
        {
            var outboxMessage = new OutboxMessage(
                _dateService,
                Key,
                data,
                processOnUtc ?? _dateService.GetUtcNow()
            );

            await _repository.AddAsync(outboxMessage);
        }
    }
}