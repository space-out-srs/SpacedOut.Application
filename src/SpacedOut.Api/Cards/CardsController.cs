using Microsoft.AspNetCore.Mvc;
using SpacedOut.Api.Cards.Models;
using SpacedOut.Domain.Cards;
using SpacedOut.Domain.Schedules;
using SpacedOut.SharedKernal.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpacedOut.Api.Cards
{
    [ApiController]
    [Route("[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IUnitOfWorkFactory _uowFactory;
        public CardsController(IRepository repository, IUnitOfWorkFactory uowFactory)
        {
            _repository = repository;
            _uowFactory = uowFactory;
        }

        [HttpGet]
        public async Task<IEnumerable<CardViewModel>> Get()
        {
            var cards = (await _repository.ListAsync<Card>())
                .Select(CardViewModel.FromCard)
                .ToList();

            return cards;
        }

        [HttpGet]
        [Route("Add")]
        public async Task<Card> Add()
        {
            using (var uow = _uowFactory.Begin())
            {
                var card = await uow.AddAsync(new Card(ScheduleEnum.LEITNER));

                await uow.CommitAsync();

                return card;
            }
        }
    }
}