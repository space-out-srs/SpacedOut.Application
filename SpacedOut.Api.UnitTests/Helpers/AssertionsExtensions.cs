using Xunit.Sdk;

namespace SpacedOut.Api.UnitTests.Helpers
{
    public static class Assert
    {
        public static void Fail(string message)
        {
            throw new XunitException(message);
        }
    }
}