using Moq;
using SpacedOut.Domain.Cards.Events;
using SpacedOut.Domain.Cards.Handlers;
using System.Threading.Tasks;
using Xunit;
using D = SpacedOut.Domain.Cards.Handlers;

namespace SpacedOut.Domain.UnitTests.Cards
{
    public class CardCreatedDemoHandler
    {
        private readonly D.CardCreatedDemoHandler _handler;
        private readonly Mock<ICardCreatedNotification> _notificationMock;

        public CardCreatedDemoHandler()
        {
            _notificationMock = new Mock<ICardCreatedNotification>();
            _handler = new D.CardCreatedDemoHandler(_notificationMock.Object);
        }

        // This is no longer needed because `nullable` is enabled and warnings are errors
        //[Fact]
        //public async Task ShouldThrowArgumentNullException()
        //{
        //    Exception ex = await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(null));
        //}

        [Fact]
        public async Task ShouldTriggerNotificationOnce()
        {
            var card = new CardBuilder()
                .WithDefaultValues()
                .Build();

            await _handler.Handle(new CardCreatedEvent(card));

            _notificationMock.Verify(sender => sender.Send(card), Times.Once);
        }
    }
}
