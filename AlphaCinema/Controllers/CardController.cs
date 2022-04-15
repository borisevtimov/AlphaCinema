using AlphaCinema.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Controllers
{
    public class CardController : BaseController
    {
        private readonly ICardService cardService;

        public CardController(ICardService cardService)
        {
            this.cardService = cardService;
        }

        public IActionResult All()
        {
            return View();
        }
    }
}
