using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Areas.Administrator.Controllers
{
    public class MovieController : BaseController
    {
        private readonly IMovieService movieService;

        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        public async Task<IActionResult> Info() 
        {
            IList<MovieMainInfoVM> movies = await movieService.GetAllMoviesMainInfoAsync();

            return View(movies);
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddMovieVM model)
        {
            Console.WriteLine(Request);
            return View();
        }
    }
}
