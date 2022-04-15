using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IMovieService movieService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IMovieService movieService, ILogger<HomeController> logger)
        {
            this.movieService = movieService;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IList<ActiveMovieMainInfoVM> activeMovies = await movieService.GetAllActiveMoviesMainInfoAsync();

            return View(activeMovies);
        }
    }
}