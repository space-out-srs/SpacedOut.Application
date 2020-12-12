using FluentAssertions;
using Moq;
using SpacedOut.Domain.Cards;
using SpacedOut.Domain.Schedules;
using System.Threading.Tasks;
using Xunit;

namespace SpacedOut.Api.UnitTests.CardsController
{
    public class Add : CardsControllerBase
    {
        [Fact]
        public async Task ShouldAddAndReturnNewCard()
        {
            var sampleCard = GetSampleCard();

            _uowMock
                .Setup(u => u.AddAsync(It.IsAny<Card>()))
                .ReturnsAsync(sampleCard);

            var card = await _cardsController.Add();

            card.Should().Be(sampleCard);

            _uowMock.Verify(r => r.AddAsync(It.IsAny<Card>()), Times.Once);
            _uowMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        private static Card GetSampleCard()
        {
            return new Card(ScheduleEnum.LEITNER);
        }
    }
}