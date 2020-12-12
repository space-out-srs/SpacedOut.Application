using Xunit;

namespace SpacedOut.Domain.UnitTests.Cards
{
    public class DoNothing
    {
        [Fact]
        public void ShouldDoNothing()
        {
            var card = new CardBuilder()
                .WithDefaultValues()
                .Build();

            Assert.True(card != null); // yes, this basically does nothing, but this is just a placeholder
        }
    }
}
