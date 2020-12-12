using SpacedOut.Domain.Cards;
using SpacedOut.Domain.Cards.Handlers;
using SpacedOut.Infrastucture.Processing.Outbox;
using SpacedOut.SharedKernal.Interfaces;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpacedOut.Infrastucture.Domain.Cards
{
    internal class CardCreatedNotification : BaseOutboxMessageHandler, ICardCreatedNotification
    {
        public CardCreatedNotification(IDateService dateService, IRepository repository) : base(dateService, repository)
        {
        }

        public override string Key => "CardCreatedNotification";

        public async override Task Process(string json)
        {
            await Task.Run(() =>
            {
                var data = JsonSerializer.Deserialize<Data>(json);

                Debug.WriteLine(data);

                // TODO: do something here like send an email 🤷‍
            });
        }

        public async Task Send(Card card)
        {
            var dataModel = new Data
            {
                CardId = card.Id
            };

            await Queue(JsonSerializer.Serialize(dataModel));
        }

        private class Data
        {
            public int CardId { get; init; }
        }
    }
}