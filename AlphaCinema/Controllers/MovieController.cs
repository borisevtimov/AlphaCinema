using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Controllers
{
    public class MovieController : BaseController
    {
        private readonly IMovieService movieService;
        private readonly ILogger<MovieController> logger;

        public MovieController(IMovieService movieService, ILogger<MovieController> logger)
        {
            this.movieService = movieService;
            this.logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Active()
        {
            IList<ActiveMovieMainInfoVM> activeMovies = await movieService.GetAllActiveMoviesMainInfoAsync();

            return View(activeMovies);
        }

        [AllowAnonymous]
        public async Task<IActionResult> MoreInfo(int id)
        {
            try
            {
                MovieFullInfoVM movie = await movieService.GetMovieFullInfoByIdAsync(id);
                return View(movie);
            }
            catch (ArgumentException ae)
            {
                ViewData[MessageConstant.ErrorMessage] = ae.Message;
                logger.LogWarning(ae.Message);
                return RedirectToAction(nameof(Active));
            }
        }
    }
}
