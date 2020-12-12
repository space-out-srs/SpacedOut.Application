using SpacedOut.Domain.Cards.Events;
using System.Linq;
using Xunit;

namespace SpacedOut.UnitTests.Cards
{
    public class Constructor
    {
        [Fact]
        public void RaisesCardCreatedEvent()
        {
            var card = new CardBuilder()
                .WithDefaultValues()
                .Build();

            Assert.Single(card.Events);
            Assert.IsType<CardCreatedEvent>(card.Events.First());
        }
    }
}
