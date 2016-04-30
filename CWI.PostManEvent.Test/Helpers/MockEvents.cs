using CWI.PostManEvent.Common.Hubs;
using Moq;

namespace CWI.PostManEvent.Test.Helpers
{
    public class MockEvents
    {
        public static Mock<IHubEvent> Hub()
        {
            Mock<IHubEvent> moq = new Mock<IHubEvent>();

            return moq;
        }

    }
}
