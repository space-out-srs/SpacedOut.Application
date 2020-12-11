using Microsoft.AspNetCore.Mvc;
using SpacedOut.Domain.Cards;
using SpacedOut.Domain.Schedules;
using SpacedOut.SharedKernal.Interfaces;
using SpaceOut.Api.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceOut.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IUnitOfWorkFactory _uowFactory;
        public CardController(IRepository repository, IUnitOfWorkFactory uowFactory)
        {
            _repository = repository;
            _uowFactory = uowFactory;
        }

        [HttpGet]
        public async Task<IEnumerable<CardModel>> Get()
        {
            var cards = (await _repository.ListAsync<Card>())
                .Select(CardModel.FromCard)
                .ToList();

            return cards;
        }

        [HttpGet]
        [Route("GetWithTransaction")]
        public async Task<IEnumerable<CardModel>> GetWithTransaction()
        {
            using (var uow = _uowFactory.Begin())
            {
                await uow.AddAsync(new Card(ScheduleEnum.LEITNER));

                //await repository.RollbackAsync();

                //var cards = (await repository.ListAsync<Card>())
                //    .Select(CardModel.FromCard)
                //    .ToList();

                //await repository.RollbackAsync();
                await uow.CommitAsync();

                //return cards;
            }

            var cards = (await _repository.ListAsync<Card>())
                .Select(CardModel.FromCard)
                .ToList();

            return cards;
        }

        [HttpGet]
        [Route("Add")]
        public async Task<Card> Add()
        {
            using (var uow = _uowFactory.Begin())
            {
                var card = new Card(ScheduleEnum.LEITNER);

                await uow.AddAsync(card);

                await uow.CommitAsync();

                return card;
            }
        }
    }
}