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

        public async Task<IActionResult> MoreInfo(int id)
        {
            MoreMovieInfoVM? model = null;

            try
            {
                model = await movieService.GetMovieByIdAsync(id);
            }
            catch (ArgumentException ae)
            {
                logger.LogError(ae.Message);
                ViewData[MessageConstant.ErrorMessage] = ae.Message;
                return View();
            }
            catch (Exception e)
            {
                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.UnexpectedError;
                logger.LogError(e.Message);
                return View();
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            EditMovieVM? model = null;

            try
            {
                model = await movieService.GetMovieForEditByIdAsync(id);
            }
            catch (ArgumentException ae)
            {
                logger.LogWarning(ae.Message);
                ViewData[MessageConstant.ErrorMessage] = ae.Message;
                return View();
            }
            catch (Exception e)
            {
                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.UnexpectedError;
                logger.LogWarning(e.Message);
                return View();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMovieVM model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    logger.LogWarning(error.ErrorMessage);
                }

                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.InvalidInput;
                return View(new { id = model.MovieId });
            }

            try
            {
                await movieService.EditMovieAsync(model);
            }
            catch (ArgumentException ae)
            {
                logger.LogWarning(ae.Message);
                ViewData[MessageConstant.ErrorMessage] = ae.Message;
                return RedirectToAction(nameof(Edit), new { id = model.MovieId });
            }
            catch (Exception e)
            {
                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.UnexpectedError;
                logger.LogWarning(e.Message);
                return RedirectToAction(nameof(Edit), new { id = model.MovieId });
            }

            ViewData[MessageConstant.SuccessMessage] = "Movie changed successfully";
            return RedirectToAction(nameof(Info));
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
                    logger.LogWarning(error.ErrorMessage);
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
                logger.LogWarning(ae.Message);
                return View();
            }
            catch (Exception e)
            {
                ViewData[MessageConstant.ErrorMessage] = ExceptionConstant.UnexpectedError;
                logger.LogWarning(e.Message);
                return View();
            }

            return RedirectToAction(nameof(Info));
        }
    }
}
