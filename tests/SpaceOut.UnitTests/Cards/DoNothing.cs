using Xunit;

namespace SpaceOut.UnitTests.Cards
{
    public class DoNothing
    {
        [Fact]
        public void DoesNothing()
        {
            var card = new CardBuilder()
                .WithDefaultValues()
                .Build();

            Assert.True(card != null); // yes, this basically does nothing, but this is just a placeholder
        }
    }
}
