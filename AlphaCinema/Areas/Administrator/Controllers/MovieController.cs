using AlphaCinema.Core.Constants;
using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AlphaCinema.Areas.Administrator.Controllers
{
    public class MovieController : BaseController
    {
        private readonly ILogger<MovieController> logger;
        private readonly IMovieService movieService;

        public MovieController(ILogger<MovieController> logger, IMovieService movieService)
        {
            this.logger = logger;
            this.movieService = movieService;
        }

        public async Task<IActionResult> Info() 
        {
            IList<MovieMainInfoVM> movies = await movieService.GetAllMoviesMainInfoAsync();

            return View(movies);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMovieVM model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    logger.LogError(error.ErrorMessage);
                }

                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.InvalidInput;
                return View();
            }

            try
            {
                await movieService.AddMovieAsync(model);
            }
            catch (ArgumentException ae)
            {
                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.InvalidInput;
                logger.LogError(ae.Message);
                return View();
            }
            catch (Exception e)
            {
                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.UnexpectedError;
                logger.LogError(e.Message);
                return View();
            }

            return RedirectToAction(nameof(Info));
        }
    }
}
