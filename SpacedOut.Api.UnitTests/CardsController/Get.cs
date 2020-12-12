using FluentAssertions;
using Moq;
using SpacedOut.Domain.Cards;
using SpacedOut.Domain.Schedules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SpacedOut.Api.UnitTests.CardsController
{
    public class Get : CardsControllerBase
    {
        [Fact]
        public async Task ShouldReturnEmptyListWhenNoCards()
        {
            _repositoryMock
                .Setup(r => r.ListAsync<Card>())
                .ReturnsAsync(new List<Card>());

            var cards = await _cardsController.Get();

            cards.Should().BeEmpty();

            _repositoryMock.Verify(r => r.ListAsync<Card>(), Times.Once);
        }

        [Fact]
        public async Task ShouldReturnCards()
        {
            var sample = GetSampleCards().ToList();

            _repositoryMock
                .Setup(r => r.ListAsync<Card>())
                .ReturnsAsync(sample);

            var cards = await _cardsController.Get();

            cards.Should().HaveCount(sample.Count);

            _repositoryMock.Verify(r => r.ListAsync<Card>(), Times.Once);
        }

        private static IEnumerable<Card> GetSampleCards()
        {
            var random = new Random();
            for (int i = 0; i < random.Next(2, 10); i++)
            {
                yield return new Card(ScheduleEnum.LEITNER);
            }
        }
    }
}