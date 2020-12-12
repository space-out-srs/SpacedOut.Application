using Moq;
using SpacedOut.SharedKernal.Interfaces;

namespace SpacedOut.Api.UnitTests.CardsController
{
    public abstract class CardsControllerBase
    {
        protected readonly Cards.CardsController _cardsController;
        protected readonly Mock<IRepository> _repositoryMock;
        protected readonly Mock<IUnitOfWork> _uowMock;

        public CardsControllerBase()
        {
            _repositoryMock = new Mock<IRepository>();

            var uowFactory = new Mock<IUnitOfWorkFactory>();

            _uowMock = GetUnitOfWork(uowFactory);

            _cardsController = new Cards.CardsController(
                _repositoryMock.Object,
                uowFactory.Object
            );
        }

        private static Mock<IUnitOfWork> GetUnitOfWork(Mock<IUnitOfWorkFactory> uowFactory)
        {
            var uow = new Mock<IUnitOfWork>();

            uowFactory
                .Setup(u => u.Begin())
                .Returns(uow.Object);

            return uow;
        }
    }
}