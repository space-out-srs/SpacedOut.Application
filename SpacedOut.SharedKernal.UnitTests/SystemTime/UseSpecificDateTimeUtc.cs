using System;
using System.Threading.Tasks;
using Xunit;

namespace SpacedOut.SharedKernal.UnitTests.SystemTime
{
    public class UseSpecificDateTimeUtc
    {
        [Fact]
        public void ShouldUseGiveTime()
        {
            var dateTimeUtcToUse = new DateTime(2020, 12, 25, 9, 33, 28);

            Assert.NotEqual(dateTimeUtcToUse, SharedKernel.SystemTime.UtcNow);

            using (SharedKernel.SystemTime.UseSpecificDateTimeUtc(dateTimeUtcToUse))
            {
                Assert.Equal(dateTimeUtcToUse, SharedKernel.SystemTime.UtcNow);
            }

            Assert.NotEqual(dateTimeUtcToUse, SharedKernel.SystemTime.UtcNow);
        }

        [Fact]
        public void ShouldThrowError()
        {
            using (SharedKernel.SystemTime.UseSpecificDateTimeUtc(new DateTime(2020, 12, 25)))
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    SharedKernel.SystemTime.UseSpecificDateTimeUtc(new DateTime(2020, 01, 01));
                });
            }
        }

        [Fact]
        public void ShouldThreadCorrectly()
        {
            var dateTimeToUse = new DateTime(2020, 12, 25);

            using (SharedKernel.SystemTime.UseSpecificDateTimeUtc(dateTimeToUse))
            {
                Assert.Equal(dateTimeToUse, SharedKernel.SystemTime.UtcNow);

                var threadedDateTime = Task.Run(() =>
                {
                    return SharedKernel.SystemTime.UtcNow;
                }).Result;

                Assert.NotEqual(dateTimeToUse, threadedDateTime);
            }
        }
    }
}