using AlphaCinema.Core.Contracts;
using AlphaCinema.Infrastructure.Data.Common;

namespace AlphaCinema.Core.Services
{
    public class CardService : ICardService
    {
        private readonly IRepository repository;

        public CardService(IRepository repository)
        {
            this.repository = repository;
        }
    }
}
